using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebLT.Models;
using WebLT.ModBM;

namespace WebLT.Areas.Admin.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class DONHANGController : Controller
    {
        //
        // GET: /Admin/DONHANG/
        QL_LPDataContext db = new QL_LPDataContext();

        public ActionResult Index()
        {
            var dh = db.DONHANGs.Where(p => p.ID_DH != null).OrderBy(x => x.trangthai);
            return PartialView(dh);
        }

        [ChildActionOnly]
        public ActionResult SoDonHang()
        {
            var dh = db.DONHANGs.Where(p => p.trangthai == null).OrderBy(x => x.trangthai).ToList();
            ViewBag.SoDH = dh.Count();
            return PartialView(dh);
        }
        public ActionResult Details(int id)
        {
            var model = db.DONHANGs.Single(s => s.ID_DH == id);
            ViewBag.CTHD = db.CTDONHANGs.Where(x => x.ID_DH == id);
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var model = db.DONHANGs.Single(s => s.ID_DH == id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, DONHANG model)
        {
            try
            {
                model.ID_DH = id;
                var dbEntry = db.DONHANGs.Single(s => s.ID_DH == model.ID_DH);
                if (dbEntry == null)
                {
                    ViewBag.ThongBap = "Đơn Hàng Đã Tồn Tại!!!";
                }
                dbEntry.ID_TK = model.ID_TK;
                dbEntry.ID_TIN = model.ID_TIN;
                dbEntry.ngaylap = model.ngaylap;
                dbEntry.ngaynhanhang = model.ngaynhanhang;
                dbEntry.diachigiaohang = model.diachigiaohang;
                dbEntry.phone = model.phone;
                dbEntry.trangthai = model.trangthai;
                db.SubmitChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            return View();
        }
        public ActionResult InHoaDon(int id)
        {
            var dbEntry = db.DONHANGs.Single(s => s.ID_DH != id);
            if (dbEntry == null)
            {
                ViewBag.ThongBap = "Đơn Hàng Không Tồn Tại!!!";
            }
            var entry = (from a in db.DONHANGs
                         join b in db.CTDONHANGs on a.ID_DH equals b.ID_DH
                         join c in db.SANPHAMs on b.ID_SP equals c.ID_SP
                         where (a.ID_DH != null)
                         select new tblCTDonHangViewModel
                         {
                             ID_DH = a.ID_DH,
                             ID_TIN = a.ID_TIN,
                             ngaylap = a.ngaylap,
                             ngaynhanhang = a.ngaynhanhang,
                             diachigiaohang = a.diachigiaohang,
                             trangthai = a.trangthai,
                             hotenkh = a.hotenkh,
                             ID_CTDH = b.ID_CTDH,
                             ID_SP = b.ID_SP,
                             soluong = b.soluong,
                             dongia = b.dongia,
                             ID_DM = c.ID_DM,
                             tensanpham = c.tensanpham,
                             giabd = c.giabd,
                             trongluong = c.trongluong,
                             mausac = c.mausac,
                             ImgLink = c.ImgLink,
                             memory = c.memory,
                             hedieuhanh = c.hedieuhanh,
                             vga = c.vga,
                             cpuz = c.cpuz,
                             battery = c.cpuz,
                             phukiendikem = c.phukiendikem,
                             chedobaohanh = c.chedobaohanh
                         }).FirstOrDefault();
            return View(entry);
        }
        [HttpPost]
        public ActionResult Delete(int id, DONHANG model)
        {
            try
            {
                model.ID_DH = id;
                var result = db.DONHANGs.FirstOrDefault(s => s.ID_DH == id);
                if(id==null)
                {
                    ViewBag.ThongBao = "Khong Ton tai";
                }
                db.DONHANGs.DeleteOnSubmit(result);
                db.SubmitChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
	}
}