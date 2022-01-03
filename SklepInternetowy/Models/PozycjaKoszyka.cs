using SklepInternetowy.Models;

namespace SklepInternetowy.Infrastructure
{
    public class PozycjaKoszyka
    {
        public Kurs Kurs { get; set; }
        public int Ilosc { get; set; }
        public decimal Wartosc { get; set; }
        
    }
}