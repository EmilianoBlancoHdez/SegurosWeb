namespace Seguros.Models.ViewModels
{
    using System;

    public class ClientesViewModel
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaModificacion { get; set; }
        public int[] Planes { get; set; }
        public bool Activo { get; set; }
    }
}