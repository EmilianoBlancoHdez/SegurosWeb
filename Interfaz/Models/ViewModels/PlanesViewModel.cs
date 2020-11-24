namespace Interfaz.Models.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class PlanesViewModel
    {
        public int ID { get; set; }
        [Required]
        public string Descripcion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string MensajeError { get; set; }
        public int[] Coberturas { get; set; }
    }
}