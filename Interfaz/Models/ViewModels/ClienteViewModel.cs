namespace Interfaz.Models.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ClienteViewModel
    {
        public int ID { get; set; }
        [Required]
        public string Nombre { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string MensajeError { get; set; }
        public int[] Planes { get; set; }
        public bool Activo { get; set; }
    }
}