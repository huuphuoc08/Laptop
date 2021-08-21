using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Laptopp.Models;

namespace Laptopp.Controllers
{
    public class SearchController : Controller
    {
        dbLaptoppDataContext db = new dbLaptoppDataContext();
        public ActionResult Search(string strSearch)
        {

            ViewBag.Search = strSearch;
            if (!string.IsNullOrEmpty(strSearch))
            {

                var kq = from x in db.LAPs where x.TenLap.Contains(strSearch) select x;
                return View(kq);
            }
            return View();
        }
        public ActionResult Group()
        {
            ///
            var kq = db.LAPs.GroupBy(x => x.MaLap);
            return View(kq);
        }
        public ActionResult ThongKe()
        {
            var kq = from x in db.LAPs
                     group x by x.MaLap into g
                     select new ReportInfo
                     {
                         Id = g.Key.ToString(),
                         Count = g.Count(),
                         Sum = g.Sum(n => n.SoLuongBan),
                         Max = g.Max(n => n.SoLuongBan),
                         Min = g.Min(n => n.SoLuongBan),
                         Avg = Convert.ToDecimal(g.Average(n => n.SoLuongBan))
                     };
            return View(kq);
        }
    }
}