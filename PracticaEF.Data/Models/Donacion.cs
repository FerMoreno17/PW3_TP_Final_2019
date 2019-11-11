using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Models
{
    public class Donacion
    {
        public int TipoDonacion { get; set; }
         public int IdPropuestaDonacionMonetaria { get; set; }
        public int IdPropuestaDonacionInsumo { get; set; }
        public int IdPropuestaDonacionHorasTrabajo { get; set; }

        public int IdPropuesta { get; set; }
        public int IdUsuario { get; set; }
        [Required(ErrorMessage = "La cantidad es necesaria.")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "Ingresa el monto depositado.")]
        public decimal Dinero { get; set; }

        [Required(ErrorMessage = "Compartenos la imagen del comprobante de deposito o transferencia, gracias.")]
        [FileExtensions(Extensions = "jpg|jpeg|png|gif", ErrorMessage = "Archivos válidos: jpg|jpeg|png|gif")]
        public string ArchivoTransferencia { get; set; }
        public System.DateTime FechaCreacion { get; set; }
    }
}