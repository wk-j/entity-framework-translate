using System;
using Translate.Models;

namespace Translate {
    internal class KProperty {
        public string Name { get; set; }
        public int Id { get; set; }
        public string StringValue { get; set; }
        public DateTime DateTimeValue { get; set; }
        public PropertyType PropertyType { get; set; }
        public string Title { get; set; }
    }
}