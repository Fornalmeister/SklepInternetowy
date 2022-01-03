using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SklepInternetowy.Models
{
    public class Kurs
    {
        public int KursId { get; set; }
        public int CategoryId { get; set; }
        [Required(ErrorMessage ="Wprowadz nazwe kursu")]
        [StringLength(100)]
        public string Title { get; set; }
        [Required(ErrorMessage = "Wprowadz nazwe autora")]
        [StringLength(100)]
        public string Author { get; set; }
        public DateTime DateAdded { get; set; }
        [StringLength(100)]
        public string NameImg { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool Bestseller { get; set; }
        public bool Hidden { get; set; }
        public string DescriptionShort { get; set; }

        public virtual Category Category { get; set; }
    }
}