using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Model
{
    //Esta clase parcial la crea ef core por medio de "Reverse Engineering" 
    //segun la documentacion oficial de microsoft, el metodo OnModelCreating 
    //se encarga de traducir las clases modeladas a tablas de una bd, en mi caso 
    //como lo hice bajo el enfoque "database first" en lugar de "code firts" esta clase
    //se crea en base la bd
    public partial class MaviContext : DbContext
    {
        public MaviContext()
        {
        }

        public MaviContext(DbContextOptions<MaviContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ContactosMavidavidBlancos> ContactosMavidavidBlancos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContactosMavidavidBlancos>(entity =>
            {
                entity.ToTable("ContactosMAVIDavidBlancos");

                entity.Property(e => e.EstadoCivil)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.FecheDeNacimiento)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Sexo)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.TelefonoFijo)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.TelefonoMovil)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.TipoContacto)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
