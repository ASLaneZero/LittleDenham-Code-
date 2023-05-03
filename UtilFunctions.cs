using System;
using System.Collections.Generic;
using StardewValley;
using Microsoft.Xna.Framework;
using System.Linq;

namespace LittleDenham
{
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
}