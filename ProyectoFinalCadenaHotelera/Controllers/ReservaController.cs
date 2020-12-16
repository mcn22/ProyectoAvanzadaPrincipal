using Microsoft.AspNet.Identity;
using ProyectoFinalCadenaHotelera.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace ProyectoFinalCadenaHotelera.Controllers
{

    public class ReservaController : Controller
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

        public ActionResult PreReserva(int? id)
        {
            List<TipoHabitacion> listaTiposHabitacion = new List<TipoHabitacion>();
            listaTiposHabitacion = listarTiposHabitacion();
            ViewBag.ListaTipos = listaTiposHabitacion.ToList();
                        
            using (var ctx = new ApplicationDbContext())
            {
                var hotel = ctx.Hoteles.Where(h => h.HotelId == id).FirstOrDefault();
                ViewBag.Hotel = hotel;
                return View();
            }
        }//fin prereserva

        [HttpPost, ActionName("PreReserva")]
        [ValidateAntiForgeryToken]
        public ActionResult CrearReserva()
        {
            if (Request.IsAuthenticated)
            {
                Reserva reservaParcial = new Reserva();
                DateTime llegada = Convert.ToDateTime(Request["fecha_llegada"]);
                DateTime salida = Convert.ToDateTime(Request["fecha_salida"]);
                int diasDeHospedaje = DiasHospedaje(llegada, salida);

                string txtLlegada = Request["fecha_llegada"];
                string txtsalida = Request["fecha_salida"];
                int tipoHabitacion = int.Parse(Request["tipoHabitacionId"]);
                int idHotel = int.Parse(Request["idHotel"]);
                int costoTotal = CalculaCosto(diasDeHospedaje, tipoHabitacion);

                int idHabitacion = ValidaDisponibilidad(llegada, salida, idHotel, tipoHabitacion);
                if (idHabitacion != 0)
                {
                    reservaParcial.fecha_llegada = txtLlegada;
                    reservaParcial.fecha_salida = txtsalida;
                    reservaParcial.Id = HttpContext.User.Identity.GetUserId();
                    reservaParcial.tipoHabitacionId = tipoHabitacion;
                    reservaParcial.costo_total = costoTotal;
                    reservaParcial.EstadoReservaId = 1;
                    reservaParcial.HabitacionId = idHabitacion;
                    return RedirectToAction("ConfirmarReserva", reservaParcial);
                }
                else {
                    TempData["Mensaje"] = "No hay disponibilidad actualmente.";
                    return RedirectToAction("PreReserva", new { id = idHotel});                  
                }
            }
            else
            {
                return RedirectToAction("../Account/Login");
            }
        }//fin del post de la prereserva

        public static int ValidaDisponibilidad(DateTime llegada, DateTime salida, int idHotel, int tipoHabitacion)
        {
            int resultado = 0;
            try
            {
                using (var ctx = new ApplicationDbContext())
                {                   
                    var listaHabitaciones = ctx.Habitaciones.Where(k => k.HotelId == idHotel && k.TipoHabitacionId == tipoHabitacion).ToList();
                    foreach (var habitacion in listaHabitaciones)
                    {
                            var listaReservas = ctx.Reservas.Where(r => r.Habitacion.HabitacionId == habitacion.HabitacionId && r.EstadoReservaId != 3).ToList();
                            if (listaReservas.Count == 0)
                            {
                                resultado = habitacion.HabitacionId;
                                break;
                            }
                            else
                            {
                                foreach (var reserva in listaReservas)
                                {

                                    if (llegada >= Convert.ToDateTime(reserva.fecha_llegada) && llegada <= Convert.ToDateTime(reserva.fecha_salida) ||
                                        salida >= Convert.ToDateTime(reserva.fecha_llegada) && salida <= Convert.ToDateTime(reserva.fecha_salida))
                                    {
                                        resultado = 0;
                                        break;
                                    }
                                    else
                                    {
                                        resultado = habitacion.HabitacionId;
                                    }

                                }
                            }
                    }
                }
            }
            catch (Exception)
            {
                resultado = 0;
            }
            return resultado;
        }//fin de la validacion de la disponibilidad


        [Authorize(Roles = "Cliente")]
        public ActionResult ConfirmarReserva(Reserva reservaParcial)
        {
            return View();
        }


        [HttpPost, ActionName("ConfirmarReserva")]
        [ValidateAntiForgeryToken]
        public ActionResult ProcesaReserva()
        {
            string txtLlegada = Request["fecha_llegada"];
            string txtSalida = Request["fecha_salida"];
            string UserId_reserva = (Request["Id"]);
             int id_habitacion_reserva = int.Parse(Request["HabitacionId"]);
            int estado_reserva = int.Parse(Request["EstadoReservaId"]);
            int costo_total = int.Parse(Request["costo_total"]);
            int id_Tipo = int.Parse(Request["tipoHabitacionId"]);

            Reserva reserva = new Reserva();
            reserva.fecha_llegada = txtLlegada;
            reserva.fecha_salida = txtSalida;
            reserva.ReservaId = 1;
            reserva.Id = UserId_reserva;
            reserva.HabitacionId = id_habitacion_reserva;
            reserva.EstadoReservaId = estado_reserva;
            reserva.costo_total = costo_total;
            reserva.saldo_actual = costo_total - ((int)(costo_total * 0.30));
            reserva.tipoHabitacionId = id_Tipo;
           
            if (CreaReserva(reserva))
            {
                return RedirectToAction("ListaReservasUsuario");
            }
            else
            {
                return RedirectToAction("PreReserva");
            }
            
        }

        [Authorize(Roles = "Cliente")]
        public ActionResult ListaReservasUsuario()
        {
            var userId = HttpContext.User.Identity.GetUserId().ToString();
            var ctx = new ApplicationDbContext();
            var listaReservas = ctx.Reservas.Where(k => k.Id == userId).ToList();
            return View(listaReservas);
        }

        [Authorize(Roles = "Cliente")]
        public ActionResult DetalleReserva(int ? id)
        {
            var ctx = new ApplicationDbContext();
            var reserva = ctx.Reservas.Where(k => k.ReservaId == id).FirstOrDefault();
            return View(reserva);
        }

        //---------------------------------------------------
        //metodos llamados desde la misma clase
        public static bool CreaReserva(Reserva datosReserva)
        {
            bool resultado = false;
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var reserva = new Reserva()
                    {
                        ReservaId = 0,
                        fecha_llegada = datosReserva.fecha_llegada,
                        fecha_salida = datosReserva.fecha_salida,
                        costo_total = datosReserva.costo_total,
                        Id = datosReserva.Id,
                        saldo_actual = datosReserva.saldo_actual,
                        EstadoReservaId = datosReserva.EstadoReservaId,
                        HabitacionId = datosReserva.HabitacionId,
                        tipoHabitacionId = datosReserva.tipoHabitacionId
                    };
                    ctx.Reservas.Add(reserva);
                    ctx.SaveChanges();
                    resultado = true;
                }
            }
            catch (Exception)
            {
                resultado = false;
            }
            return resultado;
        }

        private List<TipoHabitacion> listarTiposHabitacion()
        {
            List<TipoHabitacion> listaTiposHabitacion = new List<TipoHabitacion>();
            using (var ctx = new ApplicationDbContext())
            {
                var habitacion = ctx.TipoHabitaciones.ToList();
                foreach (var item in habitacion)
                {
                    listaTiposHabitacion.Add(
                        new TipoHabitacion
                        {
                            TipoHabitacionId = item.TipoHabitacionId,
                            nombre_tipo = item.nombre_tipo,
                            descripcion_tipo = item.descripcion_tipo,
                            costo_noche_habitacion = item.costo_noche_habitacion,
                            imagen_tipo = item.imagen_tipo,
                        }
                    );
                }
            }
            return listaTiposHabitacion;
        }//fin de listar tipos

        private int DiasHospedaje(DateTime llegada, DateTime salida)
        {
            return Convert.ToInt32((salida - llegada).TotalDays);
        }

        private int CalculaCosto(int dias, int id_habitacion)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var habitacion = ctx.TipoHabitaciones.Where(h => h.TipoHabitacionId == id_habitacion).FirstOrDefault();
                int costo = dias * habitacion.costo_noche_habitacion;
                return costo;
            }
        }

    }//fin de la clase
}//fin del namespace 70126122