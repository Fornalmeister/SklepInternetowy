using SklepInternetowy.Models;
using SklepInternetowy.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SklepInternetowy.Infrastructure
{
    public class PostalMailService : IMailService
    {
        public void WyslaniePotwierdzenieZamowieniaEmail(Order zamowienie)
        {
            PotwierdzenieZamowieniaEmail email = new PotwierdzenieZamowieniaEmail();
            email.To = zamowienie.Email;
            email.From = "szymekfornal6@gmail.com";
            email.Wartosc = zamowienie.Value;
            email.NumerZamowienia = zamowienie.OrderId;
            email.PozycjeZamowienia = zamowienie.OrderPostion;
            email.sciezkaObrazka = AppConfig.ObrazkiFolderWzgledny;
            email.Send();
        }

        public void WyslanieZamowienieZrealizowaneEmail(Order zamowienie)
        {
            ZamowienieZrealizowaneEmail email = new ZamowienieZrealizowaneEmail();
            email.To = zamowienie.Email;
            email.From = "szymekfornal6@gmail.com";
            email.NumerZamowienia = zamowienie.OrderId;
            email.Send();
        }
    }
}