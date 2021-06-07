using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webSimple.Areas.Admin.Models.Framework;
using webSimple.Models;
using PagedList;

namespace webSimple.Areas.Admin.Controllers
{
    public class ImageController : BaseController
    {
        // GET: Admin/Image
        public ActionResult Index(int page=1, int pageSize=20)
        {
            using (DbDataContext context = new DbDataContext())
            {
                var res = context.Images.OrderByDescending(i => i.Url).ToPagedList(page, pageSize);
                return View(res);
            }
        }


        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(ImageModel model, HttpPostedFileBase img)
        {
            using (DbDataContext context = new DbDataContext())
            {
                if (img != null && img.ContentLength > 0)
                {
                    string filename = System.IO.Path.GetFileName(img.FileName);
                    string urlname = Server.MapPath("~/Uploads/" + filename);
                    img.SaveAs(urlname);

                    model.Url = "~/Uploads/" + filename;
                }

                if (ModelState.IsValid)
                {
                    Image image = new Image() {
                        Url = model.Url
                    };

                    context.Images.InsertOnSubmit(image);
                    context.SubmitChanges();

                    return RedirectToAction("Index");
                }
                return View();
            }

        }
        [HttpPost]
        public ActionResult Delete(long id)
        {
            using (DbDataContext context = new DbDataContext())
            {
                var res = context.Images.SingleOrDefault(r => r.Id == id);
                if (res is null) return HttpNotFound();
                context.Images.DeleteOnSubmit(res);
                context.SubmitChanges();
                return RedirectToAction("Index");
            }
        }
    }
}