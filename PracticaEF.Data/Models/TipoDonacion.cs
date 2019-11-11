using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Models
{
    public class TipoDonacion
    {
        public int Td { get; set; }
        [Required(ErrorMessage = "Ups! no te olvides este dato.")]
        public string ValorString { get; set; }
        [Required(ErrorMessage = "Ups! no te olvides este dato.")]
        public int ValorInt { get; set; }
    }
}