using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webSimple.Areas.Admin.Data;
using webSimple.Areas.Admin.Models;
using webSimple.Models;

namespace webSimple.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index(int page=1, int pageSize=4)
        {
            #region HorizontalBarChart- Lượt xem
            using (DbDataContext model = new DbDataContext())
            {
                var res = model.Relics.OrderByDescending(r=>r.ViewCount).ToPagedList(page, pageSize);
                return View(res);
            }
            #endregion
        }

    }
}