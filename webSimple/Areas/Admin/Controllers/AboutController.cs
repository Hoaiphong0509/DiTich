using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webSimple.Models;

namespace webSimple.Areas.Admin.Controllers
{
    public class AboutController : BaseController
    {

        // GET: Admin/About
        public ActionResult Index()
        {
            using (DbDataContext context = new DbDataContext())
            {
                var res = context.Abouts.ToList();
                return View(res);
            }
                
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(About about)
        {
            using (DbDataContext context = new DbDataContext())
            {
                if (ModelState.IsValid)
                {
                    if(about.Status==true)
                        foreach(var item in context.Abouts.ToList())
                        {
                            if (item.Id != about.Id) item.Status = false;
                        }
                    if (about.Status is null)
                        about.Status = false;

                    context.Abouts.InsertOnSubmit(about);
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
                var res = context.Abouts.SingleOrDefault(a => a.Id == id);
                return View(res);
            }
        }

        [HttpPost]
        public ActionResult Edit(About about)
        {
            using (DbDataContext context = new DbDataContext())
            {
                if (ModelState.IsValid)
                {
                    var res = context.Abouts.SingleOrDefault(a => a.Id == about.Id);
                    if (about.Status == true)
                        foreach (var item in context.Abouts.ToList())
                        {
                            if (item.Id != about.Id) item.Status = false;
                        }
                    if (about.Status is null)
                        res.Status = false;

                    res.Author = about.Author;
                    res.Title = about.Title;
                    res.Content = about.Content;
                    res.Status = about.Status;

                    context.SubmitChanges();
                    return RedirectToAction("Index");
                }
                return View();
            }
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            using (DbDataContext context = new DbDataContext())
            {
                var res = context.Abouts.SingleOrDefault(a => a.Id == id);
                return View(res);
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            using (DbDataContext context = new DbDataContext())
            {
                var res = context.Abouts.SingleOrDefault(a => a.Id == id);
                context.Abouts.DeleteOnSubmit(res);
                context.SubmitChanges();
                return View(res);
            }
        }


    }
}