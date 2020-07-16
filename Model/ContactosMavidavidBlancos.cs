using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    //Esta clase la mapea entity framework, es la tabla de la base de datos
    public partial class ContactosMavidavidBlancos
    {
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string TipoContacto { get; set; }
        [RegularExpression("([1-9][0-9]*)")]
        public string TelefonoFijo { get; set; }
        [Required]
        [RegularExpression("([1-9][0-9]*)")]
        public string TelefonoMovil { get; set; }
        [Required]
        public string FecheDeNacimiento { get; set; }
        [Required]
        public string Sexo { get; set; }
        [Required]
        public string EstadoCivil { get; set; }
    }
}
