using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.Abstract;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebDictionary.Controllers
{
    public class AboutController : Controller
    {
        AboutManager am = new AboutManager(new EfAboutDal());
        AboutValidator validator = new AboutValidator();

        public ActionResult Index()
        {
            var list = am.GetList();
            return View(list);
        }

        [HttpGet]
        public ActionResult addAbout()
        {
            return View();
        }

        [HttpPost]
        public ActionResult addAbout(About about)
        {
            ValidationResult result = validator.Validate(about);
            if (result.IsValid)
            {
                am.AddAbout(about);
                ViewBag.okay=true;
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
                ViewBag.okay = false;
                return RedirectToAction("Index",about);
            }            
        }

        public PartialViewResult AboutPartial()
        {
            return PartialView();
        }

        public ActionResult Delete(int id)
        {
            var find=am.GetAbout(id);
            if (find.AboutRemove == true)
            {
                find.AboutRemove = false;
            }
            else
            {
                find.AboutRemove = true;
            }
            am.UpdateAbout(find);
            return RedirectToAction("Index");
        }
    }
}