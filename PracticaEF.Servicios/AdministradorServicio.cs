using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PracticaEF.Data;
using System.Security.Cryptography;
using System.Web;


namespace PracticaEF.Servicios
{
    public class AdministradorServicio
    {

        private readonly Entities ctx = new Entities();
        public Usuarios BuscarPorId(int id)
        {
            return ctx.Usuarios.SingleOrDefault(o => o.IdUsuario == id);
        }

    }
}

