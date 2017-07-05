using System.Linq;
using MiNET.Worlds;

namespace MiNETDevTools.Graphics.Biomes
{
    public static class BiomeMapUtil
    {
        public static int MaxBiomeId = BiomeUtils.Biomes.Max(b => b.Id);
        public static float MinBiomeTemperature = BiomeUtils.Biomes.Min(b => b.Temperature);
        public static float MaxBiomeTemperature = BiomeUtils.Biomes.Max(b => b.Temperature);

        public static float MinBiomeDownfall = BiomeUtils.Biomes.Min(b => b.Downfall);
        public static float MaxBiomeDownfall = BiomeUtils.Biomes.Max(b => b.Downfall);


    }
}
