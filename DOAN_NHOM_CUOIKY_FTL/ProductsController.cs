using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.Web.UI.WebControls;
using DOAN_NHOM_CUOIKY_FTL.Models;
using PagedList;
using PagedList.Mvc;

namespace DOAN_NHOM_CUOIKY_FTL.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Products
        private DB4TLEntities db = new DB4TLEntities();
        public ActionResult Index(int? category, int? page, string SearchString, double min = double.MinValue, double max = double.MaxValue)
        {
            //Tạo Products và có tham chiếu đến category
            var products = db.Product.Include(p => p.Category);
            //tìm kiếm chuỗi truy vẫn theo category
            if (category == null)
            {
                products = db.Product.OrderByDescending(x => x.NamePro);
            }
            else
            {
                products = db.Product.OrderByDescending(x => x.CateID).Where(x => x.CateID == category);
            }
            //tìm theo tên
            if (!String.IsNullOrEmpty(SearchString))
            {
                products = products.Where(s => s.NamePro.Contains(SearchString));
            }
            if (min >= 0 && max > 0)
            {
                products = db.Product.OrderByDescending(x => x.Price).Where(p => (double)p.Price >= min && (double)p.Price <= max);
            }
            // Khai báo mỗi trang 4 sản phẩm
            int pageSize = 8;
            // Toán tử ?? trong C# mô tả nếu page khác null thì lấy giá trị page, còn
            // nếu page = null thì lấy giá trị 1 cho biến pageNumber.
            int pageNumber = (page ?? 1);

            // Nếu page = null thì đặt lại page là 1.
            if (page == null) page = 1;

            // Trả về các product được phân trang theo kích thước và số trang.
            return View(products.ToPagedList(pageNumber, pageSize));

        }
        public PartialViewResult CategoryPartial()
        {
            var cateList = db.Category.ToList();
            return PartialView(cateList);
        }
        public ActionResult slideshow()
        {
            return View(db.Product.ToList());
        }
        public ActionResult Viewdetail()
        {
            return View();
        }
        //detail
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }
       

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}

