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
        public ActionResult<object> getPathOnlyLand([FromQuery] int x1, [FromQuery] int y1, [FromQuery] int x2, [FromQuery] int y2)
        {
            var a = new Point(x1, y1);
            var b = new Point(x2, y2);
            if (map.getTileType(a) == ETileType.WATER)
            {
                return BadRequest("Starting point is a watertile.");
            } else if (map.getTileType(b) == ETileType.WATER)
            {
                return BadRequest("Ending point is a watertile.");
            }
            var r = dijkstra.getShortestPathLandOnly(a, b).GetValueOrDefault((-1, new List<Point>()));
            return Ok(new {Danger=r.Danger, Path=r.Path});
        }

        [HttpGet("path-land-and-water")]
        public ActionResult<object> getPathLandAndWater([FromQuery] int x1, [FromQuery] int y1, [FromQuery] int x2, [FromQuery] int y2)
        {
            var a = new Point(x1, y1);
            var b = new Point(x2, y2);
            var r = dijkstra.getShortestPathLandAndWater(a, b).GetValueOrDefault((-1, new List<Point>()));
            return Ok(new { Danger = r.Danger, Path = r.Path });
        }

        [HttpGet("danger-map")]
        public List<List<int>> getDangerMap()
        {
            return map.getDangerMap();
        }

        [HttpGet("tile-map")]
        public List<List<ETileType>> getTileMap()
        {
            return map.getTileMap();
        }
    }
}
