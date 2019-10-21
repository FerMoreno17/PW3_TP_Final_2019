using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PracticaEF.Data;
using System.Security.Cryptography;


namespace PracticaEF.Servicios
{
    public class AdministradorServicio
    {

        private readonly Entities ctx = new Entities();
        public Usuarios BuscarPorId(int id)
        {
            var resp = ctx.Usuarios.Single(o => o.IdUsuario == id);
            return resp;
        }

        public void ModificarUsuario(Usuarios u)
        {
            
        }
    }
}

