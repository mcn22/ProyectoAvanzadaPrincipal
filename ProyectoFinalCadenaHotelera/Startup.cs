using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using ProyectoFinalCadenaHotelera.Models;
using System;
using System.Threading.Tasks;

[assembly: OwinStartupAttribute(typeof(ProyectoFinalCadenaHotelera.Startup))]
namespace ProyectoFinalCadenaHotelera
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
