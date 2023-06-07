using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebDictionary.Controllers
{
    public class WriterController : Controller
    {
        WriterManager wm = new WriterManager(new EfWriterDal());

        public ActionResult Index()
        {
            var list = wm.GetList();
            return View(list);
        }

        [Authorize(Roles = "A")]
        public ActionResult Delete(int id)
        {
            var writer = wm.GetWriter(id);
            if (writer.WriterRemove == true)
            {
                writer.WriterRemove = false;
            }
            else
            {
                writer.WriterRemove = true;
            }
            wm.UpdateWriter(writer);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "A")]
        public ActionResult WriterReport()
        {
            var writerlist = wm.GetList();
            return View(writerlist);
        }

        public ActionResult GetImage(int id)
        {
            var writer = wm.GetWriter(id);
            return File(writer.WriterImage, "image/jpeg"); // Görüntüyü byte dizisi olarak döndürme
        }

        [HttpGet]
        public JsonResult Show(int id)
        {
            var writer = wm.GetWriter(id);
            return Json(writer, JsonRequestBehavior.AllowGet);
        }
    }
}