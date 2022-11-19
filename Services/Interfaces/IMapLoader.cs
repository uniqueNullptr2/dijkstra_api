using dijkstra_api.Data;

namespace dijkstra_api.Services.Interfaces
{
    public interface IMapLoader
    {
        ETileType? getTileType(int x, int y);
        int? getDangerLevel(int x, int y);
    }
}
