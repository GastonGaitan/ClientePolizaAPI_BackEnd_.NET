using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClientePolizasAPI.Models
{
    public class Cliente
    {
        [Key] // Define la clave primaria
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-incrementable
        public int Id { get; set; }

        [Required] // Campo obligatorio
        [MaxLength(100)] // Limita la longitud a 100 caracteres
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Apellido { get; set; } = string.Empty;

        [Required]
        [MaxLength(10)] // Suponiendo que el DNI no tiene más de 10 caracteres
        public string DNI { get; set; } = string.Empty;

        [Required]
        public string FechaDeNacimiento { get; set; } = string.Empty;
    }
}
