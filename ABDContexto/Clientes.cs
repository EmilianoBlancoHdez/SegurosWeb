namespace ABDContexto
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Clientes
    {
        public int ID { get; set; }

        [Required]
        [StringLength(20)]
        public string Nombre { get; set; }

        public DateTime? FechaModificacion { get; set; }
    }
}
