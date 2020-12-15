using System.ComponentModel;
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
        [DisplayName("Comentario")]
        public string comentario { get; set; }

        [Column(TypeName = "numeric")]
        [Required]
        [DisplayName("Puntuación")]
        public int puntuacion { get; set; }

        public string Id { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public int HotelId { get; set; }
        public virtual Hotel Hotel { get; set; }
    }
}