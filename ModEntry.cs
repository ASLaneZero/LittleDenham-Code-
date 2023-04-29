using StardewModdingAPI;
using System;
using System.Collections.Generic;
using StardewValley;
using StardewModdingAPI.Events;
using Microsoft.Xna.Framework;
using StardewValley.Menus;
using System.Linq;
using StardewValley.Locations;

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
            Animals.Initialize(this);
        }
    }
}