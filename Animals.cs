using StardewModdingAPI;
using StardewValley;
using StardewModdingAPI.Events;
using Microsoft.Xna.Framework;
using StardewValley.Locations;

namespace LittleDenham
{
    internal static class Animals
    {
        static IModHelper Helper;
        internal static void Initialize(IMod ModInstance)
        {
            Helper = ModInstance.Helper;
            Animals.Helper.Events.Player.Warped += OnWarpedToForest;
        }
        internal static void OnWarpedToForest(object? sender, WarpedEventArgs e)
        {
            if (e.NewLocation is not Forest forest)
                return;

            forest.marniesLivestock.Clear();

            Multiplayer multiplayer = Animals.Helper.Reflection.GetField<Multiplayer>(typeof(Game1), "multiplayer").GetValue();

            FarmAnimal CowWhiteLD = new("White Cow", multiplayer.getNewID(), -1L)
            {
                Position = new Vector2(96 * Game1.tileSize, 18 * Game1.tileSize)
            };

            FarmAnimal CowBrownLD1 = new("Brown Cow", multiplayer.getNewID(), -1L)
            {
                Position = new Vector2(102 * Game1.tileSize, 19 * Game1.tileSize)
            };

            FarmAnimal CowBrownLD2 = new("Brown Cow", multiplayer.getNewID(), -1L)
            {
                Position = new Vector2(106 * Game1.tileSize, 9 * Game1.tileSize)
            };

            FarmAnimal GoatLD = new("Goat", multiplayer.getNewID(), -1L)
            {
                Position = new Vector2(106 * Game1.tileSize, 16 * Game1.tileSize)
            };

            FarmAnimal PigLD = new("Pig", multiplayer.getNewID(), -1L)
            {
                Position = new Vector2(108 * Game1.tileSize, 18 * Game1.tileSize)
            };

            (Game1.getLocationFromName("Forest") as Forest).marniesLivestock.Add(CowWhiteLD);
            (Game1.getLocationFromName("Forest") as Forest).marniesLivestock.Add(CowBrownLD1);
            (Game1.getLocationFromName("Forest") as Forest).marniesLivestock.Add(CowBrownLD2);
            (Game1.getLocationFromName("Forest") as Forest).marniesLivestock.Add(GoatLD);
            (Game1.getLocationFromName("Forest") as Forest).marniesLivestock.Add(PigLD);
        }
    }
}
