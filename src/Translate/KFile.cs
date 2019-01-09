using System;
using Translate.Models;

namespace Translate {
    internal class KFile {
        public int Id { get; set; }
        public string Template { get; set; }
        public string Creator { get; set; }
        public DateTime CreateDate { get; set; }
        public string Uuid { get; set; }
        public string Validator { get; set; }
        public DateTime ValidateDate { get; set; }
        public DateTime UploadDate { get; set; }
        public Status Status { get; set; }
        public string Comment { get; set; }
        public int Pages { get; set; }
        public string Reference { get; set; }
        public string DocumentType { get; set; }
        public string Category { get; set; }
        public string Department { get; set; }
        public string Site { get; set; }
    }
}