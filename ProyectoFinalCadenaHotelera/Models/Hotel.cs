using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinalCadenaHotelera.Models
{
    public class Hotel
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Id")]
        public int HotelId { get; set; }

        [Required]
        [StringLength(150)]
        [DisplayName("Nombre del hotel")]
        public string nombre_hotel { get; set; }

        [Required]
        [DisplayName("Descripción")]
        public string descripcion_hotel { get; set; }

        [Required]
        public string imagen_hotel { get; set; }

        [Required]
        [DisplayName("Dirección")]
        public string direccion_hotel { get; set; }

        [Required]
        [DisplayName("Ciudad")]
        public string ciudad_hotel { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Teléfono")]
        public string telefono_hotel { get; set; }

        public List<Habitacion> Habitaciones { get; set; }
        public List<Hotel_Empleado> Hotel_Empleados { get; set; }
    }
}