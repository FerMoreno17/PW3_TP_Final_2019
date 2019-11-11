using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Models
{
    public class TipoDonacionInsumos
    {
        [Required(ErrorMessage = "Ups! no te olvides este dato.")]
        public string NombreInsumo { get; set; }
        [Required(ErrorMessage = "Ups! no te olvides este dato.")]
        public int Cantidad { get; set; }
    }
}