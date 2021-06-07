using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webSimple.Areas.Admin.Models;
using webSimple.Models;

namespace webSimple.Areas.Admin.Controllers
{
    public class ViewChartController : BaseController
    {
        //----------------------------------
        //==================================
        //----------------------------------
        #region HorizontalBarChart - Lượt xem
        public List<string> GetNameRelic()
        {
            using (DbDataContext model = new DbDataContext())
            {
                var res = model.Relics.ToList();
                List<string> name = new List<string>();
                foreach (var item in res)
                {
                    name.Add(item.RelicName);
                }
                return name;
            }
        }

        public List<int> GetViewsCount()
        {
            using (DbDataContext model = new DbDataContext())
            {
                List<int> view = new List<int>();
                List<int> listid = new List<int>();
                var relic = model.Relics.ToList();
                foreach (var idrelic in relic)
                {
                    var res = listid.Find(x => x == idrelic.ID);
                    if (res == 0)
                    {
                        listid.Add((int)idrelic.ID);
                    }

                }

                foreach (var item in relic)
                {
                    if (item.ViewCount == null) item.ViewCount = 1;
                    view.Add((int)item.ViewCount);
                }
                return view;
            }
        }
        #endregion
        //----------------------------------
        //==================================
        //----------------------------------
        // GET: Admin/ViewChart
        public ActionResult Index()
        {
            #region HorizontalBarChart- Lượt xem
            using (DbDataContext model = new DbDataContext())
            {
                ViewBag.LabelHorizontalBarChart = GetNameRelic();
                ViewBag.Data = GetViewsCount();
                return View();
            }
            #endregion
        }
    }
}