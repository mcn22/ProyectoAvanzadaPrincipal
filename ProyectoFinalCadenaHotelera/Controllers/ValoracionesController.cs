using Microsoft.AspNet.Identity;
using ProyectoFinalCadenaHotelera.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoFinalCadenaHotelera.Controllers
{
    public class ValoracionesController : Controller
    {
        public ActionResult Index()
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
            return View(listaHoteles);
        }

        public ActionResult Valoraciones(int ? id)
        {
            var ctx = new ApplicationDbContext();
            var hotel = ctx.Hoteles.Where(h => h.HotelId == id).FirstOrDefault();
            ViewBag.Hotel = hotel;
            var valoraciones = ctx.Valoraciones.Where(p => p.HotelId == hotel.HotelId).ToList();
            return View(valoraciones);
        }

        [HttpPost, ActionName("Valoraciones")]
        [ValidateAntiForgeryToken]
        public ActionResult DejaComentario()
        {
            int idHotel = int.Parse(Request["idHotel"]);
            return RedirectToAction("Comentar", new { id = idHotel });
        }


        public ActionResult Comentar(int? id)
        {
            ViewBag.idHotel = id;
            return View();
        }

        [HttpPost, ActionName("Comentar")]
        [ValidateAntiForgeryToken]
        public ActionResult ComentarPost()
        {
            int idHotel = int.Parse(Request["idHotel"]);
            var cometario = Request["comentario"];
            var puntuacion = int.Parse(Request["puntuacion"]);
            var idUser = HttpContext.User.Identity.GetUserId();

            using (var ctx = new ApplicationDbContext())
            {
                var valoracion = new Valoraciones()
                {
                    ValoracionesId = 0,
                    comentario = cometario,
                    puntuacion = puntuacion,
                    Id = idUser,
                    HotelId = idHotel
                };
                ctx.Valoraciones.Add(valoracion);
                ctx.SaveChanges();
                return RedirectToAction("Valoraciones", new { id = idHotel });
            }           
        }
    }
}