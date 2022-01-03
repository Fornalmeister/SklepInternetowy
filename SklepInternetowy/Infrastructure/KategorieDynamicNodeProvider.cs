using MvcSiteMapProvider;
using SklepInternetowy.DAL;
using SklepInternetowy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SklepInternetowy.Infrastructure
{
    public class KategorieDynamicNodeProvider : DynamicNodeProviderBase
    {
        private KursyContext db = new KursyContext();
        public override IEnumerable<DynamicNode> GetDynamicNodeCollection(ISiteMapNode nodee)
        {
            var returnValue = new List<DynamicNode>();

            foreach (Category kategoria in db.Category)
            {
                DynamicNode node = new DynamicNode();
                node.Title = kategoria.NameCategory;
                node.Key = "Kategoria_" + kategoria.CategoryId;
                node.RouteValues.Add("nazwaKategori", kategoria.NameCategory);
                returnValue.Add(node);
            }
            return returnValue;
        }
    }
}