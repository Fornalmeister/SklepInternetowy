using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using NLog;
using SklepInternetowy.App_Start;
using SklepInternetowy.DAL;
using SklepInternetowy.Infrastructure;
using SklepInternetowy.Models;
using SklepInternetowy.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SklepInternetowy.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private KursyContext db;
        private IMailService mailService;

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            Error
        }

        public ManageController(KursyContext context, IMailService mailService)
        {
            this.db = context;
            this.mailService = mailService;
        }

        public ManageController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
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

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        // GET: Manage
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            var name = User.Identity.Name;
            logger.Info("Admin główna | " + name);

            if (TempData["ViewData"] != null)
            {
                ViewData = (ViewDataDictionary)TempData["ViewData"];
            }

            if (User.IsInRole("Admin"))
                ViewBag.UserIsAdmin = true;
            else
                ViewBag.UserIsAdmin = false;

            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }

            var model = new ManageCredentialsViewModel
            {
                Message = message,
                DaneUzytkownika = user.DaneUzytkownika
            };

            return View(model);
        }

        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeProfile([Bind(Prefix = "DaneUzytkownika")] DaneUzytkownika daneUzytkownika)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                user.DaneUzytkownika = daneUzytkownika;
                var result = await UserManager.UpdateAsync(user);

                AddErrors(result);
            }

            if (!ModelState.IsValid)
            {
                TempData["ViewData"] = ViewData;
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword([Bind(Prefix = "ChangePasswordViewModel")] ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ViewData"] = ViewData;
                return RedirectToAction("Index");
            }

            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInAsync(user, isPersistent: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);

            if (!ModelState.IsValid)
            {
                TempData["ViewData"] = ViewData;
                return RedirectToAction("Index");
            }

            var message = ManageMessageId.ChangePasswordSuccess;
            return RedirectToAction("Index", new { Message = message });
        }


        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("password-error", error);
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie, DefaultAuthenticationTypes.TwoFactorCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = isPersistent }, await user.GenerateUserIdentityAsync(UserManager));
        }

        public ActionResult ListaZamowien()
        {
            var name = User.Identity.Name;
            logger.Info("Admin zamowienia | " + name);

            bool isAdmin = User.IsInRole("Admin");
            ViewBag.UserIsAdmin = isAdmin;

            IEnumerable<Order> zamowieniaUzytkownika;

            // Dla administratora zwracamy wszystkie zamowienia
            if (isAdmin)
            {
                zamowieniaUzytkownika = db.Order.Include("PozycjeZamowienia").OrderByDescending(o => o.DateAdd).ToArray();
            }
            else
            {
                var userId = User.Identity.GetUserId();
                zamowieniaUzytkownika = db.Order.Where(o => o.UserId == userId).Include("PozycjeZamowienia").OrderByDescending(o => o.DateAdd).ToArray();
            }

            return View(zamowieniaUzytkownika);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public OrderStatus ZmianaStanuZamowienia(Order zamowienie)
        {
            Order zamowienieDoModyfikacji = db.Order.Find(zamowienie.OrderId);
            zamowienieDoModyfikacji.OrderStatus = zamowienie.OrderStatus;
            db.SaveChanges();

            if (zamowienieDoModyfikacji.OrderStatus == OrderStatus.Zrealizowane)
            {
                this.mailService.WyslanieZamowienieZrealizowaneEmail(zamowienieDoModyfikacji);
            }
            return zamowienie.OrderStatus;
        }

        [Authorize(Roles = "Admin")]
        public ActionResult DodajKurs(int? kursId, bool? potwierdzenie)
        {
            Kurs kurs;
            if (kursId.HasValue)
            {
                ViewBag.EditMode = true;
                kurs = db.Kursy.Find(kursId);
            }
            else
            {
                ViewBag.EditMode = false;
                kurs = new Kurs();
            }

            var result = new EditKursViewModel();
            result.Category = db.Category.ToList();
            result.Kurs = kurs;
            result.Potwierdzenie = potwierdzenie;

            return View(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult DodajKurs(EditKursViewModel model, HttpPostedFileBase file)
        {
            if (model.Kurs.KursId > 0)
            {
                // modyfikacja kursu
                db.Entry(model.Kurs).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DodajKurs", new { potwierdzenie = true });
            }
            else
            {
                // Sprawdzenie, czy użytkownik wybrał plik
                if (file != null && file.ContentLength > 0)
                {
                    if (ModelState.IsValid)
                    {
                        // Generowanie pliku
                        var fileExt = Path.GetExtension(file.FileName);
                        var filename = Guid.NewGuid() + fileExt;

                        var path = Path.Combine(Server.MapPath(AppConfig.ObrazkiFolderWzgledny), filename);
                        file.SaveAs(path);

                        model.Kurs.NameImg = filename;
                        model.Kurs.DateAdded = DateTime.Now;

                        db.Entry(model.Kurs).State = EntityState.Added;
                        db.SaveChanges();

                        return RedirectToAction("DodajKurs", new { potwierdzenie = true });
                    }
                    else
                    {
                        var kategorie = db.Category.ToList();
                        model.Category = kategorie;
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Nie wskazano pliku");
                    var kategorie = db.Category.ToList();
                    model.Category = kategorie;
                    return View(model);
                }
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult UkryjKurs(int kursId)
        {
            var kurs = db.Kursy.Find(kursId);
            kurs.Hidden = true;
            db.SaveChanges();

            return RedirectToAction("DodajKurs", new { potwierdzenie = true });
        }

        [Authorize(Roles = "Admin")]
        public ActionResult PokazKurs(int kursId)
        {
            var kurs = db.Kursy.Find(kursId);
            kurs.Hidden = false;
            db.SaveChanges();

            return RedirectToAction("DodajKurs", new { potwierdzenie = true });
        }

        [AllowAnonymous]
        public ActionResult WyslaniePotwierdzenieZamowieniaEmail(int zamowienieId, string nazwisko)
        {
            var zamowienie = db.Order.Include("PozycjeZamowienia").Include("PozycjeZamowienia.Kurs")
                               .SingleOrDefault(o => o.OrderId == zamowienieId && o.LastName == nazwisko);

            if (zamowienie == null) return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            PotwierdzenieZamowieniaEmail email = new PotwierdzenieZamowieniaEmail();
            email.To = zamowienie.Email;
            email.From = "szymekfornal6@gmail.com";
            email.Wartosc = zamowienie.Value;
            email.NumerZamowienia = zamowienie.OrderId;
            email.PozycjeZamowienia = zamowienie.OrderPostion;
            email.sciezkaObrazka = AppConfig.ObrazkiFolderWzgledny;
            email.Send();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [AllowAnonymous]
        public ActionResult WyslanieZamowienieZrealizowaneEmail(int zamowienieId, string nazwisko)
        {
            var zamowienie = db.Order.Include("PozycjeZamowienia").Include("PozycjeZamowienia.Kurs")
                                  .SingleOrDefault(o => o.OrderId == zamowienieId && o.LastName == nazwisko);

            if (zamowienie == null) return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            ZamowienieZrealizowaneEmail email = new ZamowienieZrealizowaneEmail();
            email.To = zamowienie.Email;
            email.From = "szymekfornal6@gmail.com";
            email.NumerZamowienia = zamowienie.OrderId;
            email.Send();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}