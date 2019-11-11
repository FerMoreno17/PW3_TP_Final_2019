using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Models
{
    public class ModificarUsuario
    {
        [Required(ErrorMessage = "El Nombre no puede quedar vacío.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Nos interesa saber tu Apellido.")]
        public string Apellido { get; set; }

        /*[Required(ErrorMessage = "La foto es obligatoria.")]
        [FileExtensions(Extensions = "jpg|jpeg|png|gif", ErrorMessage = "Your error message.")]
        public string Foto { get; set; }*/

    }
}