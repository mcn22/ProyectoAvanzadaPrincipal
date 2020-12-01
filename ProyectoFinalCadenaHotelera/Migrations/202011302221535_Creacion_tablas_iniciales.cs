namespace ProyectoFinalCadenaHotelera.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Creacion_tablas_iniciales : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EstadoReservas",
                c => new
                    {
                        EstadoReservaId = c.Decimal(nullable: false, precision: 18, scale: 0, identity: true, storeType: "numeric"),
                        nombre_estado = c.String(nullable: false, maxLength: 150),
                    })
                .PrimaryKey(t => t.EstadoReservaId);
            
            CreateTable(
                "dbo.Reservas",
                c => new
                    {
                        ReservaId = c.Decimal(nullable: false, precision: 18, scale: 0, identity: true, storeType: "numeric"),
                        fecha_llegada = c.String(nullable: false),
                        fecha_salida = c.String(nullable: false),
                        costo_total = c.Decimal(nullable: false, precision: 18, scale: 0, storeType: "numeric"),
                        saldo_actual = c.Decimal(nullable: false, precision: 18, scale: 0, storeType: "numeric"),
                        tipoHabitacionId = c.Int(nullable: false),
                        EstadoReservaId = c.Decimal(nullable: false, precision: 18, scale: 0, storeType: "numeric"),
                        Id = c.String(maxLength: 128),
                        HabitacionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ReservaId)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .ForeignKey("dbo.Habitacions", t => t.HabitacionId, cascadeDelete: true)
                .ForeignKey("dbo.EstadoReservas", t => t.EstadoReservaId, cascadeDelete: true)
                .Index(t => t.EstadoReservaId)
                .Index(t => t.Id)
                .Index(t => t.HabitacionId);
            
            CreateTable(
                "dbo.Valoraciones",
                c => new
                    {
                        ValoracionesId = c.Decimal(nullable: false, precision: 18, scale: 0, identity: true, storeType: "numeric"),
                        comentario = c.String(nullable: false),
                        puntuacion = c.Decimal(nullable: false, precision: 18, scale: 0, storeType: "numeric"),
                        Id = c.String(maxLength: 128),
                        HotelId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ValoracionesId)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .ForeignKey("dbo.Hotels", t => t.HotelId, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.HotelId);
            
            CreateTable(
                "dbo.Hotels",
                c => new
                    {
                        HotelId = c.Int(nullable: false, identity: true),
                        nombre_hotel = c.String(nullable: false, maxLength: 150),
                        descripcion_hotel = c.String(nullable: false),
                        imagen_hotel = c.String(nullable: false),
                        direccion_hotel = c.String(nullable: false),
                        ciudad_hotel = c.String(nullable: false),
                        telefono_hotel = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.HotelId);
            
            CreateTable(
                "dbo.Habitacions",
                c => new
                    {
                        HabitacionId = c.Int(nullable: false, identity: true),
                        num_habitacion = c.Int(nullable: false),
                        TipoHabitacionId = c.Decimal(nullable: false, precision: 18, scale: 0, storeType: "numeric"),
                        HotelId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.HabitacionId)
                .ForeignKey("dbo.Hotels", t => t.HotelId, cascadeDelete: true)
                .ForeignKey("dbo.TipoHabitacions", t => t.TipoHabitacionId, cascadeDelete: true)
                .Index(t => t.TipoHabitacionId)
                .Index(t => t.HotelId);
            
            CreateTable(
                "dbo.TipoHabitacions",
                c => new
                    {
                        TipoHabitacionId = c.Decimal(nullable: false, precision: 18, scale: 0, identity: true, storeType: "numeric"),
                        nombre_tipo = c.String(nullable: false, maxLength: 128),
                        descripcion_tipo = c.String(nullable: false, maxLength: 250),
                        costo_noche_habitacion = c.Decimal(nullable: false, precision: 18, scale: 0, storeType: "numeric"),
                        imagen_tipo = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.TipoHabitacionId);
            
            CreateTable(
                "dbo.Hotel_Empleado",
                c => new
                    {
                        Hotel_EmpleadoID = c.Decimal(nullable: false, precision: 18, scale: 0, identity: true, storeType: "numeric"),
                        Id = c.String(maxLength: 128),
                        HotelId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Hotel_EmpleadoID)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .ForeignKey("dbo.Hotels", t => t.HotelId, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.HotelId);
            
            AddColumn("dbo.AspNetUsers", "Nombre", c => c.String());
            AddColumn("dbo.AspNetUsers", "Apellido", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reservas", "EstadoReservaId", "dbo.EstadoReservas");
            DropForeignKey("dbo.Valoraciones", "HotelId", "dbo.Hotels");
            DropForeignKey("dbo.Hotel_Empleado", "HotelId", "dbo.Hotels");
            DropForeignKey("dbo.Hotel_Empleado", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Habitacions", "TipoHabitacionId", "dbo.TipoHabitacions");
            DropForeignKey("dbo.Reservas", "HabitacionId", "dbo.Habitacions");
            DropForeignKey("dbo.Habitacions", "HotelId", "dbo.Hotels");
            DropForeignKey("dbo.Valoraciones", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Reservas", "Id", "dbo.AspNetUsers");
            DropIndex("dbo.Hotel_Empleado", new[] { "HotelId" });
            DropIndex("dbo.Hotel_Empleado", new[] { "Id" });
            DropIndex("dbo.Habitacions", new[] { "HotelId" });
            DropIndex("dbo.Habitacions", new[] { "TipoHabitacionId" });
            DropIndex("dbo.Valoraciones", new[] { "HotelId" });
            DropIndex("dbo.Valoraciones", new[] { "Id" });
            DropIndex("dbo.Reservas", new[] { "HabitacionId" });
            DropIndex("dbo.Reservas", new[] { "Id" });
            DropIndex("dbo.Reservas", new[] { "EstadoReservaId" });
            DropColumn("dbo.AspNetUsers", "Apellido");
            DropColumn("dbo.AspNetUsers", "Nombre");
            DropTable("dbo.Hotel_Empleado");
            DropTable("dbo.TipoHabitacions");
            DropTable("dbo.Habitacions");
            DropTable("dbo.Hotels");
            DropTable("dbo.Valoraciones");
            DropTable("dbo.Reservas");
            DropTable("dbo.EstadoReservas");
        }
    }
}
