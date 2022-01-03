using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SklepInternetowy.App_Start;
using SklepInternetowy.DAL;
using SklepInternetowy.Infrastructure;
using SklepInternetowy.Models;
using SklepInternetowy.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SklepInternetowy.Controllers
{
    public class KoszykController : Controller
    {

        private KoszykMenager koszykMenager;
        private ISessionMenager sessionMenager { get; set; }
        private KursyContext db = new KursyContext();

        public KoszykController()
        {
            db = new KursyContext();
            sessionMenager = new SessionMenager();
            koszykMenager = new KoszykMenager(sessionMenager, db);
        }


        // GET: Koszyk
        public ActionResult Index()
        {
            var pozycjeKoszyka = koszykMenager.PobierzKoszyk();
            var cenaCalkowita = koszykMenager.PobierzWartoscKoszyka();

            KoszykViewModel koszykVM = new KoszykViewModel()
            {
                PozycjaKoszyka = pozycjeKoszyka,
                CenaCalkowita = cenaCalkowita
            };

            return View(koszykVM);

        
        }

        public ActionResult DodajDoKoszyka(int id)
        {
            koszykMenager.DodajDoKoszyka(id);

            return RedirectToAction("Index");
        }

        public int PobierzIloscElementowKoszyka()
        {
           return koszykMenager.PobierzIloscPozycjiKoszyka();
        }

        public ActionResult UsunZKoszyka(int kursId)
        {
            int iloscPozycji = koszykMenager.UsunZKoszyka(kursId);
            int iloscPozycjiKoszyka = koszykMenager.PobierzIloscPozycjiKoszyka();
            decimal wartoscKoszyka = koszykMenager.PobierzWartoscKoszyka();

            var wynik = new KoszykUsuwanieViewModel
            {
                IdPozycjiUsuwanej = kursId,
                IloscPozycjiUsuwanej = iloscPozycji,
                KoszykCenaCalkowita = wartoscKoszyka,
                KoszykIloscPozycji = iloscPozycjiKoszyka
            };

            return Json(wynik);
        }
        public async Task<ActionResult> Zaplac()
        {
            var name = User.Identity.Name;
            //logger.Info("Strona koszyk | zaplac | " + name);

            if (Request.IsAuthenticated)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

                var zamowienie = new Order
                {
                    FirstName = user.DaneUzytkownika.Imie,
                    LastName = user.DaneUzytkownika.Nazwisko,
                    Street = user.DaneUzytkownika.Adres,
                    City = user.DaneUzytkownika.Miasto,
                    Postmal = user.DaneUzytkownika.KodPocztowy,
                    Email = user.DaneUzytkownika.Email,
                    Phone = user.DaneUzytkownika.Telefon
                };
                return View(zamowienie);
            }
            else
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Zaplac", "Koszyk") });
        }

        [HttpPost]
        public async Task<ActionResult> Zaplac(Order zamowienieSzczegoly)
        {
            if (ModelState.IsValid)
            {
                // pobieramy id uzytkownika aktualnie zalogowanego
                var userId = User.Identity.GetUserId();

                // utworzenie obiektu zamowienia na podstawie tego co mamy w koszyku
                var newOrder = koszykMenager.UtworzZamowienie(zamowienieSzczegoly, userId);

                // szczegóły użytkownika - aktualizacja danych 
                var user = await UserManager.FindByIdAsync(userId);
                TryUpdateModel(user.DaneUzytkownika);
                await UserManager.UpdateAsync(user);

                // opróżnimy nasz koszyk zakupów
                koszykMenager.PustyKoszyk();

              //  maileService.WyslaniePotwierdzenieZamowieniaEmail(newOrder);

                return RedirectToAction("PotwierdzenieZamowienia");
            }
            else
                return View(zamowienieSzczegoly);
        }

        public ActionResult PotwierdzenieZamowienia()
        {
            var name = User.Identity.Name;
            //logger.Info("Strona koszyk | potwierdzenie | " + name);
            return View();
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

    }
}