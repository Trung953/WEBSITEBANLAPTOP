using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebLT.Models;
using WebLTModBM;

namespace WebLT.Areas.Admin.Controllers
{
    [CustomAuthorize(Roles = "Admin")]
    public class DANHMUCADMINController : Controller
    {
        QL_LPDataContext db = new QL_LPDataContext();
        // GET: /Admin/DANHMUCADMIN/
        public ActionResult Index()
        {
            var dm = db.DANHMUCs
                 .Where(p => p.tendanhmuc != null);
            return View(dm);
        }

        //
        // GET: /Admin/DANHMUCADMIN/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Admin/DANHMUCADMIN/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Admin/DANHMUCADMIN/Create
        [HttpPost]
        public ActionResult Create(DANHMUC model)
        {
            try
            {
                // TODO: Add insert logic here
                var result = db.DANHMUCs.FirstOrDefault(s => s.ID_DM == model.ID_DM);
                if (result != null)
                {
                    return View();
                }
                db.DANHMUCs.InsertOnSubmit(model);
                db.SubmitChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Admin/DANHMUCADMIN/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Admin/DANHMUCADMIN/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, DANHMUC model)
        {
            try
            {
                model.ID_DM = id;
                var result = db.DANHMUCs.Single(s => s.ID_DM == id);
                if (result == null)
                {
                    return View();
                }
                result.tendanhmuc = model.tendanhmuc;
                db.SubmitChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Admin/DANHMUCADMIN/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Admin/DANHMUCADMIN/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, DANHMUC model)
        {
            try
            {
                model.ID_DM = id;

                var result = db.DANHMUCs.FirstOrDefault(s => s.ID_DM == id);
                db.DANHMUCs.DeleteOnSubmit(result);
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