using DroneDeliveryAPI.Enums;
using DroneDeliveryAPI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DroneDeliveryAPI.Services
{
    public class AlocacaoService
    {
        private const double ConsumoPorKm = 1.0;
        private const int MinutosParaCarregar100 = 10;
        private readonly Random _random = new Random();

        // Armazenamento interno
        private List<Pedido> _pedidos = new List<Pedido>();
        private List<Drone> _drones = new List<Drone>();
        private List<Viagem> _viagens = new List<Viagem>();

        // Construtor: você pode inicializar alguns dados de teste

        // ============================
        // MÉTODOS PÚBLICOS PARA CONTROLLERS
        // ============================

        public List<Pedido> ObterPedidos()
        {
            return _pedidos;
        }

        public List<Drone> ObterDrones()
        {
            return _drones;
        }

        public List<Viagem> ObterViagens()
        {
            return _viagens;
        }

        public async Task<List<Viagem>> GerarViagens()
        {
            List<Viagem> viagens = new List<Viagem>();

            var pedidosPendentes = _pedidos.Where(p => !p.Entregue).OrderByDescending(p => p.Prioridade).ThenBy(p => p.PesoKg).ToList();

            foreach (var drone in _drones)
            {
                while (true)
                {
                    var viagem = await GerarViagemParaDrone(drone, pedidosPendentes);
                    if (viagem == null)
                        break;

                    viagens.Add(viagem);
                    _viagens.Add(viagem);

                    foreach (var p in viagem.Pedidos)
                        p.Entregue = true;
                }
            }

            return viagens;
        }

        // ============================
        // LÓGICA DE GERAÇÃO DE VIAGEM
        // ============================

        public async Task<Viagem?> GerarViagemParaDrone(Drone drone, List<Pedido> pedidos)
        {
            if (drone.Status == DroneStatus.Carregando && DateTime.Now < drone.HorarioDisponivel)
                return null;

            if (drone.BateriaAtual < 5)
                return null;

            double raio = 5;
            List<Pedido> selecionados = new List<Pedido>();

            while (raio <= drone.AutonomiaKm / 2)
            {
                var candidatos = pedidos
                    .Where(p => !p.Entregue)
                    .OrderByDescending(p => p.Prioridade)
                    .ThenBy(p => p.PesoKg)
                    .ToList();

                foreach (var p in candidatos)
                {
                    if (selecionados.Contains(p))
                        continue;

                    double pesoAtual = selecionados.Sum(x => x.PesoKg);
                    if (pesoAtual + p.PesoKg <= drone.CapacidadeKg)
                        selecionados.Add(p);
                }

                if (selecionados.Count > 0)
                    break;

                raio += 5;
            }

            if (selecionados.Count == 0)
                return null;

            double distanciaTotal = selecionados.Max(p => p.PesoKg) * 2; // Exemplo simples
            double consumo = distanciaTotal * ConsumoPorKm;

            if (consumo > drone.BateriaAtual * 0.95)
                return null;

            Viagem viagem = new Viagem
            {
                Drone = drone,
                Pedidos = new List<Pedido>(selecionados),
                DistanciaTotalKm = distanciaTotal,
                ConsumoTotalPercentual = consumo,
                BateriaAntes = drone.BateriaAtual,
                HorarioPartida = DateTime.Now
            };

            await SimularEstadosDrone(drone, consumo);

            viagem.BateriaDepois = drone.BateriaAtual;
            viagem.HorarioRetorno = drone.HorarioDisponivel;

            return viagem;
        }

        private async Task SimularEstadosDrone(Drone drone, double consumo)
        {
            var random = new Random();

            drone.AlterarStatus(DroneStatus.EmVoo, random.Next(1, 4));
            await Task.Delay(random.Next(3000, 10000));

            drone.AlterarStatus(DroneStatus.Entregando, random.Next(1, 4));
            await Task.Delay(random.Next(3000, 10000));

            drone.AlterarStatus(DroneStatus.Retornando, random.Next(1, 4));
            await Task.Delay(random.Next(3000, 10000));

            drone.BateriaAtual -= consumo;
            drone.BateriaAtual = Math.Round(drone.BateriaAtual, 2);

            int tempoRecarga = (int)Math.Ceiling((100 - drone.BateriaAtual) / 100.0 * MinutosParaCarregar100);
            drone.AlterarStatus(DroneStatus.Carregando, tempoRecarga);
            drone.HorarioDisponivel = DateTime.Now.AddMinutes(tempoRecarga);

            await Task.Delay(TimeSpan.FromMinutes(tempoRecarga));
            drone.BateriaAtual = drone.AutonomiaKm;
            drone.Status = DroneStatus.Idle;
        }
    }
}
