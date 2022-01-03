using MvcSiteMapProvider;
using SklepInternetowy.DAL;
using SklepInternetowy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SklepInternetowy.Infrastructure
{
    public class KursySzczegolyDynamicNodeProvider : DynamicNodeProviderBase
    {
        private KursyContext db = new KursyContext();

        public override IEnumerable<DynamicNode> GetDynamicNodeCollection(ISiteMapNode nodee)
        {
            var returnValue = new List<DynamicNode>();

            foreach(Kurs kurs in db.Kursy)
            {
                DynamicNode node = new DynamicNode();
                node.Title = kurs.Title;
                node.Key = "Kurs_" + kurs.KursId;
                node.ParentKey = "Kategoria_" + kurs.CategoryId;
                node.RouteValues.Add("id", kurs.KursId);
                returnValue.Add(node);
            }

            return returnValue;
        }

    }
}