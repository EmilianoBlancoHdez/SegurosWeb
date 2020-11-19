namespace Interfaz.Models.Request
{
    using System.ComponentModel.DataAnnotations;

    public class Cliente
    {
        [Required]
        public string Nombre { get; set; }
    }
}