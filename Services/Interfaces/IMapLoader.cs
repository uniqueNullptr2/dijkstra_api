using dijkstra_api.Data;

namespace dijkstra_api.Services.Interfaces
{
    public interface IMapLoader
    {
        ETileType? getTileType(Point point);
        int? getDangerLevel(Point point);

        List<List<int>> getDangerMap();
        List<List<ETileType>> getTileMap();
    }
}
