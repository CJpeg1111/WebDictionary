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
    public class WriterPanelController : Controller
    {
        HeadingManager hm = new HeadingManager(new EfHeadingDal());
        HeadingValidator validator = new HeadingValidator();
        CategoryManager cm = new CategoryManager(new EfCategoryDal());
        WriterManager wm = new WriterManager(new EfWriterDal());
        WriterValidator wvalidator = new WriterValidator();

        [HttpGet]
        public ActionResult WriterProfile()
        {
            string WriterMail = (string)Session["WriterMail"];
            var info = wm.GetWriterIdByMail(WriterMail);
            int id = info.WriterID;
            var writer = wm.GetWriter(id);
            return View(writer);
        }

        [HttpPost]
        public ActionResult WriterProfile(Writer writer)
        {
            ValidationResult results = wvalidator.Validate(writer);
            if (results.IsValid)
            {
                wm.UpdateWriter(writer);
                return RedirectToAction("WriterProfile");
            }
            else
            {
                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
                return View();
            }
        }

        public ActionResult MyHeading()
        {
            string WriterMail = (string)Session["WriterMail"];
            var info = wm.GetWriterIdByMail(WriterMail);
            int id = info.WriterID;
            var headings = hm.GetListByWriter(id);
            return View(headings);
        }

        [HttpGet]
        public ActionResult addHeading()
        {
            List<SelectListItem> categoryList = (from x in cm.GetList()
                                                 select new SelectListItem
                                                 {
                                                     Text = x.CategoryName,
                                                     Value = x.CategoryID.ToString()
                                                 }).ToList();
            ViewBag.ctglist = categoryList;
            return View();
        }

        [HttpPost]
        public ActionResult addHeading(Heading heading)
        {
            heading.HeadingDate = DateTime.Now;
            string WriterMail = (string)Session["WriterMail"];
            var info = wm.GetWriterIdByMail(WriterMail);
            int id = info.WriterID;
            heading.WriterID = id;
            ValidationResult result = validator.Validate(heading);
            if (result.IsValid)
            {
                hm.AddHeading(heading);
                return RedirectToAction("MyHeading");
            }
            else
            {
                List<SelectListItem> categoryList = (from x in cm.GetList()
                                                     select new SelectListItem
                                                     {
                                                         Text = x.CategoryName,
                                                         Value = x.CategoryID.ToString()
                                                     }).ToList();
                ViewBag.ctglist = categoryList;

                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
                return View();
            }
        }

        [HttpGet]
        public ActionResult updateHeading(int id)
        {
            List<SelectListItem> categoryList = (from x in cm.GetList()
                                                 select new SelectListItem
                                                 {
                                                     Text = x.CategoryName,
                                                     Value = x.CategoryID.ToString()
                                                 }).ToList();
            ViewBag.ctglist = categoryList;

            var hdg = hm.GetHeading(id);
            return View(hdg);
        }

        [HttpPost]
        public ActionResult updateHeading(Heading heading)
        {
            ValidationResult result = validator.Validate(heading);
            if (result.IsValid)
            {
                hm.UpdateHeading(heading);
                return RedirectToAction("MyHeading");
            }
            else
            {
                List<SelectListItem> categoryList = (from x in cm.GetList()
                                                     select new SelectListItem
                                                     {
                                                         Text = x.CategoryName,
                                                         Value = x.CategoryID.ToString()
                                                     }).ToList();
                ViewBag.ctglist = categoryList;

                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            var heading = hm.GetHeading(id);
            heading.HeadingRemove = true;
            hm.DeleteHeading(heading);
            return RedirectToAction("MyHeading");
        }

        public ActionResult AllHeading()
        {
            var list = hm.GetList();
            return View(list);
        }
    }
}