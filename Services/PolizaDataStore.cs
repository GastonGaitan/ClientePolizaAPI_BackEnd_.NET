using ClientePolizasAPI.Models;

namespace ClientePolizasAPI.Services
{
    public class PolizaDataStore
    {
        public List<Poliza> Polizas { get; set; }

        public static PolizaDataStore Current { get; } = new PolizaDataStore();

        public PolizaDataStore()
        {
            Polizas = new List<Poliza>
            {
                new Poliza { Id = 1, Auto = "Toyota Corolla", Costo = 15000, FechaVigencia = "2024-12-31", IdCliente = 1 },
                new Poliza { Id = 2, Auto = "Honda Civic", Costo = 18000, FechaVigencia = "2025-06-30", IdCliente = 2 }
            };
        }
    }
}
