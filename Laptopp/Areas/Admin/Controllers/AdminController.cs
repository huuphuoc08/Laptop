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
    public class AdminController : Controller
    {
        dbLaptoppDataContext db = new dbLaptoppDataContext();
        // GET: Admin/Admin
        public ActionResult Index()
        {
            return View();
        }
    }
}