using SklepInternetowy.DAL;
using SklepInternetowy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SklepInternetowy.Infrastructure
{
    public class KoszykMenager
    {
        private KursyContext db;
        private ISessionMenager session;
        public KoszykMenager(ISessionMenager session, KursyContext db)
        {
            this.session = session; //this czyli nazwa naszej klasy
            this.db = db;
        }

        public List<PozycjaKoszyka> PobierzKoszyk()
        {

            List<PozycjaKoszyka> koszyk;

            if (session.Get<List<PozycjaKoszyka>>(Consts.KoszykSessionKlucz)==null)
            {
                koszyk = new List<PozycjaKoszyka>();
            }
            else
            {
                koszyk = session.Get<List<PozycjaKoszyka>>(Consts.KoszykSessionKlucz) as List<PozycjaKoszyka>;
            }

            return koszyk;
        }

        public void DodajDoKoszyka(int kursId)
        {
            var koszyk = PobierzKoszyk();
            var pozycjaKoszyka = koszyk.Find(k => k.Kurs.KursId == kursId);

            if (pozycjaKoszyka != null)
            {
                pozycjaKoszyka.Ilosc++;
            }
            else
            {
                var kursDoDodania = db.Kursy.Where(k => k.KursId == kursId).SingleOrDefault();
                if(kursDoDodania != null)
                {
                    var nowaPozycjaKoszyka = new PozycjaKoszyka()
                    {
                        Kurs = kursDoDodania,
                        Ilosc = 1,
                        Wartosc = kursDoDodania.Price
                    };
                    koszyk.Add(nowaPozycjaKoszyka);
                }
            }
            session.Set(Consts.KoszykSessionKlucz, koszyk);
        }
        public int UsunZKoszyka(int kursId)
        {
            var koszyk = PobierzKoszyk();
            var pozycjaKoszyka = koszyk.Find(k => k.Kurs.KursId == kursId);

            if(pozycjaKoszyka != null)
            {
                if(pozycjaKoszyka.Ilosc > 1)
                {
                    pozycjaKoszyka.Ilosc--;
                    return pozycjaKoszyka.Ilosc;
                }
                else
                {
                    koszyk.Remove(pozycjaKoszyka);
                }
            }
            return 0;
        }

        public decimal PobierzWartoscKoszyka()
        {
            var koszyk = PobierzKoszyk();
            return koszyk.Sum(k => (k.Ilosc * k.Kurs.Price));
        }

        public int PobierzIloscPozycjiKoszyka()
        {
            var koszyk = PobierzKoszyk();
            int ilosc = koszyk.Sum(k => k.Ilosc);
            return ilosc;
        }

        public Order UtworzZamowienie(Order noweZamowienie, string userId)
        {
            var koszyk = PobierzKoszyk();
            noweZamowienie.DateAdd = DateTime.Now;
            noweZamowienie.UserId = userId;

            db.Order.Add(noweZamowienie);

            if (noweZamowienie.OrderPostion == null)
                noweZamowienie.OrderPostion = new List<OrderPosition>();

            decimal koszykWartosc = 0;

            foreach (var koszykElement in koszyk)
            {
                var nowaPozycjaZamowienia = new OrderPosition()
                {
                    KursId = koszykElement.Kurs.KursId,
                    quantity = koszykElement.Ilosc,
                    OrderCost = koszykElement.Kurs.Price
                };

                koszykWartosc += (koszykElement.Ilosc * koszykElement.Kurs.Price);
                noweZamowienie.OrderPostion.Add(nowaPozycjaZamowienia);
            }

            noweZamowienie.Value = koszykWartosc;
            db.SaveChanges();

            return noweZamowienie;
        }

        public void PustyKoszyk()
        {
            session.Set<List<OrderPosition>>(Consts.KoszykSessionKlucz, null);
        }


    }
}