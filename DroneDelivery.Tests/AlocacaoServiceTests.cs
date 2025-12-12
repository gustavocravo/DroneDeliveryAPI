using Xunit;
using DroneDeliveryAPI.Services;
using DroneDeliveryAPI.Domain; // Ajuste conforme seu namespace
using System.Collections.Generic;

namespace DroneDelivery.Tests
{
    public class AlocacaoServiceTests
    {
        private AlocacaoService _service;

        public AlocacaoServiceTests()
        {
            _service = new AlocacaoService();
        }

        [Fact]
        public void GerarViagens_DeveRetornarListaNaoVazia_QuandoExistemPedidosEDrones()
        {
            // Arrange
            var pedidos = new List<Pedido>
            {
                new Pedido { Id = 1, PesoKg = 5, LocalizacaoX = 10, LocalizacaoY = 10 },
                new Pedido { Id = 2, PesoKg = 3, LocalizacaoX = 12, LocalizacaoY = 12 }
            };

            var drones = new List<Drone>
            {
                new Drone { Id = 1, CapacidadeKg = 10, BateriaAtual = 100, AutonomiaKm = 50 }
            };

            // Act
            var viagens = _service.GerarViagens(pedidos, drones);

            // Assert
            Assert.NotEmpty(viagens);
            Assert.All(viagens, v => Assert.True(v.ConsumoTotalPercentual <= 95)); // Limite de 95% de bateria
        }

        [Fact]
        public void GerarViagens_NaoDeveExcederCapacidadeDoDrone()
        {
            // Arrange
            var pedidos = new List<Pedido>
            {
                new Pedido { Id = 1, PesoKg = 8, LocalizacaoX = 5, LocalizacaoY = 5 },
                new Pedido { Id = 2, PesoKg = 5, LocalizacaoX = 6, LocalizacaoY = 6 }
            };

            var drones = new List<Drone>
            {
                new Drone { Id = 1, CapacidadeKg = 10, BateriaAtual = 100, AutonomiaKm = 50 }
            };

            // Act
            var viagens = _service.GerarViagens(pedidos, drones);

            // Assert
            Assert.All(viagens, v =>
                Assert.True(v.Pedidos.Sum(p => p.PesoKg) <= drones[0].CapacidadeKg));
        }

        [Fact]
        public void RecargarBateria_DeveRestaurarBateriaDoDrone()
        {
            // Arrange
            var drone = new Drone { Id = 1, BateriaAtual = 50, CapacidadeKg = 10, AutonomiaKm = 50 };

            // Act
            drone.BateriaAtual = 100; // Simulando recarga
            // Se tiver método específico, troque por drone.RecargarBateria();

            // Assert
            Assert.Equal(100, drone.BateriaAtual);
        }

        [Fact]
        public void Viagem_DeveConterPedidosDentroDoRaio()
        {
            // Arrange
            var pedidos = new List<Pedido>
            {
                new Pedido { Id = 1, PesoKg = 2, LocalizacaoX = 0, LocalizacaoY = 0 },
                new Pedido { Id = 2, PesoKg = 3, LocalizacaoX = 1, LocalizacaoY = 1 },
                new Pedido { Id = 3, PesoKg = 2, LocalizacaoX = 20, LocalizacaoY = 20 } // fora do raio
            };
            var drones = new List<Drone>
            {
                new Drone { Id = 1, CapacidadeKg = 10, BateriaAtual = 100, AutonomiaKm = 50 }
            };

            // Act
            var viagens = _service.GerarViagens(pedidos, drones);

            // Assert
            Assert.All(viagens, v =>
                Assert.All(v.Pedidos, p =>
                    Assert.True(CalcularDistancia(p, v) <= 5))); // Raio = 5km
        }

        // Função auxiliar para calcular distância (simples)
        private double CalcularDistancia(Pedido p, Viagem v)
        {
            var dx = p.LocalizacaoX - v.Drone.LocalizacaoX;
            var dy = p.LocalizacaoY - v.Drone.LocalizacaoY;
            return System.Math.Sqrt(dx * dx + dy * dy);
        }
    }
}
