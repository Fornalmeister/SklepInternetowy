using MvcSiteMapProvider.Caching;
using NLog;
using SklepInternetowy.DAL;
using SklepInternetowy.Infrastructure;
using SklepInternetowy.Models;
using SklepInternetowy.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SklepInternetowy.Controllers
{
    public class HomeController : Controller
    {
        private KursyContext db = new KursyContext();
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        public ActionResult Index()
        {
            Logger.Info("Jesteś na stronie głównej");
            ICacheProvider cache = new DefaultCacheProvider();

            List<Category> kategorie;

            if (cache.IsSet(Consts.KategorieCacheKey))
            {
                kategorie = cache.Get(Consts.KategorieCacheKey) as List<Category>;
            }
            else
            {
                kategorie = db.Category.ToList();
                cache.Set(Consts.KategorieCacheKey, kategorie, 60);
            }

            List<Kurs> nowosci;

            if(cache.IsSet(Consts.NowosciCacheKey))
            {
                nowosci = cache.Get(Consts.NowosciCacheKey) as List<Kurs>;
            }
            else
            {
                nowosci = db.Kursy.Where(a => !a.Hidden).OrderByDescending(a => a.DateAdded).Take(3).ToList();
                cache.Set(Consts.NowosciCacheKey, nowosci, 60);
            }

            List<Kurs> bestseller;

            if (cache.IsSet(Consts.BestsellerCacheKey))
            {
                bestseller = cache.Get(Consts.BestsellerCacheKey) as List<Kurs>;
            }
            else
            {
                bestseller = db.Kursy.Where(a => !a.Hidden && a.Bestseller).OrderBy(a => Guid.NewGuid()).Take(3).ToList();
                cache.Set(Consts.BestsellerCacheKey, bestseller, 60);
            }


            var vm = new HomeViewModel()
            {
                Kategorie = kategorie,
                Nowosci = nowosci,
                Bestsellery = bestseller
            };

            return View(vm);
        }

        public ActionResult StronyStatyczne(string nazwa)
        {
            // var name = User.Identity.Name;
            // logger.Info("Strona " + nazwa + " | " + name);
            return View(nazwa);
        }


    }
}