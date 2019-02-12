using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Concat {
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
                new TableA { Id = 1, N1 = "N11" },
                new TableA { Id = 2, N1 = "N12" }
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
                var str = "Host=localhost;Database=Concat;User Id=postgres;Password=1234";
                var factory = new LoggerFactory(new[] { new ConsoleLoggerProvider((_, __) => true, true) });
                option.UseLoggerFactory(factory);
                option.UseNpgsql(str);
            });

            var provider = collection.BuildServiceProvider();
            var context = provider.GetService<MyContext>();

            context.Database.EnsureCreated();

            var query =
                from a in context.TableA
                join b in context.TableB on a.Id equals b.Id
                select new {
                    A = a.Id,
                    B = b.Id,
                    N12 = string.Concat(a.N1, b.N2)
                    // N12 = $"{a.N1}{b.N2}"
                };

            query = query.Where(x => x.N12.Contains("N12N22"));
            // query = query.Where(x => x.A == 1).Take(1);

            foreach (var item in query) {
                Console.WriteLine($" -- {item.A} {item.B} {item.N12}");
            }
        }
    }
}
