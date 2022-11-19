using dijkstra_api.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace dijkstra_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DijkstraController : ControllerBase
    {
        private IMapLoader map;
        public DijkstraController(IMapLoader map)
        {
            this.map = map;
        }

        [HttpGet("danger")]
        public int? getDangerLevel([FromQuery] int x, [FromQuery] int y)
        {
            return map.getDangerLevel(x, y);
        }

        [HttpGet("tile")]
        public string getTileType([FromQuery] int x, [FromQuery] int y)
        {
            return map.getTileType(x, y).ToString();
        }
    }
}
