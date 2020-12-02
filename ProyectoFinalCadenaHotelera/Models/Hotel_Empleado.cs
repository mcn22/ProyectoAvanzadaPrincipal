using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinalCadenaHotelera.Models
{
    public class Hotel_Empleado
    {
        [Key]
        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Hotel_EmpleadoID { get; set; }

        public string Id { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public int HotelId { get; set; }
        public virtual Hotel Hotel { get; set; }

    }
}