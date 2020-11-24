namespace Seguros.Models.Request
{
    using System;

    public class Cliente
    {
        public string Nombre { get; set; }
        public int[] Planes { get; set; }
    }
}