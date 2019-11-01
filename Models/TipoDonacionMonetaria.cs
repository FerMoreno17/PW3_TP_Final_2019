using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Models
{
    public class TipoDonacionMonetaria
    {
        [Required(ErrorMessage = "Ups! no te olvides este dato.")]
        public decimal Dinero { get; set; }
        [Required(ErrorMessage = "Ups! no te olvides este dato.")]
        public string CBU { get; set; }
    }
}