using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PracticaEF.Data;
using System.Security.Cryptography;
using Models;

namespace PracticaEF.Servicios
{
    public class UsuarioServicio
    {
        SHA1CryptoServiceProvider crypt = new SHA1CryptoServiceProvider();
        private readonly Entities1 ctx = new Entities1();
        
        public int RegistrarUsuario(RegistrarUsuario us)
        {
            Usuarios u = new Usuarios();
            u.Email = us.Email;
            u.Password = us.Password;
            u.FechaNacimiento = us.FechaNacimiento;
            u.TipoUsuario = 0;
            u.FechaCreacion = DateTime.Today;
            u.Activo = false;
            u.Foto = "default.jpg";
            u.Token = Guid.NewGuid().ToString();
            ctx.Usuarios.Add(u);
            ctx.SaveChanges();
            return 1;
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

        public Usuarios GetUsuario(Usuarios u)
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
