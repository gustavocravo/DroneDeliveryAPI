using DroneDeliveryAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace DroneDeliveryAPI.Controllers
{
    public class DroneController : Controller
    {
        [ApiController]
        [Route("api/[controller]")]
        public class DronesController : ControllerBase
        {
            private readonly AlocacaoService _service;

            public DronesController(AlocacaoService service)
            {
                _service = service;
            }

            [HttpGet]
            public IActionResult GetDrones()
            {
                var drones = _service.ObterDrones(); // Retorna lista de drones
                return Ok(drones);
            }
        }
    }
}
