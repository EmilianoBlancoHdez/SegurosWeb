namespace Interfaz.Models.Request
{
    using System.ComponentModel.DataAnnotations;

    public class Planes
    {
        [Required]
        public string Descripcion { get; set; }
        public string MensajeError { get; set; }
        public int[] Coberturas { get; set; }
    }
}