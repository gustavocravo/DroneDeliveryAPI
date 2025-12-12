using DroneDeliveryAPI.Enums;
using DroneDeliveryAPI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneDeliveryAPI.Domain
{
    public class Pedido
    {
        public int Id { get; set; }
        public double PesoKg { get; set; }
        public Prioridade Prioridade { get; set; }
        public Coordenada CoordenadaDestino { get; set; } = new Coordenada(0, 0);
        public double DistanciaKm { get; set; }
        public DateTime Prazo { get; set; }
        public bool Entregue { get; set; } = false;

        public Pedido(int id, double pesoKg, Prioridade prioridade, Coordenada destino, DateTime prazo)
        {
            Id = id;
            PesoKg = pesoKg;
            Prioridade = prioridade;
            CoordenadaDestino = destino;
            Prazo = prazo;
        }

        public override string ToString()
        {
            return $"Pedido {Id} | Peso: {PesoKg}kg | Prioridade: {Prioridade} | Destino: ({CoordenadaDestino.X}, {CoordenadaDestino.Y})";
        }
    }
}
