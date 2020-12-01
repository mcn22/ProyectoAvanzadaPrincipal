using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinalCadenaHotelera.Models
{
    public class TipoHabitacion
    {
        [Key]
        [Column(TypeName = "numeric")]
        [DisplayName("Id")]
        public int TipoHabitacionId { get; set; }

        [Required]
        [StringLength(128)]
        [DisplayName("Tipo de habitación")]
        public string nombre_tipo { get; set; }

        [Required]
        [StringLength(250)]
        [DisplayName("Descripción")]
        public string descripcion_tipo { get; set; }

        [Required]
        [Column(TypeName = "numeric")]
        [DisplayName("Costo por noche")]
        public int costo_noche_habitacion { get; set; }

        [Required]
        [StringLength(128)]
        public string imagen_tipo { get; set; }

        public List<Habitacion> Habitaciones { get; set; }
    }
}