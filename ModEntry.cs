using StardewModdingAPI;
using System;
using System.Collections.Generic;
using StardewValley;
using StardewModdingAPI.Events;
using Microsoft.Xna.Framework;
using StardewValley.Menus;
using System.Linq;

namespace LittleDenham
{
    public class ModEntry : Mod
    {
        internal static IMonitor ModMonitor { get; set; }
        internal new static IModHelper Helper { get; set; }
        public override void Entry(IModHelper helper)
        {
            ModMonitor = Monitor;
            Helper = helper;

            Lifts.Initialize(this);
            TileActionHandler.Initialize(Helper);

        }
    }

    internal static class Log
    {
        internal static void Error(string msg) => ModEntry.ModMonitor.Log(msg, StardewModdingAPI.LogLevel.Error);
        internal static void Alert(string msg) => ModEntry.ModMonitor.Log(msg, StardewModdingAPI.LogLevel.Alert);
        internal static void Warn(string msg) => ModEntry.ModMonitor.Log(msg, StardewModdingAPI.LogLevel.Warn);
        internal static void Info(string msg) => ModEntry.ModMonitor.Log(msg, StardewModdingAPI.LogLevel.Info);
        internal static void Debug(string msg) => ModEntry.ModMonitor.Log(msg, StardewModdingAPI.LogLevel.Debug);
        internal static void Trace(string msg) => ModEntry.ModMonitor.Log(msg, StardewModdingAPI.LogLevel.Trace);
        internal static void Verbose(string msg) => ModEntry.ModMonitor.VerboseLog(msg);
    }

    internal static class UtilFunctions
    {
        internal static void StartEvent(StardewValley.Event EventObj, string locationName, int x, int y)
        {
            if (Game1.currentLocation.Name.Equals(locationName))
            {
                Game1.delayedActions.Add(new DelayedAction(500, delegate
                {
                    Game1.currentLocation.startEvent(EventObj);
                }));
                Game1.fadeScreenToBlack();
            }
            else
            {

                LocationRequest warpRequest = Game1.getLocationRequest(locationName);
                warpRequest.OnLoad += delegate
                {
                    Game1.currentLocation.currentEvent = EventObj;
                };
                Game1.warpFarmer(warpRequest, x, y, Game1.player.FacingDirection);
            }

        }

        public static IEnumerable<Point> YieldSurroundingTiles(Vector2 tile, int radius = 1)
        {
            int x = (int)tile.X;
            int y = (int)tile.Y;
            for (int xdiff = -radius; xdiff <= radius; xdiff++)
            {
                for (int ydiff = -radius; ydiff <= radius; ydiff++)
                {
                    yield return new Point(x + xdiff, y + ydiff);
                }
            }
        }

        public static IEnumerable<Vector2> YieldAllTiles(GameLocation location)
        {
            for (int x = 0; x < location.Map.Layers[0].LayerWidth; x++)
            {
                for (int y = 0; y < location.Map.Layers[0].LayerHeight; y++)
                {
                    yield return new Vector2(x, y);
                }
            }
        }

        public static List<string> ContextSort(IEnumerable<string> enumerable)
        {
            List<string> outputlist = enumerable.ToList();
            outputlist.Sort(GetCurrentLanguageComparer(ignoreCase: true));
            return outputlist;
        }

        public static StringComparer GetCurrentLanguageComparer(bool ignoreCase = false)
            => StringComparer.Create(Game1.content.CurrentCulture, ignoreCase);
    }

    public class TileActionHandler
    {

        static Dictionary<string, Action<string, Vector2>> tileActions = new Dictionary<string, Action<string, Vector2>>();

        static IModHelper Helper;
        internal static void Initialize(IModHelper Helper)
        {
            TileActionHandler.Helper = Helper;
            TileActionHandler.Helper.Events.Input.ButtonPressed += OnButtonPressed;
        }

        internal static void RegisterTileAction(string name, Action<string, Vector2> actionFunction)
        {
            Log.Trace($"Registered {name}");
            tileActions.Add(name, actionFunction);
        }

        private static void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {


            if (!e.Button.IsActionButton())
                return;
            if (!Context.IsWorldReady)
                return;

            bool probablyDontCheck =
            !StardewModdingAPI.Context.CanPlayerMove
            || Game1.player.isRidingHorse()
            || Game1.currentLocation == null
            || Game1.eventUp
            || Game1.isFestival()
            || Game1.IsFading()
            || Game1.menuUp;

            if (probablyDontCheck)
            {
                return;
            }

            Vector2 clickedTile = Vector2.Zero;
            string actionString = "";
            bool usingGamepad = Game1.options.gamepadControls;
            if (usingGamepad)
            {
                clickedTile = Utility.clampToTile(Game1.player.GetToolLocation(Helper.Input.GetCursorPosition().ScreenPixels)) / 64f;
                actionString = Game1.currentLocation.doesTileHaveProperty(((int)clickedTile.X), ((int)clickedTile.Y), "Action", "Buildings");
            }
            if (!usingGamepad || String.IsNullOrWhiteSpace(actionString))
            {
                clickedTile = Helper.Input.GetCursorPosition().GrabTile;
                actionString = Game1.currentLocation.doesTileHaveProperty(((int)clickedTile.X), ((int)clickedTile.Y), "Action", "Buildings");

            }

            if (actionString != null && actionString != "")
            {
                Log.Trace($"Checking for {actionString}");
                foreach (var key in tileActions.Keys)
                {
                    if (actionString.StartsWith(key))
                    {
                        tileActions[key](actionString, clickedTile);
                        break;
                    }
                }
            }
        }
    }

    internal class DialogueBoxWithActions : DialogueBox
    {
        private List<Action> ResponseActions;

        internal DialogueBoxWithActions(string dialogue, List<Response> responses, List<Action> Actions) : base(dialogue, responses)
        {
            this.ResponseActions = Actions;
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            int responseIndex = this.selectedResponse;
            base.receiveLeftClick(x, y, playSound);
            if (base.safetyTimer <= 0 && responseIndex > -1 && responseIndex < this.ResponseActions.Count && this.ResponseActions[responseIndex] != null)
            {
                this.ResponseActions[responseIndex]();
            }
        }
    }

    internal static class Lifts
    {
        static IModHelper Helper;
        internal static void Initialize(IMod ModInstance)
        {
            Helper = ModInstance.Helper;

            TileActionHandler.RegisterTileAction("LDLift", OpenLiftDialogue);
        }
        internal static void OpenLiftDialogue(string tileActionString, Vector2 position)
        {
            var choices = new List<Response>();
            var selectionActions = new List<Action>();
            if (!tileActionString.Contains("1"))
            {
                choices.Add(new Response("loc1", Helper.Translation.Get("LD.Lift.Location.1")));
                selectionActions.Add(delegate
                {
                    Game1.playSound("crystal");
                    Game1.warpFarmer("Custom_LDLobby", 14, 16, false);
                });
            }
            if (!tileActionString.Contains("2"))
            {
                choices.Add(new Response("loc2", Helper.Translation.Get("LD.Lift.Location.2")));
                selectionActions.Add(delegate
                {
                    Game1.playSound("crystal");
                    Game1.warpFarmer("Custom_LDLobby", 14, 10, false);
                });
            }
            if (!tileActionString.Contains("3"))
            {
                choices.Add(new Response("loc3", Helper.Translation.Get("LD.Lift.Location.3")));
                selectionActions.Add(delegate
                {
                    Game1.playSound("crystal");
                    Game1.warpFarmer("Custom_LDLobby", 14, 4, false);
                });
            }
            choices.Add(new Response("cancel", Helper.Translation.Get("LD.Lift.Exit")));
            selectionActions.Add(delegate { });
            Game1.activeClickableMenu = new DialogueBoxWithActions(Helper.Translation.Get("LD.Lift.Question"), choices, selectionActions);
        }
    }
}