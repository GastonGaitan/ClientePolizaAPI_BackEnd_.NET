namespace ClientePolizasAPI.Models
{
    public class Poliza
    {
        public int Id { get; set; }
        public required string Auto { get; set; }
        public required int Costo { get; set; }
        public required string FechaVigencia { get; set; }
        public required int IdCliente { get; set; } // Id del cliente asociado
    }

}