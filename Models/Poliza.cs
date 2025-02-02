namespace ClientePolizasAPI.Models
{
    public class Poliza
    {
        public int Id { get; set; }
        public string Auto { get; set; }
        public int Costo { get; set; }
        public string FechaVigencia { get; set; }
        public int IdCliente { get; set; } // Id del cliente asociado
    }

}