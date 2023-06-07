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
using System.Web.UI.WebControls;

namespace WebDictionary.Controllers
{
    [AllowAnonymous]
    public class DictionaryPanelController : Controller
    {
        HeadingManager hm = new HeadingManager(new EfHeadingDal(), new EfCategoryDal());
        WriterManager wm = new WriterManager(new EfWriterDal());
        WriterValidator writervalidator = new WriterValidator();
        // GET: DictionaryPanel
        public ActionResult Index()
        {
            //var list = hm.GetList();
            return View();
        }

        public ActionResult GetImageHeading(int id)
        {
            var heading = hm.GetHeading(id);
            return File(heading.HeadingImage, "image/jpeg"); // Görüntüyü byte dizisi olarak döndürme
        }

        public ActionResult GetImageWriter(int id)
        {
            var writer = wm.GetWriter(id);
            return File(writer.WriterImage, "image/jpeg"); // Görüntüyü byte dizisi olarak döndürme
        }

        [HttpGet]
        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Signup(Writer writer, HttpPostedFileBase imageFile, bool termsCheckbox = false)
        {
            if (termsCheckbox == true)
            {
                ValidationResult result = writervalidator.Validate(writer);
                if (result.IsValid)
                {
                    var existingUser = wm.GetWriterIdByMail(writer.WriterMail);
                    if (existingUser != null)
                    {
                        ModelState.AddModelError("WriterMail", "An account already exists with this email address!");
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

                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                    }
                }
            }
            else
            {
                ViewBag.terms = true;
            }

            return View();
        }

        public ActionResult TermsAndService()
        {
            return View();
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index");
        }

    }
}