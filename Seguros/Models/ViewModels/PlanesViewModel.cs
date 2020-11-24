namespace Seguros.Models.ViewModels
{
    using System;
    public class PlanesViewModel
    {
        public int ID { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public int[] Coberturas { get; set; }
    }
}