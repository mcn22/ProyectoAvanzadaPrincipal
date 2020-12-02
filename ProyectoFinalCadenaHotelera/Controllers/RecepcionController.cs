using Microsoft.AspNet.Identity;
using ProyectoFinalCadenaHotelera.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoFinalCadenaHotelera.Controllers
{
    public class RecepcionController : Controller
    {
        public ActionResult Index()
        {
            var ctx = new ApplicationDbContext();
            var userId = HttpContext.User.Identity.GetUserId().ToString();
            var hotelId = ctx.HotelEmpleados.Where(p => p.Id == userId).FirstOrDefault();
            var listaReservas = ctx.Reservas.Where(k => k.Habitacion.HotelId == hotelId.HotelId).ToList();
            return View(listaReservas);
        }

        public ActionResult DetalleReserva(int? id)
        {
            var ctx = new ApplicationDbContext();
            var reserva = ctx.Reservas.Where(k => k.ReservaId == id).FirstOrDefault();
            return View(reserva);
        }
    }
}