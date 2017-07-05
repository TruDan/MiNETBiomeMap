using System;
using MiNET.Worlds;

namespace MiNETDevToolsPlugin.Models
{
    [Serializable]
    public class LevelData
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public int Players { get; set; }
        public long AvarageTickProcessingTime { get; set; }

        internal static LevelData FromLevel(Level level)
        {
            return new LevelData()
            {
                Id = level.LevelId,
                Name = level.LevelName,
                Players = level.PlayerCount,
                AvarageTickProcessingTime = level.AvarageTickProcessingTime
            };
        } 
    }
}
