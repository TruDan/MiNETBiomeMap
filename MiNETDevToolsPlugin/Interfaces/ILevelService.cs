using MiNETDevToolsPlugin.Models;

namespace MiNETDevToolsPlugin.Interfaces
{
    public interface ILevelService
    {
        LevelData[] FetchAllLevels();
        ChunkData[] FetchAllChunksForLevel(string levelId);
        ChunkData[] FetchUpdatedChunksForLevel(string levelId);
    }
}
