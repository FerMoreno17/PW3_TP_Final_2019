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
            user.Nombre =  u.Nombre;
            user.Apellido = u.Apellido;
            user.FechaNacimiento = u.FechaNacimiento;
            user.Email = u.Email;

            /*byte[] vectoBytes = System.Text.Encoding.UTF8.GetBytes(u.Password);
            byte[] inArray = crypt.ComputeHash(vectoBytes);
            crypt.Clear();*/

            user.Password = u.Password;

            user.UserName =string.Concat(u.Nombre,u.Apellido);
            user.TipoUsuario = 1;
            user.FechaCreacion = DateTime.Now;
            user.Activo = false;
            user.Foto = "default.jpg";
            user.Token ="aaaa";
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

        public Usuarios getUsuario(Usuarios u)
        {
           var flag = ctx.Usuarios.Single(o => o.Email == u.Email);
            return flag;
        }

        public void Eliminar(int id)
        {
           
        }

        public List<Usuarios> ObtenerTodos()
        {
            return ctx.Usuarios.ToList();
        }

        public void Modifica(Usuarios u)
        {
            
        }
    }
}
