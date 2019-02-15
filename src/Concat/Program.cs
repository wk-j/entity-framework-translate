using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations.Schema;

namespace Concat {
    public class TableA {
        [Key]
        public int Id { set; get; }
        public string N1 { set; get; }
    }

    public class TableB {
        [Key]
        public int Id { set; get; }
        public string N2 { set; get; }

        [ForeignKey("Reference")]
        public TableA TableA { set; get; }

        public int Reference { set; get; }
    }

    class MyContext : DbContext {
        public DbSet<TableA> TableA { set; get; }
        public DbSet<TableB> TableB { set; get; }

        public MyContext(DbContextOptions options) : base(options) {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            // var factory = new LoggerFactory(new[] {
            //     new ConsoleLoggerProvider( (s, l) => true, true)
            // });
            // optionsBuilder.UseLoggerFactory(factory);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<TableA>().HasData(
                new TableA { Id = 1, N1 = "In the news" },
                new TableA { Id = 2, N1 = "NASA" }
            );
            modelBuilder.Entity<TableB>().HasData(
                new TableB { Id = 1, N2 = "Roma", Reference = 1 },
                new TableB { Id = 2, N2 = "Win", Reference = 2 }
            );
        }
    }

    class Program {

        static void Main(string[] args) {

            var collection = new ServiceCollection();
            collection.AddDbContext<MyContext>(option => {
                var str = "Host=localhost;Database=Concat;User Id=postgres;Password=1234";
                var factory = new LoggerFactory(new[] { new ConsoleLoggerProvider((_, __) => true, true) });
                option.UseLoggerFactory(factory);
                option.UseNpgsql(str);
            });

            var provider = collection.BuildServiceProvider();
            var context = provider.GetService<MyContext>();

            // context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var query =
                from a in context.TableA
                join b in context.TableB on a.Id equals b.Reference
                select new {
                    A = a.N1,
                    B = b.N2,
                };

            var queryA = query.Where(x => string.Concat(x.A, x.B).Contains("SAWin")).ToList();
            var queryB = query.Where(x => (x.A + x.B).Contains("SAWin")).ToList();

            Console.WriteLine(queryA.Count);
            Console.WriteLine(queryB.Count);
        }
    }
}
