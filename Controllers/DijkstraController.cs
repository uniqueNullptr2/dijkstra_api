using dijkstra_api.Data;
using dijkstra_api.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace dijkstra_api.Controllers
{
    [Route("api/dijkstra")]
    [ApiController]
    public class DijkstraController : ControllerBase
    {
        private IMapLoader map;
        private IDijkstraService dijkstra;
        public DijkstraController(IMapLoader map, IDijkstraService dijkstra)
        {
            this.map = map;
            this.dijkstra = dijkstra;
        }

        [HttpGet("danger")]
        public int? getDangerLevel([FromQuery] int x, [FromQuery] int y)
        {
            return map.getDangerLevel(new Point(x,y));
        }

        [HttpGet("tile")]
        public string getTileType([FromQuery] int x, [FromQuery] int y)
        {
            return map.getTileType(new Point(x, y)).ToString();
        }

        [HttpGet("path-only-land")]
        public (int Danger, List<Point> path)? getPathOnlyLand([FromQuery] int x1, [FromQuery] int y1, [FromQuery] int x2, [FromQuery] int y2)
        {
            return dijkstra.getShortestPathLandOnly(new Point(x1,y1), new Point(x2,y2));
        }
    }
}
