using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinalCadenaHotelera.Models
{
    public class Reserva
    {

        [Key]
        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReservaId { get; set; }

        [Required]
        [DisplayName("Día de llegada")]
        public string fecha_llegada { get; set; }

        [Required]
        [DisplayName("Día de salida")]
        public string fecha_salida { get; set; }

        [Required]
        [Column(TypeName = "numeric")]
        [DisplayName("Total a pagar")]
        public int costo_total { get; set; }

        [Column(TypeName = "numeric")]
        [DisplayName("Saldo actual")]
        public int saldo_actual { get; set; }

        public int tipoHabitacionId { get; set; }

        //foreign
        public int EstadoReservaId { get; set; }
        public EstadoReserva EstadoReserva { get; set; }

        public string Id { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public int HabitacionId { get; set; }
        public Habitacion Habitacion { get; set; }
    }
}