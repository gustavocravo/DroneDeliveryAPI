using System;
using DroneDeliveryAPI.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace DroneDeliveryAPI.Domain
{
    public class Drone
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public double CapacidadeKg { get; set; }
        public double AutonomiaKm { get; set; }
        public double BateriaAtual { get; set; }
        public DroneStatus Status { get; set; } = DroneStatus.Idle;
        public bool Disponivel => Status == DroneStatus.Idle;
        public DateTime HorarioDisponivel { get; set; } = DateTime.Now;
        public double ConsumoPorKm { get; set; } = 1.0;

        public Drone(string nome, double capacidadeKg, double autonomiaKm)
        {
            Nome = nome;
            CapacidadeKg = capacidadeKg;
            AutonomiaKm = autonomiaKm;
            BateriaAtual = autonomiaKm;
        }

        public void AlterarStatus(DroneStatus novoStatus, int minutos = 0)
        {
            Status = novoStatus;
            if (minutos > 0)
                HorarioDisponivel = DateTime.Now.AddMinutes(minutos);
        }

        public void RecarregarBateria()
        {
            Status = DroneStatus.Carregando;
            BateriaAtual = AutonomiaKm;
        }

        public double CalcularRaioDinamico()
        {
            return BateriaAtual / ConsumoPorKm;
        }

        public override string ToString()
        {
            return $"{Nome} | Capacidade: {CapacidadeKg}kg | Bateria: {BateriaAtual}/{AutonomiaKm}";
        }
    }
}
