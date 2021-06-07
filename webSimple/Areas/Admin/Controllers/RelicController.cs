using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webSimple.Areas.Admin.Models.Framework;
using webSimple.Models;

namespace webSimple.Areas.Admin.Controllers
{
    public class RelicController : BaseController
    {
        // GET: Admin/Relic
        #region CRUD
        // GET: Home
        [HttpGet]
        public ActionResult Index (int page=1, int pageSize = 10)
        {
            using (DbDataContext context = new DbDataContext())
            {
                var res = context.Relics.OrderByDescending(r => r.RelicName).ToList();
                return View(res.ToPagedList(page, pageSize));
            }
        }

        [HttpPost]
        public ActionResult Index(string name, int page=1, int pageSize = 10)
        {
            using (DbDataContext context = new DbDataContext())
            {
                var res = context.Relics.OrderByDescending(r => r.RelicName).ToList();
                if(name != null)
                {
                    res = (from p in context.Relics
                           where context.ufn_removeMark(p.RelicName).Contains(name) || context.ufn_removeMark(p.Title).Contains(name) || (p.RelicName).Contains(name) || (p.Title).Contains(name) || context.ufn_removeMark(p.Keyword).Contains(name) || (p.Keyword).Contains(name)
                           select p).ToList();
                }
                return View(res.ToPagedList(page, pageSize));
            }

        }

        [HttpGet]
        public ActionResult Create() 
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Relic relic, HttpPostedFileBase img)
        {
            using (DbDataContext context = new DbDataContext())
            {
                if (ModelState.IsValid)
                {
                    if (img != null && img.ContentLength > 0)
                    {
                        string filename = System.IO.Path.GetFileName(img.FileName);
                        string urlname = Server.MapPath("~/Assets/img/Relics/" + filename);
                        img.SaveAs(urlname);

                        relic.Img = "~/Assets/img/Relics/" + filename;
                    }


                    context.Relics.InsertOnSubmit(relic);

                    context.SubmitChanges();

                    return RedirectToAction("Index");
                }
                return View();
            }
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            using (DbDataContext context = new DbDataContext())
            {
                var res = context.Relics.SingleOrDefault(r => r.ID == id);
                if (res is null) return HttpNotFound();
                return View(res);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Relic relic, HttpPostedFileBase img)
        {
            using (DbDataContext context = new DbDataContext())
            {
                if (ModelState.IsValid)
                {
                    var res = context.Relics.SingleOrDefault(r => r.ID == relic.ID);

                    if (img != null && img.ContentLength > 0)
                    {
                        string filename = System.IO.Path.GetFileName(img.FileName);
                        string urlname = Server.MapPath("~/Assets/img/Relics/" + filename);
                        img.SaveAs(urlname);

                        res.Img = "~/Assets/img/Relics/" + filename;
                    }


                    res.RelicName = relic.RelicName;
                    res.Title = relic.Title;
                    res.Summary = relic.Summary;
                    res.Content = relic.Content;

                    context.SubmitChanges();

                    return RedirectToAction("Index");
                }
                return View();
            }
        }

        public ActionResult Details(int id)
        {
            using (DbDataContext context = new DbDataContext())
            {
                var res = context.Relics.SingleOrDefault(r => r.ID == id);
                if (res is null) return HttpNotFound();
                return View(res);
            }
        }
        public ActionResult Delete(int id)
        {
            using (DbDataContext context = new DbDataContext())
            {
                var res = context.Relics.SingleOrDefault(r => r.ID == id);
                if (res is null) return HttpNotFound();
                context.Relics.DeleteOnSubmit(res);
                context.SubmitChanges();
                return RedirectToAction("Index", "Relic");
            }
        }
        #endregion

        #region Keyword

        [HttpGet]
        public ActionResult Keyword(int page = 1, int pageSize = 10)
        {
            using (DbDataContext context = new DbDataContext())
            {
                var res = context.Relics.OrderByDescending(r => r.RelicName).ToList();
                return View(res.ToPagedList(page, pageSize));
            }
        }

        [HttpPost]
        public ActionResult Keyword(string name, int page = 1, int pageSize = 10)
        {
            using (DbDataContext context = new DbDataContext())
            {
                var res = context.Relics.OrderByDescending(r => r.RelicName).ToList();
                if (name != null)
                {
                    res = (from p in context.Relics
                           where context.ufn_removeMark(p.RelicName).Contains(name) || context.ufn_removeMark(p.Title).Contains(name) || (p.RelicName).Contains(name) || (p.Title).Contains(name) || context.ufn_removeMark(p.Keyword).Contains(name) || (p.Keyword).Contains(name)
                           select p).ToList();
                }
                return View(res.ToPagedList(page, pageSize));
            }

        }
        [HttpGet]
        public ActionResult EditKeyword(int id)
        {
            using (DbDataContext context = new DbDataContext())
            {
                var res = context.Relics.SingleOrDefault(r => r.ID == id);
                if (res is null) return HttpNotFound();
                return View(res);
            }
        }

        [HttpPost]
        public ActionResult EditKeyword(Relic relic)
        {
            using (DbDataContext context = new DbDataContext())
            {
                if (ModelState.IsValid)
                {
                    var res = context.Relics.SingleOrDefault(r => r.ID == relic.ID);

                    res.Keyword = relic.Keyword;
                    context.SubmitChanges();

                    return RedirectToAction("Keyword");
                }
                return View();
            }
        }

        public ActionResult DetailsKeyword(int id)
        {
            using (DbDataContext context = new DbDataContext())
            {
                var res = context.Relics.SingleOrDefault(r => r.ID == id);
                if (res is null) return HttpNotFound();
                return View(res);
            }
        }
        #endregion Keyword
    }
}