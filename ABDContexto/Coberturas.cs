namespace ABDContexto
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Coberturas
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Descripcion { get; set; }

        public DateTime? FechaModificacion { get; set; }
    }
}
