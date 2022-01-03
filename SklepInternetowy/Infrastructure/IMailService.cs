using SklepInternetowy.Models;

namespace SklepInternetowy.Infrastructure
{
    public interface IMailService
    {
        void WyslaniePotwierdzenieZamowieniaEmail(Order zamowienie);
        void WyslanieZamowienieZrealizowaneEmail(Order zamowienie);
    }
}