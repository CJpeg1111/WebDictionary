using BusinessLayer.Concrete;
using DataAccessLayer.Abstract;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebDictionary.Controllers
{
    public class ContentController : Controller
    {
        ContentManager cm = new ContentManager(new EfContentDal(), new EfHeadingDal(), new EfWriterDal());
        HeadingManager hm = new HeadingManager(new EfHeadingDal(), new EfCategoryDal());

        public ActionResult Index()
        {
            var values = cm.GetListAll();
            return View(values);
        }

        public ActionResult ContentByHeading(int id)
        {
            var list = cm.GetListById(id);
            var heading = hm.GetHeading(id);
            ViewBag.heading = heading.HeadingName;
            return View(list);
        }

        public ActionResult Delete(int id)
        {
            var content = cm.GetContent(id);
            if (content.ContentRemove == true)
            {
                content.ContentRemove = false;
            }
            else
            {
                content.ContentRemove = true;
            }
            cm.UpdateContent(content);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult Show(int id)
        {
            var content = cm.GetContentInclude(id);
            List<object> list;
            if (content.Writer.WriterImage != null)
            {
                var img = File(content.Writer.WriterImage, "image/jpeg");
                list = new List<object> {
                content.ContentID,
                content.ContentCaption,
                content.ContentDate,
                content.Heading.HeadingName,
                content.Writer.WriterName,
                content.Writer.WriterSurname,
                img.FileContents,
                content.ContentRemove};
            }
            else
            {
                list = new List<object> {
                content.ContentID,
                content.ContentCaption,
                content.ContentDate,
                content.Heading.HeadingName,
                content.Writer.WriterName,
                content.Writer.WriterSurname,
                content.Writer.WriterImage,
                content.ContentRemove};
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "A")]
        public ActionResult ContentReport()
        {
            var values = cm.GetListAll();
            return View(values);
        }
    }
}