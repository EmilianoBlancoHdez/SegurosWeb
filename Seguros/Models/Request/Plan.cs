namespace Seguros.Models.Request
{
    using System;
    public class Plan
    {
        public string Descripcion { get; set; }
        public int[] Coberturas { get; set; }
    }
}