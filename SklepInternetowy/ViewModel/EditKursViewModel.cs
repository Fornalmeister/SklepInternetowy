using SklepInternetowy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SklepInternetowy.ViewModel
{
    public class EditKursViewModel
    {
        public Kurs Kurs { get; set; }
        public IEnumerable<Category> Category { get; set; }
        public bool? Potwierdzenie { get; set; }
    }
}