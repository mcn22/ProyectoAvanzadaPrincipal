using Microsoft.AspNet.Identity;
using ProyectoFinalCadenaHotelera.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoFinalCadenaHotelera.Controllers
{
    public class AdminController : Controller
    {
        [Authorize(Roles = "Administrador")]
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

        [Authorize(Roles = "Administrador")]
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

        [Authorize(Roles = "Administrador")]
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

        [Authorize(Roles = "Administrador")]
        public ActionResult BusquedaReservaId()
        {
            return View();
        }

        [Authorize(Roles = "Administrador")]
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

        [Authorize(Roles = "Administrador")]
        public ActionResult FiltroEstado(int? id)
        {
            List<EstadoReserva> listaEstados = new List<EstadoReserva>();
            listaEstados = listarTiposEstado();
            ViewBag.ListaEstados = listaEstados.ToList();
            ViewBag.idHotel = id;
            return View();
        }

        [Authorize(Roles = "Administrador")]
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

        [Authorize(Roles = "Administrador")]
        public ActionResult ListaPorTipo(int? idTipo, int ? idHotel)
        {
            ViewBag.idHotel = idHotel;
            var ctx = new ApplicationDbContext();
            var listaReservas = ctx.Reservas.Where(k => k.EstadoReserva.EstadoReservaId == idTipo && k.Habitacion.HotelId == idHotel).ToList();
            return View(listaReservas);
        }

        [Authorize(Roles = "Administrador")]
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

        [Authorize(Roles = "Administrador")]
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

        //-------------------------------------------------------

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

    }//fin de la clase
}//fin del namespace