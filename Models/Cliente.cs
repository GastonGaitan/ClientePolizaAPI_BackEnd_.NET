namespace ClientePolizasAPI.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string Apellido { get; set; }
        public required string DNI { get; set; }
        public required string FechaDeNacimiento { get; set; }
    }
}
