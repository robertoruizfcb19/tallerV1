using Microsoft.EntityFrameworkCore;
using tallerV1.Models.Entities;

namespace tallerV1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSets
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Equipo> Equipos { get; set; }
        public DbSet<OrdenTrabajo> OrdenesTrabajo { get; set; }
        public DbSet<OrdenTrabajoRepuesto> OrdenesTrabajoRepuestos { get; set; }
        public DbSet<OrdenTrabajoImagen> OrdenesTrabajoImagenes { get; set; }
        public DbSet<Bodega> Bodegas { get; set; }
        public DbSet<Repuesto> Repuestos { get; set; }
        public DbSet<MantenimientoPreventivo> MantenimientosPreventivos { get; set; }
        public DbSet<MantenimientoPreventivoTarea> MantenimientosPreventivosTareas { get; set; }
        public DbSet<MovimientoInventario> MovimientosInventario { get; set; }
        public DbSet<HistorialEquipo> HistorialEquipos { get; set; }
        public DbSet<Notificacion> Notificaciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de relaciones

            // Usuario - Empresa
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Empresa)
                .WithMany(e => e.Usuarios)
                .HasForeignKey(u => u.EmpresaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Equipo - Empresa
            modelBuilder.Entity<Equipo>()
                .HasOne(e => e.Empresa)
                .WithMany(emp => emp.Equipos)
                .HasForeignKey(e => e.EmpresaId)
                .OnDelete(DeleteBehavior.Restrict);

            // OrdenTrabajo - Equipo
            modelBuilder.Entity<OrdenTrabajo>()
                .HasOne(ot => ot.Equipo)
                .WithMany(e => e.OrdenesTrabajo)
                .HasForeignKey(ot => ot.EquipoId)
                .OnDelete(DeleteBehavior.Restrict);

            // OrdenTrabajo - Usuario (Creador)
            modelBuilder.Entity<OrdenTrabajo>()
                .HasOne(ot => ot.UsuarioCreador)
                .WithMany(u => u.OrdenesCreadas)
                .HasForeignKey(ot => ot.UsuarioCreadorId)
                .OnDelete(DeleteBehavior.Restrict);

            // OrdenTrabajo - Usuario (Asignado)
            modelBuilder.Entity<OrdenTrabajo>()
                .HasOne(ot => ot.UsuarioAsignado)
                .WithMany(u => u.OrdenesAsignadas)
                .HasForeignKey(ot => ot.UsuarioAsignadoId)
                .OnDelete(DeleteBehavior.Restrict);

            // OrdenTrabajo - Usuario (Aprobador)
            modelBuilder.Entity<OrdenTrabajo>()
                .HasOne(ot => ot.UsuarioAprobador)
                .WithMany()
                .HasForeignKey(ot => ot.UsuarioAprobadorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Repuesto - Bodega
            modelBuilder.Entity<Repuesto>()
                .HasOne(r => r.Bodega)
                .WithMany(b => b.Repuestos)
                .HasForeignKey(r => r.BodegaId)
                .OnDelete(DeleteBehavior.Restrict);

            // MantenimientoPreventivo - Equipo
            modelBuilder.Entity<MantenimientoPreventivo>()
                .HasOne(mp => mp.Equipo)
                .WithMany(e => e.MantenimientosPreventivos)
                .HasForeignKey(mp => mp.EquipoId)
                .OnDelete(DeleteBehavior.Restrict);

            // HistorialEquipo - Usuario
            modelBuilder.Entity<HistorialEquipo>()
                .HasOne(he => he.Usuario)
                .WithMany()
                .HasForeignKey(he => he.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // MovimientoInventario - Usuario
            modelBuilder.Entity<MovimientoInventario>()
                .HasOne(mi => mi.Usuario)
                .WithMany()
                .HasForeignKey(mi => mi.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // Índices únicos
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<Equipo>()
                .HasIndex(e => e.Codigo)
                .IsUnique();

            modelBuilder.Entity<OrdenTrabajo>()
                .HasIndex(ot => ot.NumeroOrden)
                .IsUnique();

            modelBuilder.Entity<Repuesto>()
                .HasIndex(r => r.Codigo)
                .IsUnique();

            // Datos iniciales (seed data)
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Empresas
            modelBuilder.Entity<Empresa>().HasData(
                new Empresa { Id = 1, Nombre = "Top Green S.A", Activo = true, FechaCreacion = DateTime.Now },
                new Empresa { Id = 2, Nombre = "Tecnología Agrícola S.A", Activo = true, FechaCreacion = DateTime.Now },
                new Empresa { Id = 3, Nombre = "Servicios Agrícolas del Pacífico", Activo = true, FechaCreacion = DateTime.Now }
            );

            // Bodegas
            modelBuilder.Entity<Bodega>().HasData(
                new Bodega { Id = 1, Nombre = "San Ignacio", Activo = true, FechaCreacion = DateTime.Now },
                new Bodega { Id = 2, Nombre = "Mirador", Activo = true, FechaCreacion = DateTime.Now },
                new Bodega { Id = 3, Nombre = "Bonanza", Activo = true, FechaCreacion = DateTime.Now }
            );

            // Usuario Admin por defecto
            // Password: Admin123! (hasheado con BCrypt)
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario
                {
                    Id = 1,
                    NombreCompleto = "Administrador del Sistema",
                    Email = "admin@taller.com",
                    Username = "admin",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                    Rol = Models.Enums.RolEnum.Administrador,
                    EmpresaId = 1,
                    Activo = true,
                    FechaCreacion = DateTime.Now
                }
            );
        }
    }
}
