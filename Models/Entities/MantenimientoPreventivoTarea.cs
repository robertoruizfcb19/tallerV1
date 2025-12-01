using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tallerV1.Models.Entities
{
    public class MantenimientoPreventivoTarea
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("MantenimientoPreventivo")]
        public int MantenimientoPreventivoId { get; set; }

        [Required]
        [StringLength(500)]
        public string Descripcion { get; set; } = string.Empty;

        public int Orden { get; set; }

        // Navegación
        public MantenimientoPreventivo MantenimientoPreventivo { get; set; } = null!;
    }
}
