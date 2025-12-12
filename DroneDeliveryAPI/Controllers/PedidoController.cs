using DroneDeliveryAPI.Domain;
using DroneDeliveryAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace DroneDeliveryAPI.Controllers
{
    public class PedidoController : Controller
    {
        [ApiController]
        [Route("api/[controller]")]
        public class PedidosController : ControllerBase
        {
            private readonly AlocacaoService _alocacao;
            private readonly List<Drone> _drones;
            private readonly List<Pedido> _pedidos;

            public PedidosController(AlocacaoService alocacao)
            {
                _alocacao = alocacao;

                _drones = new List<Drone>
            {
                new Drone("Drone A", 10, 100),
                new Drone("Drone B", 5, 80)
            };

                _pedidos = new List<Pedido>();
            }

            [HttpPost]
            public IActionResult CriarPedido([FromBody] Pedido novoPedido)
            {
                if (novoPedido == null)
                    return BadRequest("Pedido inválido.");

                _pedidos.Add(novoPedido);
                return CreatedAtAction(nameof(CriarPedido), novoPedido);
            }

            [HttpGet("/api/entregas/rota")]
            public async Task<IActionResult> ObterRota()
            {
                var viagens = await Task.Run(() => _alocacao.GerarViagens());
                return Ok(viagens);
            }

            [HttpGet("/api/drones/status")]
            public IActionResult StatusDrones()
            {
                var status = _drones.Select(d => new
                {
                    d.Nome,
                    d.Status,
                    BateriaAtual = d.BateriaAtual
                });

                return Ok(status);
            }
        }
    }
}
