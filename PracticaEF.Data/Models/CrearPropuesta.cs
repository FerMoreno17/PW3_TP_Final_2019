using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Models
{
    public class CrearPropuesta
    {

        [Required(ErrorMessage = "El título es necesario.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Ingresa una descripción.")]
        [MaxLength(800,ErrorMessage ="Solo puedes escribir hasta 800 caracteres.")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "La fecha de fin  es obligatoria.")]
        [DataType(DataType.Date)]
        public DateTime FechaFin { get; set; }

        [Required(ErrorMessage = "Necesitamos tu teléfono")]
        public string TelefonoContacto { get; set; }

        [Required(ErrorMessage = "Indica el tipo de donación que deseas recibir.")]
        public int TipoDonacion { get; set; }
    }
}