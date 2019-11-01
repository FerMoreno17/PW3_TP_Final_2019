using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PracticaEF.Data;
using System.Security.Cryptography;


namespace PracticaEF.Servicios
{
    public class UsuarioServicio
    {
        SHA1CryptoServiceProvider crypt = new SHA1CryptoServiceProvider();
        private readonly Entities ctx = new Entities();
        
        public void Agregar(Usuarios u)
          {
            Usuarios user = new Usuarios();
            user.FechaNacimiento = u.FechaNacimiento;
            user.Email = u.Email;
            user.Password = u.Password;
            user.TipoUsuario = 0;
            user.FechaCreacion = DateTime.Today;
            user.Activo = false;
            user.Foto = "default.jpg";
            user.Token = Guid.NewGuid().ToString();
            ctx.Usuarios.Add(user);
            ctx.SaveChanges();
        }

        public int Login(Usuarios u)
        {
            int resp;
            if(ctx.Usuarios.FirstOrDefault(o => o.Email == u.Email) != null)
            {
                var user = ctx.Usuarios.Single(o => o.Email == u.Email);
                if(user.Password == u.Password)
                {
                    resp = 1;
                }
                else
                {
                    resp = 0;
                }
            }
            else
            {
                resp = 0;
            }
            return resp;
        }

        public int ValidarEmail(String us)
        {
            if (ctx.Usuarios.SingleOrDefault(o => o.Email == us) != null)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public int ValidarEdad(DateTime fecha)
        {
            int entrada = Convert.ToInt32(fecha.Year);
            int hoy = Convert.ToInt32(DateTime.Today.Year);
            int calc = hoy - entrada;
            if (calc >= 18)
            {
                return 1;
            }
            return 0;
        }

        public Usuarios getUsuario(Usuarios u)
        {
           var flag = ctx.Usuarios.Single(o => o.Email == u.Email);
            return flag;
        }

        public List<Usuarios> ObtenerTodos()
        {
            return ctx.Usuarios.ToList();
        }

    }
}
