using DroneDeliveryAPI.Enums;
using DroneDeliveryAPI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneDeliveryAPI.Services
{
    public class DroneService
    {
        public bool DronePodeViajar(Drone drone)
        {
            return drone.Disponivel && DateTime.Now >= drone.HorarioDisponivel;
        }

        public void IniciarRecarga(Drone drone)
        {
            drone.Status = DroneStatus.EmVoo;
            drone.HorarioDisponivel = DateTime.Now.AddMinutes(10);
        }

        public void ConcluirRecarga(Drone drone)
        {
            if (DateTime.Now >= drone.HorarioDisponivel)
            {
                drone.Status = DroneStatus.Idle;
                drone.RecarregarBateria();
            }
        }
    }
}
