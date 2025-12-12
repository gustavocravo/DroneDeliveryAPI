using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneDeliveryAPI.Domain
{
    public class Coordenada
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Coordenada(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double DistanciaAteBase()
        {
            return Math.Sqrt(X * X + Y * Y);
        }
    }
}
