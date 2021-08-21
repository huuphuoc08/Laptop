using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Laptopp.Models
{
    public class GioHang
    {
        dbLaptoppDataContext db = new dbLaptoppDataContext();

        public int iMaLap { get; set; }
        public string sTenLap { get; set; }
        public string sAnhBia { get; set; }
        public double dDonGia { get; set; }
        public int iSoLuong { get; set; }
        public double dThanhTien
        {
            get { return iSoLuong * dDonGia; }
        }

       
        public GioHang(int ml)
        {
            iMaLap = ml;
            LAP s = db.LAPs.Single(n => n.MaLap == iMaLap);
            sTenLap = s.TenLap;
            sAnhBia = s.AnhBia;
            dDonGia = double.Parse(s.GiaBan.ToString());
            iSoLuong = 1;
        }
    }
}