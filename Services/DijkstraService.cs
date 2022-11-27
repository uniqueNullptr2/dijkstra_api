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


        private (int Danger, List<Point> Path)? dijkstra(Func<int, Point, int> calculateDanger, Func<Point, ISet<Point>, List<Point>> selectPoints, Point a, Point b)
        {
            var queue = new PriorityQueue<(int Danger, List<Point> Path, Point Point), int>();
            var list = new List<Point>();
            var danger = calculateDanger(0, a);
            queue.Enqueue((danger, list, a), danger);
            var set = new HashSet<Point>();

            while (queue.Count > 0)
            {
                var tmp = queue.Dequeue();
                var Path = new List<Point>(tmp.Path) { tmp.Point };
                if (map.getDangerLevel(tmp.Point)==null)
                {
                    continue;
                }
                if (tmp.Point.Equals(b))
                {
                    return (tmp.Danger, Path);
                }
                set.Add(tmp.Point);
                var points = selectPoints(tmp.Point, set);
                foreach (var point in points)
                {
                    var tmp_danger = calculateDanger(tmp.Danger, point);
                    
                    queue.Enqueue((tmp_danger, Path, point), aStar(tmp_danger,point,b));
                }
            }
            return null;
        }

        private int aStar(int danger, Point a, Point b)
        {
            return danger * (Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y));
        }
        private int CalculateDangerLevelLandOnly(int previousDanger, Point next)
        {
            return previousDanger + map.getDangerLevel(next).GetValueOrDefault(-1);
        }

        private int CalculateDangerLevelLandAndWater(int previousDanger, Point next)
        {
            return previousDanger + map.getDangerLevel(next).GetValueOrDefault(-1) / 2 + 1;
        }

        private List<Point> pointSelectLandOnly(Point point, ISet<Point> usedPoints)
        {
            var points = new List<Point>();
            var x = point.X;
            var y = point.Y;
            points.Add(new Point(x+1, y));
            points.Add(new Point(x -1, y));
            points.Add(new Point(x, y+1));
            points.Add(new Point(x, y-1));
            return points.Where(e => map.getTileType(e) == ETileType.LAND && !usedPoints.Contains(e)).ToList();
        }

        private List<Point> pointSelectLandAndWater(Point point, ISet<Point> usedPoints)
        {
            var points = new List<Point>();
            var x = point.X;
            var y = point.Y;
            
            points.Add(new Point(x, y - 1));
            points.Add(new Point(x - 1, y));
            points.Add(new Point(x + 1, y));
            points.Add(new Point(x, y + 1));
            
            if (map.getTileType(point) == ETileType.WATER)
            {
                points.Add(new Point(x - 1, y - 1));
                points.Add(new Point(x + 1, y - 1));
                points.Add(new Point(x - 1, y + 1));
                points.Add(new Point(x + 1, y + 1));
            }

            return points.Where(e => map.getDangerLevel(e) != null && !usedPoints.Contains(e)).ToList();
        }
    }
}
