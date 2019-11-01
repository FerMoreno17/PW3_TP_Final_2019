using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Models
{
    public class Denuncia
    {
        public int IdPropuesta { get; set; }
        public int IdUsuario { get; set; } 
        public int IdMotivo { get; set; }

        [Required(ErrorMessage = "Debes contarnos porque denuncias.")]
        [MaxLength(300,ErrorMessage ="No puedes superar los 300 caracteres.")]
        [MinLength(30,ErrorMessage ="Tu comentario debe tener un mínimo de 30 caracteres.")]
        public string Comentarios { get; set; }
    }
}