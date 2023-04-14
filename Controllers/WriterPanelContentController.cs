using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace WebDictionary.Controllers
{
    public class WriterPanelContentController : Controller
    {
        ContentManager cm = new ContentManager(new EfContentDal());
        ContentValidator validator = new ContentValidator();
        WriterManager wm = new WriterManager(new EfWriterDal());
        HeadingManager hm = new HeadingManager(new EfHeadingDal());

        public ActionResult MyContent()
        {
            string WriterMail = (string)Session["WriterMail"];
            var info = wm.GetWriterIdByMail(WriterMail);
            int id = info.WriterID;
            var list = cm.GetListByWriterId(id);
            return View(list);
        }

        [HttpGet]
        public ActionResult addContent(int id)
        {
            ViewBag.id = id;
            var head = hm.GetHeading(id);
            ViewBag.heading = head.HeadingName;
            return View();
        }

        [HttpPost]
        public ActionResult addContent(Content content)
        {
            ValidationResult result = validator.Validate(content);
            if (result.IsValid)
            {
                string WriterMail = (string)Session["WriterMail"];
                var info = wm.GetWriterIdByMail(WriterMail);
                int id = info.WriterID;
                content.WriterID = id;
                content.ContentDate = DateTime.Now;
                cm.AddContent(content);
                return RedirectToAction("MyContent");
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            var content=cm.GetContent(id);
            content.ContentRemove = true;
            cm.UpdateContent(content);
            return RedirectToAction("MyContent");
        }
    }
}