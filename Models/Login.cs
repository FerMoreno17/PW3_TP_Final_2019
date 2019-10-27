using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Models
{
    public class Login
    {
        [Required(ErrorMessage = "El E-mail es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato valido: direccion@company.com")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public int Valor { get; set; }
    }
}