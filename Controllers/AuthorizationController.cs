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

namespace WebDictionary.Controllers
{
    public class AuthorizationController : Controller
    {
        AdminManager am = new AdminManager(new EfAdminDal());
        AdminValidator validator = new AdminValidator();

        public ActionResult Index()
        {
            var admin = am.GetList();
            return View(admin);
        }

        [HttpGet]
        public ActionResult addAdmin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult addAdmin(Admin admin)
        {
            ValidationResult result = validator.Validate(admin);
            if (result.IsValid)
            {
                am.AddAdmin(admin);
                ViewBag.alert = "true";
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }
            return View();
        }


        [HttpGet]
        public ActionResult updateAdmin(int id)
        {
            var admin = am.GetAdmin(id);
            return View(admin);
        }

        [HttpPost]
        public ActionResult updateAdmin(Admin admin)
        {
            ValidationResult result = validator.Validate(admin);
            if (result.IsValid)
            {
                am.UpdateAdmin(admin);
                ViewBag.alert = "true";
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }
            return View();
        }
    }
}