namespace ABDContexto
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AltaClientes
    {
        public int ID { get; set; }

        public int? IDCliente { get; set; }

        public DateTime? FechaAlta { get; set; }

        public virtual Clientes Clientes { get; set; }
    }
}
