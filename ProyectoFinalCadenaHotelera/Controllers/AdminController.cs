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
        // GET: Admin
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
            //List<Habitacion> listaHabitaciones = new List<Habitacion>();
            //Reserva reserva = new Reserva();
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
            return View(reserva);                 
        }

    }
}