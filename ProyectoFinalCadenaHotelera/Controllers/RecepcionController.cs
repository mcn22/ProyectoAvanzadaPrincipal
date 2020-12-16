using Microsoft.AspNet.Identity;
using ProyectoFinalCadenaHotelera.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoFinalCadenaHotelera.Controllers
{
    [Authorize(Roles = "Recepcionista")]
    public class RecepcionController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost, ActionName("Index")]
        [ValidateAntiForgeryToken]
        public ActionResult BuscaReservaId()
        {
            int idReserva = int.Parse(Request["ReservaId"]);
            var idUsuario = HttpContext.User.Identity.GetUserId();
            var ctx = new ApplicationDbContext();           
            var hotelEmpleado = ctx.HotelEmpleados.Where(z => z.Id == idUsuario).FirstOrDefault();
            var reserva = ctx.Reservas.Where(k => k.ReservaId == idReserva && k.Habitacion.HotelId == hotelEmpleado.HotelId).FirstOrDefault();
            if (reserva != null)
            {
                return RedirectToAction("DetalleReserva", new { id = reserva.ReservaId });
            }
            else
            {
                TempData["Mensaje"] = "La reserva buscada no existe o se encuentra en otro hotel.";
                return RedirectToAction("Index");
            }
        }

        public ActionResult DetalleReserva(int? id)
        {
            var ctx = new ApplicationDbContext();
            var reserva = ctx.Reservas.Where(k => k.ReservaId == id).FirstOrDefault();
            return View(reserva);
        }


        public ActionResult ConfirmarPago(int? id)
        {
            var ctx = new ApplicationDbContext();
            var reserva = ctx.Reservas.Where(k => k.ReservaId == id).FirstOrDefault();
            return View(reserva);
        }


        [HttpPost, ActionName("ConfirmarPago")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmarPagoPost()
        {
            int idReserva = int.Parse(Request["ReservaId"]);
            var ctx = new ApplicationDbContext();
            var reserva = ctx.Reservas.Where(k => k.ReservaId == idReserva).FirstOrDefault();
            if (reserva != null) {
                reserva.saldo_actual = 0;
                reserva.EstadoReservaId = 2;
                ctx.Entry(reserva).State = System.Data.Entity.EntityState.Modified;
                ctx.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}