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
    public class DictionaryPanelWriterController : Controller
    {
        WriterManager wm = new WriterManager(new EfWriterDal());
        //HeadingManager hm = new HeadingManager(new EfHeadingDal(), new EfCategoryDal());
        //ContentManager cm = new ContentManager(new EfContentDal(), new EfHeadingDal(), new EfWriterDal());
        // GET: DictionaryPanelWriter
        public ActionResult Index(int id)
        {
            var writer = wm.GetWriter(id);
            return View(writer);
        }
    }
}