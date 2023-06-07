using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.Abstract;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebDictionary.Controllers
{
    [AllowAnonymous]
    public class DictionaryPanelContentController : Controller
    {
        CategoryManager cam = new CategoryManager(new EfCategoryDal());
        HeadingManager hm = new HeadingManager(new EfHeadingDal(), new EfCategoryDal());
        WriterManager wm = new WriterManager(new EfWriterDal());
        ContentManager cm = new ContentManager(new EfContentDal(), new EfHeadingDal(), new EfWriterDal());
        ContentValidator validator = new ContentValidator();

        [HttpGet]
        public ActionResult Index(int id)
        {
            ViewBag.headingid = id;
            var list = cm.GetListById(id);
            return View(list);
        }

        public ActionResult ContentLink(int id)
        {
            var content = cm.GetContent(id);
            return View(content);
        }

        public PartialViewResult addContentPartial(int id)
        {
            ViewBag.headingid = id;
            return PartialView();
        }

        [HttpPost]
        public PartialViewResult addContentPartial(Content content)
        {
            //var id = content.HeadingID;
            ValidationResult result = validator.Validate(content);
            if (result.IsValid)
            {
                string WriterMail = (string)Session["WriterMail"];
                var info = wm.GetWriterIdByMail(WriterMail);
                content.WriterID = info.WriterID;
                content.ContentDate = DateTime.Now;
                cm.AddContent(content);
            }
            else
            {               
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
                throw new Exception("This field cannot be left blank!");
            }

            return PartialView();
        }

        public ActionResult getInfobyWriter(int id)
        {
            int headingcount = hm.GetListByWriter(id).Count();
            int contentcount = cm.GetListByWriterId(id).Count;
            // int contentcount = cm.GetListByWriterId(id).Count;
            // 
            var categorycount = (from h in hm.GetList()
                         join c in cam.GetList() on h.CategoryID equals c.CategoryID
                         join ct in cm.GetListAll() on h.HeadingID equals ct.HeadingID
                         join w in wm.GetList() on ct.WriterID equals w.WriterID
                         where w.WriterID == id
                         select c.CategoryID).Distinct().Count();

            var dataList = new List<int> { headingcount, contentcount, categorycount };
            return Json(dataList, JsonRequestBehavior.AllowGet);
        }
    }
}