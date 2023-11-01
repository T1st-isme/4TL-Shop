using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOAN_NHOM_CUOIKY_FTL.Models;

namespace DOAN_NHOM_CUOIKY_FTL.Controllers
{
    public class LoginUserController : Controller
    {
        // GET: LoginUser
        private DB4TLEntities db = new DB4TLEntities();

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginAccount(AdminUser _user)
        {
             var check = db.AdminUser.Where(s => s.NameUser == _user.NameUser && s.PasswordUser == _user.PasswordUser).FirstOrDefault();
            if (check == null)
            {
                ViewBag.ErrorInfo = "Sai Info";
                return View("Index");
            }
            else
            {
                db.Configuration.ValidateOnSaveEnabled = false;
                Session["ID"] = _user.ID;
                Session["NameUser"] = _user.NameUser;
                Session["PasswodUser"] = _user.PasswordUser;
                Session["RoleUser"] = check.RoleUser;              
                //var ch = Session["chon"];
                //return RedirectToAction("ProductList", "Products");
                if (check.RoleUser.ToString() == "Sanpham")
                    return RedirectToAction("ADIndex", "Products");
                else if (check.RoleUser.ToString() == "Khachhang")
                    return RedirectToAction("Index", "Customers");
                else if (check.RoleUser.ToString() == "Donhang")
                    return RedirectToAction("Index", "OrderProes");
                else if (check.RoleUser.ToString() == "Admin")
                    return RedirectToAction("ViewAD", "LoginUser");
                else
                    return RedirectToAction("Index", "LoginUser");
            }
        }
        public ActionResult LogOutUserAD()
        {
            Session.Abandon();
            return RedirectToAction("Index", "LoginUser");
        }
        [HttpGet]
        public ActionResult RegisterUser()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RegisterUser(AdminUser _user)
        {
            if (ModelState.IsValid)
            {
                var check_ID = db.AdminUser.Where(s => s.ID == _user.ID).FirstOrDefault();
                if (check_ID == null)
                {
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.AdminUser.Add(_user);
                    db.SaveChanges();
                    return RedirectToAction("Index", "LoginUser");
                }
                else
                {
                    ViewBag.ErrorRegister = "This ID is exist";
                    return View();
                }
            }
            return View();
        }
        public ActionResult ViewAD()
        {
            return View();
        }
      

    }
}