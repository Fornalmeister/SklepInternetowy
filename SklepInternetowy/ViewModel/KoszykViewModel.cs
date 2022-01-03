using SklepInternetowy.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SklepInternetowy.ViewModel
{
    public class KoszykViewModel
    {
        public List<PozycjaKoszyka> PozycjaKoszyka { get; set; }
        
        public decimal CenaCalkowita { get; set; }
    
    }
}