using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebDictionary.Controllers
{
    [AllowAnonymous]
    public class DictionaryPanelHeadingController : Controller
    {
        HeadingManager hm = new HeadingManager(new EfHeadingDal(), new EfCategoryDal());
        WriterManager wm = new WriterManager(new EfWriterDal());
        ContentManager cm = new ContentManager(new EfContentDal(), new EfHeadingDal(), new EfWriterDal());
        ContentValidator validator = new ContentValidator();

        public ActionResult Index(int id)
        {
            ViewBag.categoryid = id;
            var list = hm.GetListByCategory(id);
            return View(list);
        }
    }
}