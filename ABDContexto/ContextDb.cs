namespace ABDContexto
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ContextDb : DbContext
    {
        public ContextDb()
            : base("data source=DESARROLLO-JUAN;initial catalog=Seguros;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework")
        {
        }

        public virtual DbSet<AltaClientes> AltaClientes { get; set; }
        public virtual DbSet<Clientes> Clientes { get; set; }
        public virtual DbSet<ClientesPlanes> ClientesPlanes { get; set; }
        public virtual DbSet<Coberturas> Coberturas { get; set; }
        public virtual DbSet<Planes> Planes { get; set; }
        public virtual DbSet<PlanesCobertura> PlanesCobertura { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Clientes>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Clientes>()
                .HasMany(e => e.AltaClientes)
                .WithOptional(e => e.Clientes)
                .HasForeignKey(e => e.IDCliente);

            modelBuilder.Entity<Clientes>()
                .HasMany(e => e.ClientesPlanes)
                .WithOptional(e => e.Clientes)
                .HasForeignKey(e => e.IDClientes);

            modelBuilder.Entity<Coberturas>()
                .Property(e => e.Descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<Coberturas>()
                .HasMany(e => e.PlanesCobertura)
                .WithOptional(e => e.Coberturas)
                .HasForeignKey(e => e.IDCoberturas);

            modelBuilder.Entity<Planes>()
                .Property(e => e.Descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<Planes>()
                .HasMany(e => e.ClientesPlanes)
                .WithOptional(e => e.Planes)
                .HasForeignKey(e => e.IDPlanes);

            modelBuilder.Entity<Planes>()
                .HasMany(e => e.PlanesCobertura)
                .WithOptional(e => e.Planes)
                .HasForeignKey(e => e.IDPlanes);
        }
    }
}
