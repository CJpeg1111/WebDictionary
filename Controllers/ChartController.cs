using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer;

namespace WebDictionary.Controllers
{
    public class ChartController : Controller
    {
        HeadingManager hm = new HeadingManager(new EfHeadingDal());
        // GET: Chart
        public ActionResult CategoryHeading()
        {
            var rs = hm.ListCategoryHeading();
            return View(rs);
        }

        public ActionResult CategoryHeadingChart()
        {
            return Json(hm.ListCategoryHeading(),JsonRequestBehavior.AllowGet);
        }
    }
}