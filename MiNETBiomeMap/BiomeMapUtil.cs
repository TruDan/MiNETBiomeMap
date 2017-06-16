using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiNET.Worlds;

namespace MiNETBiomeMap
{
    public static class BiomeMapUtil
    {

        public static float MinBiomeTemperature = BiomeUtils.Biomes.Min(b => b.Temperature);
        public static float MaxBiomeTemperature = BiomeUtils.Biomes.Max(b => b.Temperature);

        public static float MinBiomeDownfall = BiomeUtils.Biomes.Min(b => b.Downfall);
        public static float MaxBiomeDownfall = BiomeUtils.Biomes.Max(b => b.Downfall);


    }
}
