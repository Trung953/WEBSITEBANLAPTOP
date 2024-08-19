using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebLT.Models;
using WebLT.Security;

namespace WebLT.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        QL_LPDataContext db = new QL_LPDataContext();
        private const string CartSession = "CartSession";
        public ActionResult Index()
        {
            var cart = (Cart)Session[CartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = cart.Lines.ToList();
                ViewBag.TongTien = cart.ComputeTotalValue();
                ViewBag.TotalItem = cart.TotalItem();
            }

            return View(list);
        }
        public ActionResult AddItem(int Id)
        {

            var product = db.SANPHAMs.FirstOrDefault(s => s.ID_SP == Id);
            var cart = (Cart)Session[CartSession];
            if (cart != null)
            {
                cart.AddItem(product, 1);
                //Gán vào session
                Session[CartSession] = cart;
            }
            else
            {
                //tạo mới đối tượng cart item
                cart = new Cart();
                cart.AddItem(product, 1);
                //Gán vào session
                Session[CartSession] = cart;
            }
            return RedirectToAction("Index");
        }

        public RedirectToRouteResult AddToCart(int id)
        {
            var product = db.SANPHAMs.FirstOrDefault(s => s.ID_SP == id);
            var cart = (Cart)Session[CartSession];
            if (cart != null)
            {
                cart.AddItem(product, 1);
                //Gán vào session
                Session[CartSession] = cart;
            }
            else
            {
                //tạo mới đối tượng cart item
                cart = new Cart();
                cart.AddItem(product, 1);
                //Gán vào session
                Session[CartSession] = cart;
            }
            return RedirectToAction("Index", "Cart");
        }
        public ActionResult RemoveOneItem(int Id)
        {

            var product = db.SANPHAMs.FirstOrDefault(s => s.ID_SP == Id);
            var cart = (Cart)Session[CartSession];
            if (cart != null)
            {
                cart.AddItem(product, -1);
                //Gán vào session
                Session[CartSession] = cart;
            }
            return RedirectToAction("Index", "Cart");
        }

        public ActionResult Clear()
        {
            var cart = (Cart)Session[CartSession];
            cart.Clear();
            Session[CartSession] = cart;
            return RedirectToAction("Index", "Cart");
        }
        public ActionResult RemoveLine(int Id)
        {

            var product = db.SANPHAMs.FirstOrDefault(s => s.ID_SP == Id);
            var cart = (Cart)Session[CartSession];
            if (cart != null)
            {
                cart.RemoveLine(product);
                //Gán vào session
                Session[CartSession] = cart;
            }
            return RedirectToAction("Index");
        }
        public ActionResult Payment(string name, string mobileadd, string diachiadd, string dateout)
        {
            // A
            DONHANG order = new DONHANG();
            order.ngaylap = DateTime.Now;
            order.hotenkh = name;
            order.diachigiaohang = diachiadd;
            order.phone = mobileadd;
            DateTime? date = null;
            DateTime temp;

            if (DateTime.TryParse(dateout, out temp))
            {
                if (temp != null)
                    date = temp;
            }

            if (date != null)
                order.ngaynhanhang = date.Value;

            // B

            //nếu login
            if (SessionPersister.UserName != null)
            {
                order.ngaynhanhang = DateTime.Now;
                order.ID_TK = SessionPersister.UserName.ID_TK;

                var account = db.TAIKHOANs.FirstOrDefault(tk => tk.ID_TK == order.ID_TK);
                order.hotenkh = account.tentk;
                order.diachigiaohang = account.diachi;
                order.phone = account.phone;
            }
            try
            {
                db.DONHANGs.InsertOnSubmit(order);
                db.SubmitChanges();

                var id = order.ID_DH; // Assuming ID_DH is the primary key and is auto-generated after SubmitChanges

                var cart = (Cart)Session["CartSession"];
                var detailDao = db.CTDONHANGs;
                foreach (var item in cart.Lines)
                {
                    var orderDetail = new CTDONHANG();
                    orderDetail.ID_SP = item.Sanpham.ID_SP;
                    orderDetail.ID_DH = id;
                    orderDetail.soluong = item.Quantity;
                    orderDetail.dongia = (item.Sanpham.giabd * item.Quantity);
                    detailDao.InsertOnSubmit(orderDetail);
                }

                Session["CartSession"] = null;
            }
            catch (Exception ex)
            {
                //ghi log
                return RedirectToAction("Loi"); // action Loi ở đâu?
            }

            return RedirectToAction("MuaHangThanhCong", "Cart");
        }

        public ActionResult PaymentNotLogin()
        {
            var cart = (Cart)Session[CartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = cart.Lines.ToList();
                ViewBag.TongTien = cart.ComputeTotalValue();
                ViewBag.TotalItem = cart.TotalItem();
            }

            return View(list);
        }
        public ActionResult MuaHangThanhCong()
        {
            return View();
        }
        public PartialViewResult ShowInfor()
        {
            var user = (from a in db.TAIKHOANs
                        where (a.username == SessionPersister.UserName.username)
                        select a).FirstOrDefault();
            return PartialView("ShowInfor", user);
        }

        public ActionResult ConfirmInfo()
        {
            if (SessionPersister.UserName == null)
            {
                return RedirectToAction("Index", "UserLogin");
            }

            var cart = (Cart)Session[CartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = cart.Lines.ToList();
                ViewBag.TongTien = cart.ComputeTotalValue();
                ViewBag.TotalItem = cart.TotalItem();
            }
            return View(list);
        }
    }
}