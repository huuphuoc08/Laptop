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
    public class ThuongHieuController : Controller
    {
        dbLaptoppDataContext db = new dbLaptoppDataContext();

        // GET: Admin/ThuongHieu
        public ActionResult Index(int? page)
        {
            int iPageNum = (page ?? 1);
            int iPageeSize = 7;
            return View(db.THUONGHIEUs.ToList().OrderBy(n => n.MaTH).ToPagedList(iPageNum, iPageeSize));
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(THUONGHIEU thuonghieu, FormCollection f)
        {
            var sTenThuongHieu = f["TenThuongHieu"];
            if (String.IsNullOrEmpty(sTenThuongHieu))
            {
                ViewData["err1"] = "Thương hiệu không được rỗng";
            }
            else
            {
                thuonghieu.TenThuongHieu = sTenThuongHieu;
                db.THUONGHIEUs.InsertOnSubmit(thuonghieu);
                db.SubmitChanges();
                return RedirectToAction("Index");
            }

            return View();
        }



        [HttpGet]
        public ActionResult Delete(int id)
        {
            var thuonghieu= db.THUONGHIEUs.SingleOrDefault(n => n.MaTH == id);
            if (thuonghieu == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(thuonghieu);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id, FormCollection f)
        {
            var thuonghieu= db.THUONGHIEUs.SingleOrDefault(n => n.MaTH == id);

            if (thuonghieu == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            db.THUONGHIEUs.DeleteOnSubmit(thuonghieu);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var thuonghieu= db.THUONGHIEUs.SingleOrDefault(n => n.MaTH == id);
            if (thuonghieu == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Hiển thị danh sách chủ đề và nhà xuất bản đồng thời chọn chủ đề và nhà xuất bản của cuốn hiện tại
            ViewBag.MaTH = new SelectList(db.THUONGHIEUs.ToList().OrderBy(n => n.TenThuongHieu), "MaTH", "TenThuongHieu", thuonghieu.MaTH);

            return View(thuonghieu);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection f, HttpPostedFileBase fFileUpload)
        {
            var thuonghieu= db.THUONGHIEUs.SingleOrDefault(n => n.MaTH == int.Parse(f["iMaTH"]));
            
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


                }
                

                db.SubmitChanges();
     
                return RedirectToAction("Index");
            }
            return View(thuonghieu);
        }
    }
}