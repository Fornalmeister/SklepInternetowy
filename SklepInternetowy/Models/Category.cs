using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SklepInternetowy.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Wprowadz nazwe kategorii")]
        [StringLength(100)]
        public string NameCategory { get; set; }
        [Required(ErrorMessage = "Wprowadz opis kategorii")]
        public string DescriptionCategory { get; set; }
        public string NameImg { get; set; }

        public virtual ICollection<Kurs> Kursy { get; set; }


    }
}