

using System.ComponentModel.DataAnnotations;

namespace Model.DTO
{
    //Esta clase se usa como DTO de la clase mapeada por entity framework 

    public class ContactUpdateDto
    {
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string TipoContacto { get; set; }
        [Required]
        [RegularExpression("([1-9][0-9]*)")]
        public string TelefonoFijo { get; set; }
        [Required]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Must be a natural number")]
        public string TelefonoMovil { get; set; }
        [Required]
        public string FecheDeNacimiento { get; set; }
        [Required]
        public string Sexo { get; set; }
        [Required]
        public string EstadoCivil { get; set; }
    }
}