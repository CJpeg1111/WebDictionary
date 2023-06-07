using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Services.Description;

namespace WebDictionary.Controllers
{
    [AllowAnonymous]
    public class ShowCaseController : Controller
    {
        ContactManager cm = new ContactManager(new EfContactDal());
        ContactValidator validator = new ContactValidator();

        WriterManager wm = new WriterManager(new EfWriterDal());
        WriterValidator writervalidator = new WriterValidator();

        CategoryManager cam = new CategoryManager(new EfCategoryDal());
        HeadingManager hm = new HeadingManager(new EfHeadingDal(), new EfCategoryDal());
        ContentManager com = new ContentManager(new EfContentDal(), new EfHeadingDal(), new EfWriterDal());
        AboutManager am = new AboutManager(new EfAboutDal());
        SkillManager sm = new SkillManager(new EfSkillDal());
        ImageFileManager im = new ImageFileManager(new EfImageFileDal());
        public ActionResult Home()
        {
            ViewBag.category = cam.GetList().Count;
            ViewBag.heading = hm.GetList().Count;
            ViewBag.content = com.GetListAll().Count;
            ViewBag.writer = cam.GetListAll().Count;
            return View();
        }

        [HttpPost]
        public ActionResult Home(Contact contact, Writer writer, FormCollection form, HttpPostedFileBase imageFile)
        {
            if (form["btnInsert"] != null)
            {
                ValidationResult result = writervalidator.Validate(writer);
                if (result.IsValid)
                {
                    var existingUser = wm.GetWriterIdByMail(writer.WriterMail);
                    if (existingUser != null)
                    {
                        ModelState.AddModelError("WriterMail", "An account already exists with this email address!");
                        ViewBag.alert = "true";
                    }
                    else
                    {
                        byte[] passwordBytes = Encoding.UTF8.GetBytes(writer.WriterPassword);
                        using (SHA256 sha256 = SHA256.Create())
                        {
                            byte[] hashedPasswordBytes = sha256.ComputeHash(passwordBytes);
                            string hashedPassword = Convert.ToBase64String(hashedPasswordBytes);
                            writer.WriterPassword = hashedPassword;
                        }

                        if (imageFile != null && imageFile.ContentLength > 0)
                        {
                            byte[] imageData = new byte[imageFile.ContentLength];
                            imageFile.InputStream.Read(imageData, 0, imageFile.ContentLength);
                            writer.WriterImage = imageData;
                        }

                        wm.AddWriter(writer);

                        FormsAuthentication.SetAuthCookie(writer.WriterMail, false);
                        Session["WriterMail"] = writer.WriterMail;

                        return RedirectToAction("WriterProfile", "WriterPanel");
                    }
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                    }
                    ViewBag.alert = "true";
                }
            }
            else if (form["btnContact"] != null)
            {
                ValidationResult result = validator.Validate(contact);
                if (result.IsValid)
                {
                    contact.ContactDate = DateTime.Now;
                    cm.AddContact(contact);
                    ViewBag.send = "true";
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                    }
                    ViewBag.result = "true";
                }
            }

            ViewBag.category = cam.GetList().Count;
            ViewBag.heading = hm.GetList().Count;
            ViewBag.content = com.GetListAll().Count;
            ViewBag.writer = cam.GetListAll().Count;

            return View();
        }

        public PartialViewResult ContactPartial()
        {
            return PartialView();
        }

        public PartialViewResult AboutPartial()
        {
            var list = am.GetListActive();
            return PartialView(list);
        }

        public PartialViewResult SkillPartial()
        {
            var list = sm.GetListActive();
            return PartialView(list);
        }

        public PartialViewResult ImagePartial()
        {
            var list = im.GetListActive();
            return PartialView(list);
        }
    }
}