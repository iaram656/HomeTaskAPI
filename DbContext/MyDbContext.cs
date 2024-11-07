

using Microsoft.EntityFrameworkCore;

namespace appAPI.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
            // Configurar el comportamiento de la conexión
            Database.SetCommandTimeout(TimeSpan.FromSeconds(30));
        }

        // Remover estas propiedades estáticas ya que no son necesarias
        // public static string ConnectionString { get; set; }
        // public static DbContextOptions<MyDbContext> Options { get; set; }

        public DbSet<TAREA> TAREA { get; set; }
        public DbSet<USER> USER { get; set; }
        public DbSet<PENALIZATION> PENALIZATION { get; set; }
        public DbSet<PENALIZATIONS> PENALIZATIONS { get; set; }
        public DbSet<TRABAJOS> TRABAJOS { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TAREA>(entity =>
            {
                // Añadir índices para mejorar el rendimiento
                entity.HasIndex(e => e.USERID);
                entity.HasIndex(e => e.STATUS);

                // Configurar el tipo de datos específicamente
                entity.Property(e => e.DESCRIPTION).HasMaxLength(500);
            });
        }

        // Agregar un método específico para consultas de solo lectura
        public IQueryable<TAREA> GetTareasReadOnly()
        {
            return TAREA.AsNoTracking();
        }
    }
}
