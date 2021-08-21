using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Laptopp.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace Laptopp.Areas.Admin.Views.Admin
{
    public class LapController : Controller
    {
        dbLaptoppDataContext db = new dbLaptoppDataContext();
        // GET: Admin/Lap
        public ActionResult Index(int? page)
        {
            int iPageNum = (page ?? 1);
            int iPageSize = 7;
            return View(db.LAPs.ToList().OrderBy(n => n.MaLap).ToPagedList(iPageNum, iPageSize));
        }
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.MaTH = new SelectList(db.THUONGHIEUs.ToList().OrderBy(n => n.TenThuongHieu), "MaTH", "TenThuongHieu");
            //ViewBag.MaNXB = new SelectList(db.PHUKIENs.ToList().OrderBy(n => n.TenPK), "MaPK", "TenPK");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(LAP lap, FormCollection f, HttpPostedFileBase fFileUpload)
        {
            ViewBag.MaTH = new SelectList(db.THUONGHIEUs.ToList().OrderBy(n => n.TenThuongHieu), "MaTH", "TenThuongHieu");
            //ViewBag.MaPK = new SelectList(db.PHUKIENs.ToList().OrderBy(n => n.TenPK), "MaPK", "TenPK");

            if (fFileUpload == null)
            {
                ViewBag.ThongBao = "Hãy chọn ảnh bìa.";
                ViewBag.TenLap = f["lTenLap"];
                ViewBag.MoTa = f["lMoTa"];
                ViewBag.SoLuong = int.Parse(f["iSoLuong"]);
                ViewBag.GiaBan = decimal.Parse(f["mGiaBan"]);
                ViewBag.MaTH = new SelectList(db.THUONGHIEUs.ToList().OrderBy(n => n.TenThuongHieu), "MaTH", "TenThuongHieu", int.Parse(f["MaTH"]));
                //ViewBag.MaPK = new SelectList(db.PHUKIENs.ToList().OrderBy(n => n.TenPK), "MaPK", "TenPK", int.Parse(f["MaPK"]));
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var sFileName = Path.GetFileName(fFileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Images"), sFileName);
                    if (!System.IO.File.Exists(path))
                    {
                        fFileUpload.SaveAs(path);
                    }
                    lap.TenLap = f["lTenLap"];
                    lap.MoTa = f["lMoTa"].Replace("<p>", "").Replace("</p>", "\n");
                    lap.AnhBia = sFileName;
                    lap.NgayCapNhat = Convert.ToDateTime(f["dNgayCapNhat"]);
                    lap.SoLuongBan = int.Parse(f["iSoLuong"]);
                    lap.GiaBan = decimal.Parse(f["mGiaBan"]);
                    lap.MaTH = int.Parse(f["MaTH"]);
                    //lap.MaPK = int.Parse(f["MaPK"]);
                    db.LAPs.InsertOnSubmit(lap);
                    db.SubmitChanges();
                    return RedirectToAction("Index");

                }
                return View();
            }

        }
        public ActionResult Details(int id)
        {
            var lap = db.LAPs.SingleOrDefault(n => n.MaLap == id);
            if (lap == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(lap);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var lap = db.LAPs.SingleOrDefault(n => n.MaLap == id);
            if (lap == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(lap);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id, FormCollection f)
        {
            var lap = db.LAPs.SingleOrDefault(n => n.MaLap == id);

            if (lap == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            var ctdh = db.CHITIETDATHANGs.Where(ct => ct.MaLap == id);
            if (ctdh.Count() > 0)
            {
                
                ViewBag.ThongBao = "Sản phẩm này đang có trong bảng Chi tiết đặt hàng <br>" + " Nếu muốn xóa thì phải xóa hết mã sản phẩm này trong bảng Chi tiết đặt hàng";
                return View(lap);
            }
            db.LAPs.DeleteOnSubmit(lap);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var lap = db.LAPs.SingleOrDefault(n => n.MaLap == id);
            if (lap == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaTH = new SelectList(db.THUONGHIEUs.ToList().OrderBy(n => n.TenThuongHieu), "MaTH", "TenThuongHieu", lap.MaTH);
            return View(lap);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection f, HttpPostedFileBase fFileUpload)
        {
            var lap = db.LAPs.SingleOrDefault(n => n.MaLap == int.Parse(f["iMalap"]));
            ViewBag.MaTH = new SelectList(db.THUONGHIEUs.ToList().OrderBy(n => n.TenThuongHieu), "MaTH", "TenThuongHieu", lap.MaTH);
            if (ModelState.IsValid)
            {
                if (fFileUpload != null) //Kiểm tra để xác nhận cho thay đổi ảnh bìa
                {
                    //Lấy tên file (Khai báo thư viện: System.IO)
                    var sFileName = Path.GetFileName(fFileUpload.FileName);
                    //Lấy đường dẫn lưu file
                    var path = Path.Combine(Server.MapPath("~/Images"), sFileName);
                    //Kiểm tra file đã tồn tại chưa
                    if (!System.IO.File.Exists(path))
                    {
                        fFileUpload.SaveAs(path);
                    }

                    lap.AnhBia = sFileName;
                }
                //Lưu lap vào CSDL
                lap.TenLap = f["sTenLap"];
                lap.MoTa = f["sMoTa"].Replace("<p>", "").Replace("</p>", "\n");

                lap.NgayCapNhat = Convert.ToDateTime(f["dNgayCapNhat"]);
                lap.SoLuongBan = int.Parse(f["iSoLuong"]);
                lap.GiaBan = decimal.Parse(f["mGiaBan"]);
                lap.MaTH = int.Parse(f["MaTH"]);
                db.SubmitChanges();
                //Về lại trang Quản lý sách
                return RedirectToAction("Index");
            }
            return View(lap);
        }


    }
}