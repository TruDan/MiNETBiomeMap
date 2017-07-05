using System;

namespace MiNETDevToolsPlugin.Models
{
    [Serializable]
    public class ChunkColumnInfo
    {
        public int X { get; set; }
        public int Z { get; set; }

        public byte Height { get; set; }
        public byte BiomeId { get; set; }
    }
}