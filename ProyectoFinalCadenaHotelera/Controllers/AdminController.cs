using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using ProyectoFinalCadenaHotelera.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ProyectoFinalCadenaHotelera.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class AdminController : Controller
    {

        public ActionResult Index()
        {
            List<Hotel> listaHoteles = new List<Hotel>();
            using (var ctx = new ApplicationDbContext())
            {
                var hotel = ctx.Hoteles.ToList();
                foreach (var item in hotel)
                {
                    listaHoteles.Add(
                        new Hotel
                        {
                            HotelId = item.HotelId,
                            nombre_hotel = item.nombre_hotel,
                            descripcion_hotel = item.descripcion_hotel,
                            imagen_hotel = item.imagen_hotel,
                            direccion_hotel = item.direccion_hotel,
                            ciudad_hotel = item.ciudad_hotel,
                            telefono_hotel = item.telefono_hotel
                        }
                    );
                }
            }
            return View(listaHoteles);
        }

        public ActionResult ListaReservasHotel(int? id)
        {
            ViewBag.idHotel = id;
            var userId = HttpContext.User.Identity.GetUserId().ToString();
            List<Reserva> listaFinalReservas = new List<Reserva>();
            var listaReserva = new List<Reserva>();
            var ctx = new ApplicationDbContext();
            var listaHabitaciones = ctx.Habitaciones.Where(k => k.HotelId == id).ToList();              
            foreach (var item in listaHabitaciones)
            {
                if (item != null) {
                    var listaReservas = ctx.Reservas.Where(j => j.HabitacionId == item.HabitacionId).ToList();
                    foreach (var reserva in listaReservas) {
                        if (reserva != null)
                        {
                            listaReserva.Add(reserva);
                        }
                    }                  
                }
                               
            }
            foreach (var itemReserva in listaReserva) {
                  if (itemReserva != null) {
                        listaFinalReservas.Add(itemReserva);
                  }                    
            }            
            return View(listaFinalReservas);
        }

        public ActionResult DetalleReserva(int? id, int? idHotel)
        {
            ViewBag.hotelID = idHotel;
            var ctx = new ApplicationDbContext();
            var reserva = ctx.Reservas.Where(k => k.ReservaId == id).FirstOrDefault();
            if (reserva!= null) {
                return View(reserva);
            }
            else{
                return RedirectToAction("Index");
            }
               
        }

        public ActionResult BusquedaReservaId()
        {
            return View();
        }

        [HttpPost, ActionName("BusquedaReservaId")]
        [ValidateAntiForgeryToken]
        public ActionResult BuscaReservaId()
        {
            int idReserva = int.Parse(Request["ReservaId"]);
            var ctx = new ApplicationDbContext();
            var reserva = ctx.Reservas.Where(k => k.ReservaId == idReserva).FirstOrDefault();
            if (reserva != null)
            {
                return RedirectToAction("DetalleReserva", new { id = reserva.ReservaId });
            }
            else {
                return View();
            }
        }

        public ActionResult FiltroEstado(int? id)
        {
            List<EstadoReserva> listaEstados = new List<EstadoReserva>();
            listaEstados = listarTiposEstado();
            ViewBag.ListaEstados = listaEstados.ToList();
            ViewBag.idHotel = id;
            return View();
        }

        [HttpPost, ActionName("FiltroEstado")]
        [ValidateAntiForgeryToken]
        public ActionResult ListaPorEstado(int? id)
        {
            int EstadoReservaId = int.Parse(Request["EstadoReservaId"]);
            int idHotelRequest = int.Parse(Request["idHotel"]);
            if (EstadoReservaId > 0)
            {
                return RedirectToAction("ListaPorTipo", new { idTipo = EstadoReservaId, idHotel = idHotelRequest });
            }
            else
            {
                return View();
            }
        }

        public ActionResult ListaPorTipo(int? idTipo, int ? idHotel)
        {
            ViewBag.idHotel = idHotel;
            var ctx = new ApplicationDbContext();
            var listaReservas = ctx.Reservas.Where(k => k.EstadoReserva.EstadoReservaId == idTipo && k.Habitacion.HotelId == idHotel).ToList();
            return View(listaReservas);
        }

        public ActionResult AnulaReserva(int? id, int? idHotel)
        {
            ViewBag.idHotel = idHotel;
            var ctx = new ApplicationDbContext();
            var reserva = ctx.Reservas.Where(k => k.ReservaId == id).FirstOrDefault();
            if (reserva != null)
            {
                return View(reserva);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost, ActionName("AnulaReserva")]
        [ValidateAntiForgeryToken]
        public ActionResult AnularReserva()
        {
            int idReserva = int.Parse(Request["ReservaId"]);
            int idHotelRequest = int.Parse(Request["idHotel"]);
            var ctx = new ApplicationDbContext();
            var reserva = ctx.Reservas.Where(k => k.ReservaId == idReserva).FirstOrDefault();
            if (reserva != null)
            {
                reserva.fecha_llegada = "0000-00-00";
                reserva.fecha_salida = "0000-00-00";
                reserva.EstadoReservaId = 3;
                ctx.Entry(reserva).State = System.Data.Entity.EntityState.Modified;
                ctx.SaveChanges();
                return RedirectToAction("FiltroEstado", new { id = idHotelRequest });
            }
            else {
                return View();
            }
        }

        public ActionResult GestionEmpleados()
        {
            var ctx = new ApplicationDbContext();
            var hotel_empleados = ctx.HotelEmpleados.ToList();
            return View(hotel_empleados);   
        }

        public ActionResult CrearEmpleado()
        {
            List<Hotel> listaHoteles = new List<Hotel>();
            listaHoteles = listarHoteles();
            ViewBag.listaHoteles = listaHoteles.ToList();
            return View();           
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CrearEmpleado(RegisterViewModelEmpleado model)
        {
            if (ModelState.IsValid)
            {
                ApplicationDbContext db = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

                var user = new ApplicationUser { Nombre = model.Nombre, Apellido = model.Apellido, UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {

                    UserManager.AddToRole(user.Id, "Recepcionista");

                    var hotelEmpleado = new Hotel_Empleado
                    {
                        Hotel_EmpleadoID = 0,
                        Id = user.Id,
                        HotelId = model.idHotel
                    };
                    db.HotelEmpleados.Add(hotelEmpleado);
                    db.SaveChanges();
                    return RedirectToAction("GestionEmpleados", "Admin");
                }
            }
            return View(model);
        }

        public ActionResult EditarLugar(string id)
        {
            var ctx = new ApplicationDbContext();
            var usuario = ctx.HotelEmpleados.Where(k => k.Id == id).FirstOrDefault();
            List<Hotel> listaHoteles = new List<Hotel>();
            listaHoteles = listarHoteles();
            ViewBag.listaHoteles = listaHoteles.ToList();
            return View(usuario);
        }

        [HttpPost, ActionName("EditarLugar")]
        [ValidateAntiForgeryToken]
        public ActionResult EditarLugarPost()
        {
            var idUsuario = Request["Id"];
            int idHotelRequest = int.Parse(Request["HotelId"]);
            var ctx = new ApplicationDbContext();
            var hotel_empleado = ctx.HotelEmpleados.Where(k => k.Id == idUsuario).FirstOrDefault();
            if (hotel_empleado != null)
            {
                hotel_empleado.HotelId = idHotelRequest;
                ctx.Entry(hotel_empleado).State = System.Data.Entity.EntityState.Modified;
                ctx.SaveChanges();
                return RedirectToAction("GestionEmpleados", "Admin");
            }
            else
            {
                return View();
            }
        }

        public ActionResult EliminarEmpleado(string id)
        {
            var ctx = new ApplicationDbContext();
            var usuarioApplication = ctx.Users.Where(k => k.Id == id).FirstOrDefault();
            var usuarioHotel = ctx.HotelEmpleados.Where(k => k.Id == id).FirstOrDefault();
            if (usuarioApplication != null)
            {
                ctx.HotelEmpleados.Remove(usuarioHotel);
                ctx.Users.Remove(usuarioApplication);
                ctx.SaveChanges();
                return RedirectToAction("GestionEmpleados", "Admin");
            } 
            else
            {
                return RedirectToAction("GestionEmpleados", "Admin");
            }
        }


        private List<EstadoReserva> listarTiposEstado()
        {
            List<EstadoReserva> listaEstados = new List<EstadoReserva>();
            using (var ctx = new ApplicationDbContext())
            {
                var estado = ctx.EstadoReservas.ToList();
                foreach (var item in estado)
                {
                    listaEstados.Add(
                        new EstadoReserva
                        {
                            EstadoReservaId = item.EstadoReservaId,
                            nombre_estado = item.nombre_estado
                        }
                    );
                }
            }
            return listaEstados;
        }//fin de listar tipos

        private List<Hotel> listarHoteles()
        {
            List<Hotel> listaHoteles = new List<Hotel>();
            using (var ctx = new ApplicationDbContext())
            {
                var hotel = ctx.Hoteles.ToList();
                foreach (var item in hotel)
                {
                    listaHoteles.Add(item);
                }
            }
            return listaHoteles;
        }//fin de listar tipos

    }//fin de la clase
}//fin del namespace