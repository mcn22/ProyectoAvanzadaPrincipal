using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ProyectoFinalCadenaHotelera.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoFinalCadenaHotelera.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //if (User.Identity.IsAuthenticated) {
            //    using (ApplicationDbContext db = new ApplicationDbContext())
            //    {
            //        var idUsuarioActual = User.Identity.GetUserId();
            //        var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            //        var resultado1 = roleManager.Create(new IdentityRole("Administrador"));
            //        var resultado2 = roleManager.Create(new IdentityRole("Recepcionista"));
            //        var resultado3 = roleManager.Create(new IdentityRole("Cliente"));
            //        var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            //        var resultado = userManager.AddToRole(idUsuarioActual, "Recepcionista");
            //    }
            //}//fin del if          
            return View();
        }//fin del index

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}