using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Translate.Models;

namespace Translate {
    class Program {

        static EasyValidatorContext Context() {
            return MySql();
        }

        static EasyValidatorContext PostgreSQL() {
            var builder = new DbContextOptionsBuilder();
            builder.UseNpgsql("Host=localhost;Port=5432;User Id=postgres;Password=1234;Database=Translate");
            var options = builder.Options;
            return new EasyValidatorContext(options, new CustomOptions { ShowSqlLog = true });
        }

        static EasyValidatorContext MySql() {
            var conn = "Host=localhost; User Id=root; Password=1234; Database=Hello";

            var builder = new DbContextOptionsBuilder();
            builder.UseMySql(conn);

            var options = builder.Options;
            return new EasyValidatorContext(options, new CustomOptions { ShowSqlLog = true });
        }

        static void Create() {
            using (var context = Context()) {
                context.Database.EnsureCreated();
            }
        }

        static void A() {

            using (var context = Context()) {

                var query = from file in context.Files
                            join property in context.Properties on file.Id equals property.QFileId into properties
                            join reason in context.RejectReasons on file.Id equals reason.QFileId into reasons
                            join template in context.Templates on file.Template equals template.Template
                            where (template.ValidationRequire)
                            select new {
                                File = new KFile {
                                    Id = file.Id,
                                    Template = file.Template,
                                    Creator = file.Creator,
                                    CreateDate = file.CreateDate,
                                    Uuid = file.Uuid,
                                    Validator = file.Validator,
                                    ValidateDate = file.ValidateDate,
                                    UploadDate = file.UploadDate,
                                    Status = file.Status,
                                    Comment = file.Comment,
                                    Pages = file.Pages,
                                    //
                                    Reference = file.Reference,
                                    DocumentType = file.DocumentType,
                                    Category = file.Category,
                                    Department = file.Department,
                                    Site = file.Site
                                },
                                Properties = properties.Select(x => new KProperty {
                                    Name = x.Name,
                                    Id = x.Id,
                                    StringValue = x.StringValue,
                                    DateTimeValue = x.DateTimeValue,
                                    PropertyType = x.PropertyType,
                                    Title = x.Title
                                }),
                                Status = file.Status,
                                RejectReasons = reasons,
                            };

                var records = query.ToList();
            }
        }

        static void NoLimit() {
            Console.WriteLine(">> NO LIMIT ---------------------");
            using (var context = Context()) {
                var query =
                    from file in context.Files
                    join property in context.Properties on file.Id equals property.QFileId into properties
                    where properties.Count() > 10
                    select new {
                        File = file,
                        Properties = properties
                    };

                var a = query.OrderBy(x => x.File.Id).Take(100).ToList();
            }
        }

        static void Limit() {
            Console.WriteLine(">> LIMIT ----------------------");
            using (var context = Context()) {
                var query =
                    from file in context.Files
                    select new {
                        File = file,
                    };

                var a = query.OrderBy(x => x.File.Id).Take(100).ToList();
            }
        }

        static void Main(string[] args) {
            Create();
            NoLimit();
            Limit();
        }
    }
}