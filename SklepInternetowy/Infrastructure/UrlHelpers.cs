using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SklepInternetowy.Infrastructure
{
    public static class UrlHelpers
    {
        public static string IkonyKategoriiSciezka(this UrlHelper helper, string nazwaIkonyKategorii)
        {
            var IkonyKategoriFolder = AppConfig.IkonyKategoriFolderWzgledny;
            var sciezka = Path.Combine(IkonyKategoriFolder, nazwaIkonyKategorii);
            var sciezkaBezwzgledna = helper.Content(sciezka);

            return sciezkaBezwzgledna;
        }

        public static string ObrazkiSciezka(this UrlHelper helper, string nazwaObrazka) //metoda rozszerzajaca musi byc static
        {
            var ObrazkiFolder = AppConfig.ObrazkiFolderWzgledny;
            var sciezka = Path.Combine(ObrazkiFolder, nazwaObrazka);
            var sciezkaBezwzgledna = helper.Content(sciezka);

            return sciezkaBezwzgledna;
        }

    }
}