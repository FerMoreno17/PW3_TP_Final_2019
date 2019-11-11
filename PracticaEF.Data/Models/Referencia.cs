using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Models
{
    public class Referencia
    {
        public int IdPropuesta { get; set; }
        [Required(ErrorMessage = "Ups! no te olvides este dato. Tus referencias son importantes.")]
        public string Nombre1 { get; set; }
        [Required(ErrorMessage = "ouch! se te ha olvidado el teléfono.")]
        public string Telefono1 { get; set; }
        [Required(ErrorMessage = "Ups! no te olvides este dato. Tus referencias son importantes.")]
        public string Nombre2 { get; set; }
        [Required(ErrorMessage = "ouch! se te ha olvidado el teléfono.")]
        public string Telefono2 { get; set; }
    }
}