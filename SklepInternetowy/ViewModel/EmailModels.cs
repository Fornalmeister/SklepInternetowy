using Postal;
using SklepInternetowy.Models;
using System.Collections.Generic;


namespace SklepInternetowy.ViewModel
{
    public class PotwierdzenieZamowieniaEmail : Email
    {
        public string To { get; set; }
        public string From { get; set; }
        public decimal Wartosc { get; set; }
        public int NumerZamowienia { get; set; }
        public string sciezkaObrazka { get; set; }
        public List<OrderPosition> PozycjeZamowienia { get; set; }
    }

    public class ZamowienieZrealizowaneEmail : Email
    {
        public string To { get; set; }
        public string From { get; set; }
        public int NumerZamowienia { get; set; }
    }
}