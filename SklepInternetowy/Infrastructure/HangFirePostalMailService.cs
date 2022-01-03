using Hangfire;
using SklepInternetowy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SklepInternetowy.Infrastructure
{
    public class HangFirePostalMailService : IMailService
    {
        public void WyslaniePotwierdzenieZamowieniaEmail(Order zamowienie)
        {
            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            string url = urlHelper.Action("WyslaniePotwierdzenieZamowieniaEmail", "Manage", new { zamowienieId = zamowienie.OrderId, nazwisko = zamowienie.LastName }, HttpContext.Current.Request.Url.Scheme);

            BackgroundJob.Enqueue(() => Helpers.CallUrl(url));
        }

        public void WyslanieZamowienieZrealizowaneEmail(Order zamowienie)
        {
            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            string url = urlHelper.Action("WyslanieZamowienieZrealizowaneEmail", "Manage", new { zamowienieId = zamowienie.OrderId, nazwisko = zamowienie.LastName }, HttpContext.Current.Request.Url.Scheme);

            BackgroundJob.Enqueue(() => Helpers.CallUrl(url));
        }
    }
}