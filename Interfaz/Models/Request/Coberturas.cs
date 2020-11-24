namespace Interfaz.Models.Request
{
    using System.ComponentModel.DataAnnotations;

    public class Coberturas
    {
        [Required]
        public string Descripcion { get; set; }
        public string MensajeError { get; set; }
    }
}