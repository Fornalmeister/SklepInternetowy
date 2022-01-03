using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SklepInternetowy.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;


namespace SklepInternetowy.DAL
{
    public class KursyInitializer : DropCreateDatabaseIfModelChanges<KursyContext>
    {
        public static void SeedKursyData(KursyContext context)
            {
                var category = new List<Category>
            {
                new Category() { CategoryId=1, NameCategory="Asp.Net", NameImg="aspnet.png", DescriptionCategory="programowanie w asp net" },
                new Category() { CategoryId=2, NameCategory="JavaScript", NameImg="javascript.png", DescriptionCategory="skryptowy język programowania" },
                new Category() { CategoryId=3, NameCategory="jQuery", NameImg="jquery.png", DescriptionCategory="lekka biblioteka programistyczna dla języka JavaScript" },
                new Category() { CategoryId=4, NameCategory="Html5", NameImg="html.png", DescriptionCategory="język wykorzystywany do tworzenia i prezentowania stron internetowych www" },
                new Category() { CategoryId=5, NameCategory="Css3", NameImg="css.png", DescriptionCategory="język służący do opisu formy prezentacji (wyświetlania) stron www" },
                new Category() { CategoryId=6, NameCategory="Xml", NameImg="xml.png", DescriptionCategory="uniwersalny język znaczników przeznaczony do reprezentowania różnych danych w strukturalizowany sposób" },
                new Category() { CategoryId=7, NameCategory="C#", NameImg="csharp.png", DescriptionCategory="obiektowy język programowania zaprojektowany dla platformy .Net" }
            };

                category.ForEach(c => context.Category.AddOrUpdate(c));
                context.SaveChanges();

                var course = new List<Kurs>
            {
                new Kurs() { KursId=1, Author="Mariusz", Title="Asp.Net", CategoryId=1, Price=0, Bestseller=true, NameImg="obrazekaspnet.png",
                DateAdded = DateTime.Now, Description="Kurs ASP.NET - doskonała platforma do tworzenia dynamicznych aplikacji internetowych. Kurs jest przeznaczony dla wszystkich osób, które chcą nauczyć się od podstaw tworzenia stron internetowych wykorzystując technologię ASP-NET."},
                new Kurs() { KursId=2, Author="Mariusz", Title="Asp.Net Mvc", CategoryId=1, Price=0, Bestseller=true, NameImg="obrazekmvc.png",
                DateAdded = DateTime.Now, Description="Kurs ASP.NET MVC - przeznaczony jest dla wszystkich osób, które chcą nauczyć się od podstaw tworzenia stron internetowych wykorzystując technologię ASP-NET MVC."},
                new Kurs() { KursId=3, Author="Mariusz", Title="Asp.Net Mvc - Sklep Internetowy", CategoryId=1, Price=100, Bestseller=true, NameImg="obrazekmvc2.png",
                DateAdded = DateTime.Now, Description="Kurs Asp.Net Mvc - Sklep Internetowy - to praktyczne rozwiązanie wykorzystujące technologię Asp.Net Mvc pokazujące krok po kroku budowanie serwisu internetowego sprzedającego kursy on-line"},

                new Kurs() { KursId=4, Author="Mariusz", Title="JavaScript", CategoryId=2, Price=70, Bestseller=false, NameImg="obrazekjavascript.png",
                DateAdded = DateTime.Now, Description="Kurs JavaScript - skryptowy język programowania"},
                new Kurs() { KursId=5, Author="Mariusz", Title="jQuery", CategoryId=3, Price=90, Bestseller=true, NameImg="obrazekjquery.png",
                DateAdded = DateTime.Now, Description="Kurs jQuery - lekka biblioteka programistyczna dla języka JavaScript"},
                new Kurs() { KursId=6, Author="Mariusz", Title="Html5", CategoryId=4, Price=70, Bestseller=false, NameImg="obrazekhtml.png",
                DateAdded = DateTime.Now, Description="Kurs Html5 - język wykorzystywany do tworzenia i prezentowania stron internetowych www"},

                new Kurs() { KursId=7, Author="Mariusz", Title="Css3", CategoryId=5, Price=70, Bestseller=false, NameImg="obrazekcss.png",
                DateAdded = DateTime.Now, Description="Kurs Css3 - język służący do opisu formy prezentacji (wyświetlania) stron www"},
                new Kurs() { KursId=8, Author="Mariusz", Title="Xml", CategoryId=6, Price=60, Bestseller=false, NameImg="obrazekxml.png",
                DateAdded = DateTime.Now, Description="Kurs Xml - uniwersalny język znaczników przeznaczony do reprezentowania różnych danych w strukturalizowany sposób"},
                new Kurs() { KursId=9, Author="Mariusz", Title="C#", CategoryId=7, Price=90, Bestseller=true, NameImg="obrazekcsharp.png",
                DateAdded = DateTime.Now, Description="Kurs C# - obiektowy język programowania zaprojektowany dla platformy .Net"}

            };

                course.ForEach(c => context.Kursy.AddOrUpdate(c));
                context.SaveChanges();
        
        }


        public static void SeedUzytkownicy(KursyContext db)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

            const string name = "admin@praktycznekursy.pl";
            const string password = "P@ssw0rd";
            const string roleName = "Admin";

            var user = userManager.FindByName(name);
            if (user == null)
            {
                user = new ApplicationUser { UserName = name, Email = name, DaneUzytkownika = new DaneUzytkownika() };
                var result = userManager.Create(user, password);
            }

            // utworzenie roli Admin jeśli nie istnieje 
            var role = roleManager.FindByName(roleName);
            if (role == null)
            {
                role = new IdentityRole(roleName);
                var roleresult = roleManager.Create(role);
            }

            // dodanie uzytkownika do roli Admin jesli juz nie jest w roli
            var rolesForUser = userManager.GetRoles(user.Id);
            if (!rolesForUser.Contains(role.Name))
            {
                var result = userManager.AddToRole(user.Id, role.Name);
            }
        }

    }
}