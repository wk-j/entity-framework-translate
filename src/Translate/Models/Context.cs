using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace Translate.Models
{
    public class CustomOptions
    {
        public bool ShowSqlLog { set; get; }
    }

    public class EasyValidatorContext : DbContext
    {
        public DbSet<QFile> Files { set; get; }
        public DbSet<QProperty> Properties { set; get; }
        public DbSet<QTemplate> Templates { set; get; }
        public DbSet<QRejectReason> RejectReasons { set; get; }
        public DbSet<QReconcile> Reconciles { set; get; }


        private readonly CustomOptions customOptions;

        public EasyValidatorContext(DbContextOptions options, CustomOptions custom) : base(options)
        {
            this.customOptions = custom;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<QFile>()
                .HasIndex(x => x.Status);

            modelBuilder.Entity<QFile>()
                .HasIndex(x => x.Uuid);

            modelBuilder.Entity<QTemplate>()
                .HasIndex(x => x.Template);

            modelBuilder.Entity<QTemplate>()
                .HasIndex(x => x.ValidationRequire);

            modelBuilder.Entity<QProperty>()
                .HasIndex(x => x.Name);

            modelBuilder.Entity<QProperty>()
                .HasIndex(x => x.StringValue);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (customOptions.ShowSqlLog)
            {
                var factory = new LoggerFactory(new[] {
                    new ConsoleLoggerProvider( (s, l) => true, true)
               });
                optionsBuilder.UseLoggerFactory(factory);
            }
        }
    }
}