namespace ABDContexto
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PlanesCobertura")]
    public partial class PlanesCobertura
    {
        public int ID { get; set; }

        public int? IDPlanes { get; set; }

        public int? IDCoberturas { get; set; }

        public virtual Coberturas Coberturas { get; set; }

        public virtual Planes Planes { get; set; }
    }
}
