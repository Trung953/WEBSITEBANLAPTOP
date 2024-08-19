using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebLT.Models;
using WebLT.Security;

namespace WebLT.Controllers
{
    public class SanPhamController : Controller
    {
        QL_LPDataContext db = new QL_LPDataContext();
        // GET: SanPham
        public ActionResult AllProducer(string keyword, int? page)
        {
            int pageNumber = (page ?? 1);
            int itemsPerPage = 16;


            var products = db.SANPHAMs.ToList();
            var pageSize = products.Count / itemsPerPage;

            var sp = products
                         .Where(p => p.ID_SP != null)
                         .OrderBy(p => p.tensanpham)
                         .ToList();

            if (!string.IsNullOrEmpty(keyword) && !string.IsNullOrWhiteSpace(keyword))
            {
                sp = sp.Where(p => !string.IsNullOrEmpty(p.tensanpham) && p.tensanpham.ToLower().Contains(keyword.ToLower()))
                    .ToList();
            }
            return View(sp.OrderBy(n => n.ID_SP).ToPagedList(pageNumber, itemsPerPage));
        }

        public ActionResult KhoangGia(string keyword, int? page, int? cantren, int? canduoi)
        {
            int pageNumber = (page ?? 1);
            int itemsPerPage = 12;
            int? ct = cantren;
            int? cd = canduoi;

            var products = db.SANPHAMs.ToList();
            var pageSize = products.Count / itemsPerPage;

            var sp = products
                         .Where(p => p.giabd > cd & p.giabd <= ct)
                         .OrderBy(p => p.tensanpham)
                         .ToList();

            if (!string.IsNullOrEmpty(keyword) && !string.IsNullOrWhiteSpace(keyword))
            {
                sp = sp.Where(p => !string.IsNullOrEmpty(p.tensanpham) && p.tensanpham.ToLower().Contains(keyword.ToLower()))
                    .ToList();
            }
            return View("AllProducer", sp.OrderBy(n => n.ID_SP).ToPagedList(pageNumber, itemsPerPage));


        }

        [HttpPost]
        public ActionResult Comment(FormCollection fr, int id)
        {
            try
            {

                // TODO: Add insert logic here

                List<BINHLUAN> lstbl = new List<BINHLUAN>();
                BINHLUAN model = new BINHLUAN()
                {
                    ID_SP = id,
                    noidung = fr["textbinhluan"],
                    ID_TK = SessionPersister.UserName.ID_TK,
                    ngaybinhluan = DateTime.Now,
                };
                lstbl.Add(model);
                // Save changes to the database
                db.SubmitChanges();

                if (lstbl.Count==0)
                {
                    return View();
                }
                return RedirectToAction("Details", new RouteValueDictionary(new { Controller = "SanPham", Action = "Details", id = id }));
            }
            catch (Exception ex)
            {
                return RedirectToAction("Details", new RouteValueDictionary(new { Controller = "SanPham", Action = "Details", id = id }));
            }
        }

        public ActionResult Details(int id)
        {
            var result = db.SANPHAMs.FirstOrDefault(s => s.ID_SP == id);
            return View(result);
        }
        public ActionResult SPKhuyenMai()
        {
            var model = db.SPGIAMGIAs;
            var query = model.OrderByDescending(p => p.ID_KM);
            return View(query.ToList());
        }
        public ActionResult ViewKhoangGia()
        {
            var dm = db.KHOANGGIAs.Where(p => p.IDKG != null);
            return PartialView(dm);
        }
        public ActionResult ViewBL()
        {
            var model = db.BINHLUANs;
            var query = model.OrderByDescending(p => p.TAIKHOAN.username);
            return PartialView(query.ToList());
        }

        public ActionResult CTDonHangUser(int id)
        {
            var model = db.DONHANGs.FirstOrDefault(s => s.ID_DH == id);
            ViewBag.CTHD = db.CTDONHANGs.Where(x => x.ID_DH == id);
            return View(model);
        }

        public ActionResult FilterSPofDM(int? id)
        {
            DANHMUC dm = db.DANHMUCs.FirstOrDefault(s => s.ID_DM == id);
            if (dm != null)
            {
                ViewBag.TenDM = dm.tendanhmuc;
            }
            var sp = db.SANPHAMs.Where(p => p.ID_DM == id);
            return View(sp);
        }

        // [HttpPost]
        public ActionResult KQTimKiem(string txttimkiem, int? page)
        {
            var products = db.SANPHAMs.Where(p => string.IsNullOrEmpty(p.tensanpham) == false &&
                                                    p.tensanpham.ToLower().Contains(txttimkiem))
                                                    .ToList();
            return View();
        }

        public PartialViewResult SPOfDB(int id)
        {
            var sp = (from a in db.SANPHAMs
                      where (a.ID_SP == id)
                      select a).FirstOrDefault();
            var query = (from a in db.SANPHAMs
                         where (a.ID_DM == sp.ID_DM && a.ID_SP != sp.ID_SP)
                         select a).OrderByDescending(r => r.ID_SP).Take(4);
            return PartialView("SPOfDB", query);
        }

        public PartialViewResult SPOfNew()
        {
            var sp = db.SANPHAMs.OrderByDescending(p => p.ID_SP).Take(12).ToList();
            return PartialView("SPOfNew", sp);
        }

    }
}