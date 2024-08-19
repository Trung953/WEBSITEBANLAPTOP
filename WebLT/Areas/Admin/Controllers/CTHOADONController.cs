using WebLT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebLT.Areas.Admin.Controllers
{
    public class CTHOADONController : Controller
    {
        //
        // GET: /Admin/CTHOADON/
        QL_LPDataContext db = new QL_LPDataContext();
        public ActionResult Index()
        {
            var dm = db.CTDONHANGs.Where(p => p.ID_CTDH != null);
            return PartialView(dm);
        }

        public ActionResult Edit(int id)
        {

            var model = db.CTDONHANGs.Single(s => s.ID_CTDH == id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, CTDONHANG model)
        {
            try
            {
                var dbEntry = db.CTDONHANGs.FirstOrDefault(c => c.ID_CTDH == id);
                if (dbEntry != null)
                {
                    // Cập nhật thuộc tính của dbEntry từ model
                    dbEntry.ID_DH = model.ID_DH;
                    dbEntry.ID_SP = model.ID_SP;
                    dbEntry.soluong = model.soluong;
                    dbEntry.dongia = model.dongia;
                    // Lưu thay đổi vào cơ sở dữ liệu
                    db.SubmitChanges();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}