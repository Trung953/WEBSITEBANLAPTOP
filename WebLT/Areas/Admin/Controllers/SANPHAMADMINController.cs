using WebLT.Models;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using PagedList.Mvc;
using System.Web.Mvc;
using WebLTModBM;

namespace WebLT.Areas.Admin.Controllers
{
    [CustomAuthorize(Roles = "Admin")]
    public class SANPHAMADMINController : Controller
    {
        //
        // GET: /Admin/SANPHAMADMIN/
        //
        QL_LPDataContext db = new QL_LPDataContext();
        public ActionResult Index(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 12;
            var sp = db.SANPHAMs.Where(p => p.ID_SP != null);
            return View(sp.ToList().OrderBy(n => n.ID_SP).ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: /Admin/SANPHAMADMIN/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        public ActionResult TimKiemMod(string txttimkiemMod)
        {
            var products = db.SANPHAMs.Where(p => string.IsNullOrEmpty(p.tensanpham) == false &&
                                                        p.tensanpham.ToLower().Contains(txttimkiemMod))
                                                        .ToList();
                
            return View();
        }

        public ActionResult TimKiemSanPhamAdmin(string txttimkiem, int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            var sp = db.SANPHAMs.Where(p => p.tensanpham.Contains(txttimkiem));
            return View(sp.ToList().OrderBy(n => n.ID_SP).ToPagedList(pageNumber, pageSize));
        }
        //
        // GET: /Admin/SANPHAMADMIN/Create
        public ActionResult Create()
        {
            var dao = db.DANHMUCs.Where(p => p.tendanhmuc != null);
            ViewBag.ID_DM = new SelectList(dao, "ID_DM", "tendanhmuc", null);
            return View();
        }

        //
        // POST: /Admin/SANPHAMADMIN/Create
        [HttpPost]
        public ActionResult Create(SANPHAM model, HttpPostedFileBase file)
        {
            try
            {
                if (file == null)
                {
                    ModelState.AddModelError("File", "Please Upload Your file");
                }
                else if (file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
                    file.SaveAs(path);
                    db.SANPHAMs.InsertOnSubmit(model);
                    db.SubmitChanges();
                    // Đẩy File lên Server

                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        //
        // GET: /Admin/SANPHAMADMIN/Edit/5
        public ActionResult Edit(int id)
        {
            var model = db.SANPHAMs.Single(s => s.ID_SP == id);
            var dao = db.DANHMUCs;
            ViewBag.ID_DM = new SelectList(dao, "ID_DM", "tendanhmuc", model.ID_DM);
            var test = new SelectList(dao, "ID_DM", "tendanhmuc", model.ID_DM);
            return View(model);
        }

        //
        // POST: /Admin/SANPHAMADMIN/Edit/5
        [HttpPost]
        public ActionResult Edit(int id,SANPHAM model, HttpPostedFileBase file)
        {
            try
            {
                // TODO: Add insert logic here
                //
                if (file == null)
                {
                    ModelState.AddModelError("File", "Please Upload Your file");
                }
                else if (file.ContentLength > 0)
                {                 //TO:DO
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
                    file.SaveAs(path);
                    //     ModelState.Clear();
                    // TODO: Add insert logic here
                    model.ImgLink = fileName;
                    SANPHAM sp = new SANPHAM();
                    sp.tensanpham = model.tensanpham;
                    sp.mausac = model.mausac;
                    sp.memory = model.memory;
                    sp.phukiendikem = model.phukiendikem;
                    sp.cpuz = model.cpuz;
                    sp.chedobaohanh = model.chedobaohanh;

                    db.SubmitChanges();
                    // Upload File đẩy về Server


                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Admin/SANPHAMADMIN/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Admin/SANPHAMADMIN/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, SANPHAM model)
        {
            try
            {
                model.ID_SP = id;
                var result = db.SANPHAMs.FirstOrDefault(s => s.ID_SP == id);
                if (id == null)
                {
                    ViewBag.ThongBao = "Khong ton tai";
                }
                db.SANPHAMs.DeleteOnSubmit(result);
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
