using StardewModdingAPI;

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