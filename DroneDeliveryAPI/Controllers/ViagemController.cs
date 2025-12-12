using DroneDeliveryAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace DroneDeliveryAPI.Controllers
{
    public class ViagemController : Controller
    {
        [ApiController]
        [Route("api/[controller]")]
        public class ViagensController : ControllerBase
        {
            private readonly AlocacaoService _service;

            public ViagensController(AlocacaoService service)
            {
                _service = service;
            }

            [HttpGet]
            public IActionResult GetViagens()
            {
                var viagens = _service.ObterViagens(); // Retorna lista de viagens
                return Ok(viagens);
            }
        }
    }
}
