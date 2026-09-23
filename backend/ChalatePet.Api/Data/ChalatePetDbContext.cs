using ChalatePet.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ChalatePet.Api.Data;

public class ChalatePetDbContext : DbContext
{
    public ChalatePetDbContext(DbContextOptions<ChalatePetDbContext> options)
        : base(options)
    {
    }

    public DbSet<Cliente> Clientes { get; set; }

    public DbSet<Mascota> Mascotas { get; set; }

    public DbSet<Servicio> Servicios { get; set; }

    public DbSet<Cita> Citas {get; set;}
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ==========================================
        // TABLA: clientes
        // ==========================================

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.ToTable("clientes");

            entity.HasKey(c => c.IdCliente);

            entity.Property(c => c.IdCliente)
                .HasColumnName("id_cliente")
                .ValueGeneratedOnAdd();

            entity.Property(c => c.Nombre)
                .HasColumnName("nombre")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(c => c.Apellido)
                .HasColumnName("apellido")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(c => c.TelefonoPrincipal)
                .HasColumnName("telefono_principal")
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(c => c.TelefonoSecundario)
                .HasColumnName("telefono_secundario")
                .HasMaxLength(20);

            entity.Property(c => c.Email)
                .HasColumnName("email")
                .HasMaxLength(150);

            entity.Property(c => c.Direccion)
                .HasColumnName("direccion")
                .HasMaxLength(255);

            entity.Property(c => c.FechaRegistro)
    .HasColumnName("fecha_registro")
    .IsRequired();

            entity.Property(c => c.Activo)
                .HasColumnName("activo")
                .HasDefaultValue(true)
                .IsRequired();
        });

        // ==========================================
        // TABLA: mascotas
        // ==========================================

        modelBuilder.Entity<Mascota>(entity =>
        {
            entity.ToTable("mascotas");

            entity.HasKey(m => m.IdMascota);

            entity.Property(m => m.IdMascota)
                .HasColumnName("id_mascota")
                .ValueGeneratedOnAdd();

            entity.Property(m => m.IdCliente)
                .HasColumnName("id_cliente")
                .IsRequired();

            entity.Property(m => m.Nombre)
                .HasColumnName("nombre")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(m => m.Especie)
                .HasColumnName("especie")
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(m => m.Raza)
                .HasColumnName("raza")
                .HasMaxLength(100);

            entity.Property(m => m.Sexo)
                .HasColumnName("sexo")
                .HasColumnType("char(1)")
                .IsRequired();

            entity.Property(m => m.FechaNacimiento)
                .HasColumnName("fecha_nacimiento");

            entity.Property(m => m.Color)
                .HasColumnName("color")
                .HasMaxLength(100);

            entity.Property(m => m.Observaciones)
                .HasColumnName("observaciones")
                .HasColumnType("text");

            entity.Property(m => m.Activo)
                .HasColumnName("activo")
                .HasDefaultValue(true)
                .IsRequired();

            // Relación:
            // Un Cliente tiene muchas Mascotas.
            // Una Mascota pertenece a un Cliente.
            entity.HasOne(m => m.Cliente)
                .WithMany()
                .HasForeignKey(m => m.IdCliente)
                .OnDelete(DeleteBehavior.Restrict);
        }

        );
        // ==========================================
        // TABLA: servicios
        // ==========================================

        modelBuilder.Entity<Servicio>(entity =>
        {
            entity.ToTable("servicios");

            entity.HasKey(s => s.IdServicio);

            entity.Property(s => s.IdServicio)
                .HasColumnName("id_servicio")
                .ValueGeneratedOnAdd();

            entity.Property(s => s.Nombre)
                .HasColumnName("nombre")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(s => s.Descripcion)
                .HasColumnName("descripcion")
                .HasColumnType("text");

            entity.Property(s => s.Precio)
                .HasColumnName("precio")
                .HasPrecision(10, 2)
                .IsRequired();

            entity.Property(s => s.DuracionMinutos)
                .HasColumnName("duracion_minutos");

            entity.Property(s => s.Activo)
                .HasColumnName("activo")
                .HasDefaultValue(true)
                .IsRequired();
        });

        // ==========================================
// TABLA: citas
// ==========================================

modelBuilder.Entity<Cita>(entity =>
{
    entity.ToTable("citas");

    entity.HasKey(c => c.IdCita);

    entity.Property(c => c.IdCita)
        .HasColumnName("id_cita")
        .ValueGeneratedOnAdd();

    entity.Property(c => c.IdMascota)
        .HasColumnName("id_mascota")
        .IsRequired();

    entity.Property(c => c.IdServicio)
        .HasColumnName("id_servicio")
        .IsRequired();

    entity.Property(c => c.FechaHora)
        .HasColumnName("fecha_hora")
        .IsRequired();

    entity.Property(c => c.Motivo)
        .HasColumnName("motivo")
        .HasMaxLength(255);

    entity.Property(c => c.Estado)
        .HasColumnName("estado")
        .HasMaxLength(30)
        .IsRequired();

    entity.Property(c => c.Observaciones)
        .HasColumnName("observaciones")
        .HasColumnType("text");

    entity.Property(c => c.FechaCreacion)
    .HasColumnName("fecha_creacion")
    .IsRequired();

    // Una Mascota puede tener muchas Citas.
    entity.HasOne(c => c.Mascota)
        .WithMany()
        .HasForeignKey(c => c.IdMascota)
        .OnDelete(DeleteBehavior.Restrict);

    // Un Servicio puede estar asociado a muchas Citas.
    entity.HasOne(c => c.Servicio)
        .WithMany()
        .HasForeignKey(c => c.IdServicio)
        .OnDelete(DeleteBehavior.Restrict);
});
    }
}