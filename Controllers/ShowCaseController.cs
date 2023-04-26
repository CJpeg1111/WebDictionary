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
    [AllowAnonymous]
    public class ShowCaseController : Controller
    {
        ContactManager cm = new ContactManager(new EfContactDal());
        ContactValidator validator = new ContactValidator();

        public ActionResult Home()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Home(Contact contact)
        {
            ValidationResult result = validator.Validate(contact);
            if (result.IsValid)
            {
                contact.ContactDate = DateTime.Now;
                cm.AddContact(contact);
                return RedirectToAction("Home");
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
                ViewBag.result = "true";
                return View();
            }
        }
    }
}