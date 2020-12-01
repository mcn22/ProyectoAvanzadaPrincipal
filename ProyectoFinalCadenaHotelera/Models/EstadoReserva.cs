using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinalCadenaHotelera.Models
{
    public class EstadoReserva
    {
        [Key]
        [Column(TypeName = "numeric")]
        [DisplayName("Id")]
        public int EstadoReservaId { get; set; }

        [Required]
        [StringLength(150)]
        [DisplayName("Nombre del estado")]
        public string nombre_estado { get; set; }

        public List<Reserva> Reservas { get; set; }
    }
}