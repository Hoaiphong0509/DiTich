using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using webSimple.Models;

namespace webSimple.Controllers
{
    public class HomeController : Controller
    {
        #region test
        private static List<string> relicnames;
        public static List<string> RelicNames
        {
            get
            {
                if (relicnames is null)
                    using (DbDataContext context = new DbDataContext())
                    {
                        relicnames = context.Relics.Select(r => convertToUnSign2(r.RelicName)).ToList();
                    }
                return relicnames;
            }
        }
        private List<string> Search(string keyword)
        {
            keyword = convertToUnSign2(keyword).ToUpper();

            return RelicNames.Where(x => x.Contains(keyword)).ToList();
        }

        public static string convertToUnSign2(string s)
        {
            string stFormD = s.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();
            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                System.Globalization.UnicodeCategory uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }
            sb = sb.Replace('Đ', 'D');
            sb = sb.Replace('đ', 'd');
            return (sb.ToString().Normalize(NormalizationForm.FormD));
        }
        #endregion

        public void SetSideBar()
        {
            using (DbDataContext context = new DbDataContext())
            {
                var sidebar = context.Relics.OrderByDescending(r => r.ViewCount).Take(3).ToList();
                ViewBag.Sidebar = sidebar;
            }
        }

        [HttpPost]
        public JsonResult AutoComplete(string prefix)
        {
            //return Json(Search(prefix));
            using (DbDataContext context = new DbDataContext())
            {
                var res = context.Relics.Where(s => context.ufn_removeMark(s.RelicName).Contains(prefix) || context.ufn_removeMark(s.Title).Contains(prefix) || (s.RelicName).Contains(prefix) || (s.Title).Contains(prefix) || context.ufn_removeMark(s.Keyword).Contains(prefix) || (s.Keyword).Contains(prefix)).Select(x => x.RelicName.ToLower()).ToList();
                return Json(res);
            }
        }

        [HttpGet]
        public ActionResult Index(int page = 1, int pageSize = 4)
        {
            using (DbDataContext context = new DbDataContext())
            {
                SetSideBar();

                var homepage = context.Relics.OrderByDescending(r => r.ViewCount).ToPagedList(page, pageSize);
                return View(homepage);
            }

        }

        [HttpPost]
        public ActionResult Index(string name, int page = 1, int pageSize = 4)
        {
            using (DbDataContext context = new DbDataContext())
            {
                SetSideBar();

                var res = context.Relics.OrderByDescending(r => r.ViewCount).ToList();
                if (name != null)
                {
                    res = (from p in context.Relics
                           where context.ufn_removeMark(p.RelicName).Contains(name) || context.ufn_removeMark(p.Title).Contains(name) || (p.RelicName).Contains(name) || (p.Title).Contains(name) || context.ufn_removeMark(p.Keyword).Contains(name) || (p.Keyword).Contains(name)
                           select p).ToList();
                }
                return View(res.ToPagedList(page, pageSize));
            }

        }

        public ActionResult Details(int id)
        {
            using (DbDataContext context = new DbDataContext())
            {
                SetSideBar();
                var res = context.Relics.SingleOrDefault(r => r.ID == id);
                if (res.ViewCount == null) res.ViewCount = 1;
                else res.ViewCount++;

                context.SubmitChanges();

                return View(res);
            }

        }

        public ActionResult About()
        {
            using (DbDataContext context = new DbDataContext())
            {
                SetSideBar();
                var res = context.Abouts.FirstOrDefault(a => a.Status == true);
                return View(res);
            }
            
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}