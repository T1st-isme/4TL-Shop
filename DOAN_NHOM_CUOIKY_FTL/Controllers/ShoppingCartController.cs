using DOAN_NHOM_CUOIKY_FTL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DOAN_NHOM_CUOIKY_FTL.Controllers
{
    public class ShoppingCartController : Controller
    {
        // GET: ShoppingCart
        private DB4TLEntities db = new DB4TLEntities();
        public ActionResult ShowCart()
        {
            Cart _cart = Session["Cart"] as Cart;

            if (Session["Cart"] == null)
                return View("EmptyCart");
            return View(_cart);
        }
        // them moi gio hang
        public Cart GetCart()
        {
            Cart cart = Session["Cart"] as Cart;
            if (cart == null || Session["Cart"] == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }
        //them product vao gio hang
        public ActionResult AddToCart(int id)
        {
            var _pro = db.Product.SingleOrDefault(s => s.ProductID == id);
            if (_pro != null && _pro.Quantity > 0)
            {
                GetCart().Add_Product_Cart(_pro);
            }
            else if(_pro.Quantity == 0)
            {
                return Content("Mặt hàng này hiện đã hết");
            }
            return RedirectToAction("ShowCart", "ShoppingCart");
        }
        public ActionResult Update_Cart_Quantity(FormCollection form)
        {
            Cart cart = Session["Cart"] as Cart;
            int id_pro = int.Parse(form["idPro"]);
            int _quantity = int.Parse(form["cartQuantity"]);
            if (_quantity > 0)
            {
                cart.Update_quantity(id_pro, _quantity);
            }

            return RedirectToAction("ShowCart", "ShoppingCart");
        }
        public ActionResult RemoveCart(int id)
        {
            Cart cart = Session["Cart"] as Cart;
            cart.Remove_CartItem(id);
            if (cart.Items.Count() == 0)
            {
                cart = null;
                Session["Cart"] = null;
            }
                return RedirectToAction("ShowCart", "ShoppingCart");
            
        }
        public PartialViewResult BagCart()
        {
            int total_quantity_item = 0;
            Cart cart = Session["Cart"] as Cart;
            if (cart != null)
            {
                total_quantity_item = cart.Total_quantity();
            }
            ViewBag.QuantityCart = total_quantity_item;
            return PartialView("BagCart");
        }
        public ActionResult CheckOut(FormCollection form)
        {
            try
            {
                Cart cart = Session["Cart"] as Cart;
                OrderPro _order = new OrderPro
                {
                    DateOrder = DateTime.Now,
                    AddressDeliverry = form["AddressDeliverry"],
                    IDCus = int.Parse(form["CodeCustomer"])
                }; //Bảng hóa đơn sản SP
                db.OrderPro.Add(_order);
                foreach (var item in cart.Items)
                {
                    OrderDetail _order_detail = new OrderDetail
                    {
                        IDOrder = _order.ID,
                        IDProduct = item._product.ProductID,
                        UnitPrice = (double)item._product.Price,
                        Quantity = item._quantity
                    }; //Lưu dòng sp và bảng chi tiết hóa đơn
                    db.OrderDetail.Add(_order_detail);
                    foreach (var p in db.Product.Where(s => s.ProductID == _order_detail.IDProduct)) // lấy ID Product có trong giỏ hàng
                    {
                        var update_quan_pro = p.Quantity - item._quantity; //Số lượng tồn mới  = số lượng tồn - số lượng đã mua 
                        if (p.Quantity > 0)
                        {
                            p.Quantity = update_quan_pro; //Thực hiện cập nhật lại số lượng tồn cho cột Quantity của bảng Product

                        }
                    }
                }
                db.SaveChanges();
                cart.ClearCart();
                
                    return RedirectToAction("CheckOut_Success", "ShoppingCart");
                
                    
            }
            catch
            {
                return Content("Có sai sót. Xin vui lòng kiểm tra lại thông tin...........Xin cảm ơn");
            }
        }
        public ActionResult CheckOut_Success()
        {
            return View();
        }

    
    
    }
}