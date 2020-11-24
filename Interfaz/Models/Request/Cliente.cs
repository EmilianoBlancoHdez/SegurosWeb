﻿namespace Interfaz.Models.Request
{
    using System.ComponentModel.DataAnnotations;

    public class Cliente
    {
        [Required]
        public string Nombre { get; set; }
        public string MensajeError { get; set; }
        public int[] Planes { get; set; }
    }
}