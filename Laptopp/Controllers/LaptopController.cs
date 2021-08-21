using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Laptopp.Models;
using PagedList;
using PagedList.Mvc;

namespace Laptopp.Controllers
{
    public class LaptopController : Controller
    {
        dbLaptoppDataContext data = new dbLaptoppDataContext();
        private List<LAP> LayLapMoi(int count)
        {
            return data.LAPs.OrderByDescending(a => a.NgayCapNhat).Take(count).ToList();
        }
        // GET: Laptop
        public ActionResult Index()
        {
            var listLapMoi = data.LAPs.OrderByDescending(a => a.NgayCapNhat).Take(6).ToList();
            return View(listLapMoi);
        }
        public ActionResult ThuongHieuPartial()
        {
            var listThuongHieu = from th in data.THUONGHIEUs select th;
            return PartialView(listThuongHieu);
        }
        public ActionResult PhuKienPartial()
        {
            var listPhuKien = from pk in data.PHUKIENs select pk;
            return PartialView(listPhuKien);
        }
        public ActionResult LapBanNhieuPartial()
        {
            var listLapBanNhieu = data.LAPs.OrderByDescending(a => a.NgayCapNhat).Take(6).ToList();
            return PartialView(listLapBanNhieu);
        }
        public ActionResult LapTheoThuongHieu (int iMaTH,int ? page)
        {
            ViewBag.MaTH = iMaTH;
            int iSize = 3;
            int iPageNum = (page ?? 1);
            var lap = from l in data.LAPs where l.MaTH == iMaTH select l;
            return View(lap.ToPagedList(iPageNum,iSize));
        }
        public ActionResult PhuKien(int id)
        {
            var phukien = from s in data.PHUKIENs where s.MaPK == id select s;
            return View(phukien);
        }
        public ActionResult _NavbarPartial()
        {
            var listThuongHieu = from cd in data.THUONGHIEUs select cd;
            return PartialView(listThuongHieu);
        }
        public ActionResult NavbarPhuKien()
        {
            var listPK = from pk in data.PHUKIENs select pk;
            return PartialView(listPK);
        }
        public ActionResult ChiTietLap(int id)
        {
            var lap = from s in data.LAPs where s.MaLap == id select s;
            return View(lap.Single());
        }
        public ActionResult LoginLogout()
        {
            return PartialView("LoginLogoutPartial");
        }
        public ActionResult GioiThieu()
        {
            return View();
        }
    }
}