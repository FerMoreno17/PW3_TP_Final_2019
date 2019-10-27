using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Models
{
    public class RegistrarUsuario
    {
        [Required(ErrorMessage = "El E-mail es obligatorio")]
        [MaxLength(50, ErrorMessage = "50 caracteres como Maximo")]
        [RegularExpression(@"^(([a-z0-9._%+-]+)(@)([a-z0-9.-]+)(.)([a-z]))", ErrorMessage = "Mail incorrecto")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [MinLength(8, ErrorMessage = "Debe contener como mínimo 8 caracteres")]
        [RegularExpression(@"^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$",ErrorMessage = "Debe poseer minimamente 1 Mayúsc. y 1 número")]
        [Compare("Password2", ErrorMessage ="Las contraseñas no coinciden")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "La confirmación es obligatoria")]
        public string Password2 { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
        [DataType(DataType.Date)]
        public System.DateTime FechaNacimiento { get; set; }
    }
}