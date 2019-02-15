using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System.Linq;

namespace Contains {
    class TableA {
        [Key]
        public int Id { set; get; }
        public string N1 { set; get; }
    }

    public class TableB {
        [Key]
        public int Id { set; get; }
        public string N2 { set; get; }
    }

    class MyContext : DbContext {
        public DbSet<TableA> TableA { set; get; }
        public DbSet<TableB> TableB { set; get; }

        public MyContext(DbContextOptions options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<TableA>().HasData(
                new TableA { Id = 1, N1 = "An implementation of the SQL LIKE operation" },
                new TableA { Id = 2, N1 = "This API supports the Entity Framework Core infrastructure" }
            );
            modelBuilder.Entity<TableB>().HasData(
                new TableB { Id = 1, N2 = "N21" },
                new TableB { Id = 2, N2 = "N22" }
            );
        }
    }
    class Program {
        static void Main(string[] args) {
            var collection = new ServiceCollection();
            collection.AddDbContext<MyContext>(option => {
                var str = "Host=localhost;Database=Contain;User Id=postgres;Password=1234";
                var factory = new LoggerFactory(new[] { new ConsoleLoggerProvider((_, __) => true, true) });
                option.UseLoggerFactory(factory);
                option.UseNpgsql(str);
            });

            var provider = collection.BuildServiceProvider();
            var context = provider.GetService<MyContext>();

            // context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var array = new string[] {
                "%SQL%",
                "%API%",
            };

            /*
                SELECT *
                FROM  public."TableA"
                WHERE "N1" LIKE ANY (array['%SQL%', '%API%']) = TRUE
             */

            // var results = context.TableA.Where(tableA => array.Any(arrayK => tableA.N1.Contains(arrayK)));
            var results = context.TableA.Where(tableA =>
                array.Any(arrayK => EF.Functions.Like(tableA.N1, arrayK)));

            foreach (var item in results) {
                Console.WriteLine($"-- {item.N1}");
            }
        }
    }
}
