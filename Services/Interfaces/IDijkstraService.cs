using dijkstra_api.Data;

namespace dijkstra_api.Services.Interfaces
{
    public interface IDijkstraService
    {
        (int Danger, List<Point> Path)? getShortestPathLandOnly(Point a, Point b);
        (int Danger, List<Point> Path)? getShortestPathLandAndWater(Point a, Point b);
    }
}
