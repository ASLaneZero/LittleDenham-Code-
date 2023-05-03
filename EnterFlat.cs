using StardewModdingAPI;
using StardewValley;
using Microsoft.Xna.Framework;

namespace LittleDenham
{
    internal static class EnterFlat
    {
        static IModHelper Helper;
        internal static void Initialize(IMod ModInstance)
        {
            Helper = ModInstance.Helper;

            TileActionHandler.RegisterTileAction("LDFlat", FlatMessage);
        }
        internal static void FlatMessage(string tileActionString, Vector2 position)
        {
                if (!Game1.player.mailReceived.Contains("OpenedFlat2A"))
                {
                    Game1.playSound("woodWhack");
                    Game1.drawObjectDialogue(Game1.parseText(Helper.Translation.Get("LD.FlatA.Door")));
                    Game1.player.mailReceived.Add("OpenedFlat2A");
                }
                else if (Game1.player.mailReceived.Contains("OpenedFlat2A"))
                {
                    Game1.playSound("doorClose");
                    Game1.warpFarmer("Custom_LDFlat2A", 8, 13, 0);
                }
        }
    }
}