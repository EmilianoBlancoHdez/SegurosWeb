namespace ABDContexto
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ClientesPlanes
    {
        public int ID { get; set; }

        public int? IDClientes { get; set; }

        public int? IDPlanes { get; set; }

        public virtual Clientes Clientes { get; set; }

        public virtual Planes Planes { get; set; }
    }
}
