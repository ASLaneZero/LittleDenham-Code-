using StardewModdingAPI;
using System;
using System.Collections.Generic;
using StardewValley;
using Microsoft.Xna.Framework;

namespace LittleDenham
{
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
                    Game1.warpFarmer("Custom_LDLobby", 14, 16, true);
                });
            }
            if (!tileActionString.Contains("2"))
            {
                choices.Add(new Response("loc2", Helper.Translation.Get("LD.Lift.Location.2")));
                selectionActions.Add(delegate
                {
                    Game1.playSound("crystal");
                    Game1.warpFarmer("Custom_LDLobby", 14, 10, true);
                });
            }
            if (!tileActionString.Contains("3"))
            {
                choices.Add(new Response("loc3", Helper.Translation.Get("LD.Lift.Location.3")));
                selectionActions.Add(delegate
                {
                    Game1.playSound("crystal");
                    Game1.warpFarmer("Custom_LDLobby", 14, 4, true);
                });
            }
            choices.Add(new Response("cancel", Helper.Translation.Get("LD.Lift.Exit")));
            selectionActions.Add(delegate { });
            Game1.activeClickableMenu = new DialogueBoxWithActions(Helper.Translation.Get("LD.Lift.Question"), choices, selectionActions);
        }

    }
}
