using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebLT.Models;
using WebLT.Security;

namespace WebLT.Controllers
{
    public class UserLoginController : Controller
    {
        QL_LPDataContext db = new QL_LPDataContext();
        // GET: UserLogin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Sele()
        {
            return View();
        }

        public ActionResult Login(TAIKHOAN model, string returnUrl)
        {
            var user = db.TAIKHOANs
                .FirstOrDefault(a => a.username == model.username && a.password == model.password);

            if (user == null)
            {
                return View("Index");
            }
            else
            {
                SessionPersister.UserName = user;
            }

            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("ConfirmInfo", "Cart");
            }
        }


        public ActionResult LogOut()
        {
            SessionPersister.UserName = null;
            return RedirectToAction("AllProducer", "SANPHAM");
        }


        public ActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangKy(TAIKHOAN model,FormCollection f)
        {
            try
            {
                // TODO: Add insert logic here
                var user = f["username"];
                var tentk = f["tentk"];
                var pass = f["password"];
                var dt = f["phone"];
                var mail = f["mail"];
                var dc = f["diachi"];

                var cus = db.TAIKHOANs.Single(c => c.username == user);
                if (cus != null)
                {
                    ViewBag.TB = "Trung Ten Dang Nhap";
                }
                model.username = user;
                model.tentk = tentk;
                model.password = pass;
                model.phone = dt;
                model.mail = mail;
                model.diachi = dc;
                db.TAIKHOANs.InsertOnSubmit(model);
                db.SubmitChanges();
                return RedirectToAction("Index", "UserLogin");
            }
            catch
            {
                return View();
            }
        }

    }
}