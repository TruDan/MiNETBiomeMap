using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using MiNET;
using MiNET.Utils;
using MiNET.Worlds;
using MiNETDevToolsPlugin.Interfaces;
using MiNETDevToolsPlugin.Models;

namespace MiNETDevToolsPlugin.Services
{
    public class LevelService : ILevelService
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(LevelService));
        
        private static MiNetServer _server;

        private Dictionary<string, LevelDataCache> _levelCache = new Dictionary<string, LevelDataCache>();

        internal LevelService(MiNetServer server)
        {
            _server = server;
        }

        public ChunkData[] FetchAllChunksForLevel(string levelId)
        {
            LevelDataCache cache;
            if (!_levelCache.TryGetValue(levelId, out cache))
            {
                //Log.InfoFormat("Called FetchAllChunksForLevel");
                var level = _server.LevelManager.Levels.FirstOrDefault(l => l != null && l.LevelId.Equals(levelId,
                                                                                StringComparison
                                                                                    .InvariantCultureIgnoreCase));
                if (level == null)
                {
                    return new ChunkData[0];
                }
                cache = new LevelDataCache(level);

                _levelCache.Add(level.LevelId, cache);
            }

            cache.Update();

            return cache.Chunks.Values.ToArray();
        }


        public ChunkData[] FetchUpdatedChunksForLevel(string levelId)
        {

            LevelDataCache cache;
            if (!_levelCache.TryGetValue(levelId, out cache))
            {
                //Log.InfoFormat("Called FetchAllChunksForLevel");
                var level = _server.LevelManager.Levels.FirstOrDefault(l => l != null && l.LevelId.Equals(levelId,
                                                                                StringComparison
                                                                                    .InvariantCultureIgnoreCase));
                if (level == null)
                {
                    return new ChunkData[0];
                }
                cache = new LevelDataCache(level);

                _levelCache.Add(level.LevelId, cache);
                return cache.Chunks.Values.ToArray();
            }

            return cache.Update();
            
            //Log.InfoFormat("Called FetchAllChunksForLevel | {0}", string.Join(", ", r.Select(c => c.ToString())));
        }
        
        public LevelData[] FetchAllLevels()
        {
            //Log.InfoFormat("Called FetchAllLevels");
            var r = _server.LevelManager.Levels.Where(e => e != null).Select(LevelData.FromLevel).ToArray();
            //Log.InfoFormat("Called FetchAllLevels | {0}", string.Join(", ", r.Select(c => c.ToString())));
            return r;
        }

    }

    class LevelDataCache
    {
        public string Id { get; set; }
        public Dictionary<ChunkCoordinates, ChunkData> Chunks { get; set; }

        private Level _level;
        private DateTime _lastUpdate = DateTime.MinValue;

        public LevelDataCache(Level level)
        {
            _level = level;
            Id = level.LevelId;
            Chunks = new Dictionary<ChunkCoordinates, ChunkData>();
        }

        public ChunkData[] Update()
        {
            var now = DateTime.UtcNow;
            if ((now - _lastUpdate).TotalMilliseconds > 1000)
            {
                _lastUpdate = now;
                return UpdateInternal();
            }

            return new ChunkData[0];
        }

        private ChunkData[] UpdateInternal()
        {
            var chunks = _level.GetLoadedChunks();

            var newData = new List<ChunkData>();
            foreach (var chunk in chunks)
            {
                var c = new ChunkCoordinates(chunk.x, chunk.z);
                ChunkData data;
                if (!Chunks.TryGetValue(c, out data))
                {
                    data = ChunkData.FromChunk(chunk);
                    Chunks.Add(c, data);
                    newData.Add(data);
                }

            }
            return newData.ToArray();
        }
    }
}
