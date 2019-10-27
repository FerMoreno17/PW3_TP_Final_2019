using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PracticaEF.Data;

namespace PracticaEF.Servicios
{
    public class PropuestaServicio
    {
        Propuestas prop = new Propuestas();
        private readonly Entities ctx = new Entities();
        public void CargarPropuesta(Propuestas p, int id)
        {
            prop.Nombre = p.Nombre;
            prop.Descripcion = p.Descripcion;
            prop.FechaCreacion = DateTime.Today;
            prop.FechaFin = p.FechaFin;
            prop.Foto = "default.jpg";
            prop.TelefonoContacto = p.TelefonoContacto;
            prop.TipoDonacion = p.TipoDonacion;
            prop.IdUsuarioCreador = id;
            prop.Estado = 1;
            prop.Valoracion = 0;
            ctx.Propuestas.Add(prop);
            ctx.SaveChanges();
        }
    }
}
