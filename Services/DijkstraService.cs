using dijkstra_api.Data;
using dijkstra_api.Services.Interfaces;

namespace dijkstra_api.Services
{
    public class DijkstraService : IDijkstraService
    {
        private IMapLoader map;
        public DijkstraService(IMapLoader map)
        {
            this.map = map;
        }
        public (int Danger, List<Point> Path)? getShortestPathLandAndWater(Point a, Point b)
        {
            return dijkstra(CalculateDangerLevelLandAndWater, pointSelectLandAndWater, a, b);
        }

        public (int Danger, List<Point> Path)? getShortestPathLandOnly(Point a, Point b)
        {
            return dijkstra(CalculateDangerLevelLandOnly, pointSelectLandOnly, a, b);
        }


        private (int Danger, List<Point> Path)? dijkstra(Func<List<Point>, int> calculateDanger, Func<Point, ISet<Point>, List<Point>> selectPoints, Point a, Point b)
        {
            var queue = new PriorityQueue<(int Danger, List<Point> Path),int>();
            var list = new List<Point>() { a};
            var danger = calculateDanger(list);
            queue.Enqueue((danger, list), danger);
            var set = new HashSet<Point>();
            set.Add(a);

            while (queue.Count > 0)
            {
                var tmp = queue.Dequeue();
                var points = selectPoints(tmp.Path.Last(), set);
                foreach (var point in points)
                {
                    set.Add(point);
                    var tmp_list = new List<Point>(tmp.Item1);
                    var tmp_danger = calculateDanger(tmp_list);
                    if (point == b)
                    {
                        return (tmp_danger, tmp_list);
                    }
                    queue.Enqueue((tmp_danger, tmp_list), tmp_danger);
                }
            }
            return null;
        }

        private int CalculateDangerLevelLandOnly(List<Point> points)
        {
            return points.Select(e => map.getDangerLevel(e)).Sum().GetValueOrDefault(-1);
        }

        private int CalculateDangerLevelLandAndWater(List<Point> points)
        {
            return -1;
        }

        private List<Point> pointSelectLandOnly(Point point, ISet<Point> usedPoints)
        {
            var points = new List<Point>();
            var x = point.x;
            var y = point.y;
            points.Add(new Point(x+1, y));
            points.Add(new Point(x -1, y));
            points.Add(new Point(x, y+1));
            points.Add(new Point(x, y-1));
            return points.Where(e => map.getTileType(e) == ETileType.LAND && !usedPoints.Contains(e)).ToList();
        }

        private List<Point> pointSelectLandAndWater(Point point, ISet<Point> usedPoints)
        {
            return new List<Point>();
        }
    }
}
