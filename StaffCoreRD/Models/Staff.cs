using System.ComponentModel.DataAnnotations;

namespace StaffCoreRD.Models
{
    public class Staff
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }  // Nombre completo

        [Required]
        public string Cedula { get; set; }  // Formato: 001-0000000-0

        [Required]
        public string Cargo { get; set; }   // Ej: Analista de Sistemas

        [Required]
        public string Departamento { get; set; } // Tecnología / RRHH / Finanzas / Operaciones

        [Required]
        [Range(23223, double.MaxValue, ErrorMessage = "Mínimo RD$23,223")]
        public decimal Salario { get; set; }

        public DateTime FechaIngreso { get; set; }

        public bool Activo { get; set; } = true;
    }
}