namespace ClientePolizasAPI.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string DNI { get; set; }
        public string FechaDeNacimiento { get; set; }
        public List<int>? IdPolizas { get; set; } 
    }
}