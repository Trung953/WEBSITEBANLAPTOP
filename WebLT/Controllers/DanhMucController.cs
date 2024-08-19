using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebLT.Models;

namespace WebLT.Controllers
{
    public class DanhMucController : Controller
    {

        QL_LPDataContext db = new QL_LPDataContext();
        // GET: DanhMuc
        public ActionResult HSX()
        {
            var dm = db.DANHMUCs.Where(p => p.tendanhmuc != null);
            return PartialView(dm);
        }
        public ActionResult Index()
        {

            return View();
        }
        public ActionResult DMSP()
        {
            var dm = db.DANHMUCs.Where(p => p.tendanhmuc != null);
            return PartialView(dm);
        }
        public ActionResult DMTinTuc()
        {
            var query = db.TINTUCs.OrderByDescending(p => p.ID_NHOM);
            return PartialView(query.ToList());
        }
    }
}