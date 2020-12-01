using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinalCadenaHotelera.Models
{
    public class Habitacion
    {
        [Key]
        [DisplayName("Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int HabitacionId { get; set; }

        [Required]
        [DisplayName("Número de habitación")]
        public int num_habitacion { get; set; }

        public int TipoHabitacionId { get; set; }
        public TipoHabitacion TipoHabitacion { get; set; }

        public int HotelId { get; set; }
        public  Hotel Hotel { get; set; }

        public List<Reserva> Reservas { get; set; }
    }
}