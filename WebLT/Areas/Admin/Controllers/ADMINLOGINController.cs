using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebLT.Models;
using WebLT.ModBM;

namespace WebLT.Areas.Admin.Controllers
{
    public class ADMINLOGINController : Controller
    {
        // GET: Admin/ADMINLOGIN
        public ActionResult Index(TAIKHOAN model)
        {
            using (var dbContext = new QL_LPDataContext()) // Thay YourDbContext bằng tên DbContext của bạn
            {
                if (string.IsNullOrEmpty(model.username) || string.IsNullOrEmpty(model.password)
                    || dbContext.TAIKHOANs.FirstOrDefault(a => a.username == model.username && a.password == model.password) == null)
                {
                    return View("Index");
                }
                SessionPersister.UserName = model.username;
                return RedirectToAction("Index", "HOMEADMIN");
            }
        }


        public ActionResult LogOut()
        {
            SessionPersister.UserName = null;
            return RedirectToAction("Index", "ADMINLOGIN");
        }

        //
        // GET: /Admin/ADMINLOGIN/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Admin/ADMINLOGIN/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Admin/ADMINLOGIN/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Admin/ADMINLOGIN/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Admin/ADMINLOGIN/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Admin/ADMINLOGIN/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Admin/ADMINLOGIN/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}