using DroneDeliveryAPI.Domain;
using DroneDeliveryAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneDeliveryAPI.Services
{
    public class SimuladorService
    {
        private readonly AlocacaoService _alocacao = new AlocacaoService();
        private readonly DroneService _droneService = new DroneService();

        public async Task<List<Viagem>> ExecutarSimulacao(List<Drone> drones, List<Pedido> pedidos)
        {
            List<Viagem> viagens = new List<Viagem>();

            while (pedidos.Any(p => true))
            {
                foreach (var drone in drones)
                {
                    _droneService.ConcluirRecarga(drone);

                    if (!_droneService.DronePodeViajar(drone))
                        continue;

                    var viagem = await _alocacao.GerarViagemParaDrone(drone, pedidos);
                    
                    if (viagem == null)
                        continue;

                    viagens.Add(viagem);

                    foreach (var p in viagem.Pedidos)
                        pedidos.Remove(p);

                    if (drone.BateriaAtual <= drone.AutonomiaKm * 0.05)
                        _droneService.IniciarRecarga(drone);
                }

                if (drones.All(d => !_droneService.DronePodeViajar(d)))
                    break;
            }

            return viagens;

        }
    }
}
