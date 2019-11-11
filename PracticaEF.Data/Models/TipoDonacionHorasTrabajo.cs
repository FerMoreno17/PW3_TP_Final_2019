using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Models
{
    public class TipoDonacionHorasTrabajo
    {
        [Required(ErrorMessage = "Ups! no te olvides este dato.")]
        public int CantidadHoras { get; set; }
        [Required(ErrorMessage = "Ups! no te olvides este dato.")]
        public string Profesion { get; set; }
    }
}