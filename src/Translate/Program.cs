using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Translate.Models;

namespace Translate {
    class Program {
        static void Main(string[] args) {

            var builder = new DbContextOptionsBuilder();
            builder.UseNpgsql("Host=localhost;Port=5432;User Id=postgres;Password=1234;Database=Translate");
            var options = builder.Options;

            using (var context = new EasyValidatorContext(options, new CustomOptions { ShowSqlLog = true })) {
                context.Database.EnsureCreated();

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
    }
}