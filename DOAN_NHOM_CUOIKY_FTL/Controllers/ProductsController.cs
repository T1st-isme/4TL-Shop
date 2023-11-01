using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DOAN_NHOM_CUOIKY_FTL.Models;
using PagedList;

namespace DOAN_NHOM_CUOIKY_FTL.Controllers
{
    public class ProductsController : Controller
    {
        private DB4TLEntities db = new DB4TLEntities();     
        public ActionResult ADIndex()
        {
            var product = db.Product.Include(p => p.Category);
            return View(product.ToList());
        }
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
        public ActionResult Create()
        {
            ViewBag.CateID = new SelectList(db.Category, "IDCate", "NameCate");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,NamePro,DecriptionPro,CateID,Price,ImagePro,UserName,Quantity")] Product product)
        {
           
            if (ModelState.IsValid)
            {
                db.Product.Add(product);
                db.SaveChanges();
                return RedirectToAction("ADIndex");
            }

            ViewBag.CateID = new SelectList(db.Category, "IDCate", "NameCate", product.CateID);
            return View(product);
           
        }
        public ActionResult Edit(int? id)
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
            ViewBag.CateID = new SelectList(db.Category, "IDCate", "NameCate", product.CateID);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,NamePro,DecriptionPro,CateID,Price,ImagePro,UserName,Quantity")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ADIndex");
            }
            ViewBag.CateID = new SelectList(db.Category, "IDCate", "NameCate", product.CateID);
            return View(product);
        }
        public ActionResult Delete(int? id)
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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Product.Find(id);
            db.Product.Remove(product);
            db.SaveChanges();
            return RedirectToAction("ADIndex");
        }

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
        //partial
        public PartialViewResult CategoryPartial()
        {
            var cateList = db.Category.ToList();
            return PartialView(cateList);
        }
     
        public ActionResult slideshow(int?pages)
        {
            int pageSize = 10;
            int pageNum = (pages ?? 1);
            List<Product> proList = db.Product.ToList();
            var query = (from item in db.Product
                         orderby item.ProductID descending
                         select item);

            return View(query.Take(10).ToPagedList(pageNum, pageSize));
            
        }

         public ActionResult Viewdetail(int? id)
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
        public ActionResult TopNew(int? pages)
        {

            int pageSize = 5;
            int pageNum = (pages ?? 1);
            List<Product> proList = db.Product.ToList();
            var query = (from item in db.Product
                         orderby item.ProductID descending
                         select item);

            return View(query.Take(10).ToPagedList(pageNum, pageSize));
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
