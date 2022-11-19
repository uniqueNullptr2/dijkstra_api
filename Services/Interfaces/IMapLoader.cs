using dijkstra_api.Data;

namespace dijkstra_api.Services.Interfaces
{
    public interface IMapLoader
    {
        ETileType? getTileType(Point point);
        int? getDangerLevel(Point point);
    }
}
