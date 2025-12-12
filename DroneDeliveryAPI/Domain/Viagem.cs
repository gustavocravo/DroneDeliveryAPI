using DroneDeliveryAPI.Enums;
using DroneDeliveryAPI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneDeliveryAPI.Domain
{
    public class Viagem
    {
        public Drone? Drone { get; set; }
        public List<Pedido> Pedidos { get; set; } = new List<Pedido>();
        public double DistanciaTotalKm { get; set; }
        public double ConsumoTotalPercentual { get; set; }
        public double BateriaAntes { get; set; }
        public double BateriaDepois { get; set; }

        public DateTime HorarioPartida { get; set; }
        public DateTime HorarioRetorno { get; set; }

        public override string ToString()
        {
            var lista = string.Join(", ", Pedidos.Select(p => p.Id));
            string droneNome = Drone?.Nome ?? "Indefinido";
            return $"Drone: {droneNome} | Pedidos: [{lista}] | Distância: {DistanciaTotalKm} km | Consumo: {ConsumoTotalPercentual}%";
        }
    }
}
