using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webSimple.Areas.Admin.Data;
using webSimple.Areas.Admin.Models.Framework;
using webSimple.Models;

namespace webSimple.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admin/Login
        #region Admin
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginModel model)
        {
            using (DbDataContext context = new DbDataContext())
            {
                if (ModelState.IsValid)
                {
                    var res = context.Accounts.SingleOrDefault(a => a.Username.Equals(model.Username) && a.Password.Equals(Encryptor.MD5Hash(model.Password)));
                    if (res is null)
                    {
                        ModelState.AddModelError("", "Kiểm tra thông tin đăng nhập");
                        return View();
                    }

                    Session.Add(Constant.LOGIN_SESSION, res);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Thông tin trống !");
                return View();
            }

        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Login");
        }
        #endregion
    }
}