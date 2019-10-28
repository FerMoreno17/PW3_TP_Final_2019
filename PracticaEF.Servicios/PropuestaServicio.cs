using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        /*public List<Propuestas> Top5()
        {
            //return (from prop in ctx.Propuestas select prop).ToList();

            /*return (from propuesta in ctx.Propuestas
                    join usuario in ctx.Usuarios
                    on propuesta.IdUsuarioCreador equals usuario.IdUsuario
                    where propuesta.Estado == 1
                    select propuesta
            );
        }*/
    }
}
