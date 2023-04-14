using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebDictionary.Controllers
{
    [AllowAnonymous]
    public class HomePageController : Controller
    {
        HeadingManager hm = new HeadingManager(new EfHeadingDal());
        ContentManager cm = new ContentManager(new EfContentDal());

        public ActionResult Headings()
        {
            var list = hm.GetList();
            return View(list);
        }

        public PartialViewResult Contents(int id = 1)
        {
            var heading = hm.GetHeading(id);
            ViewBag.heading = heading.HeadingName;
            var list = cm.GetListById(id);
            return PartialView(list);
        }
    }
}