using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinalCadenaHotelera.Models
{
    public class Valoraciones
    {
        [Key]
        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ValoracionesId { get; set; }

        [Required]
        public string comentario { get; set; }

        [Column(TypeName = "numeric")]
        public int puntuacion { get; set; }

        public string Id { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public int HotelId { get; set; }
        public Hotel Hotel { get; set; }
    }
}