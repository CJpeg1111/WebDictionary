using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace WebDictionary.Controllers
{
    public class WriterPanelController : Controller
    {
        HeadingManager hm = new HeadingManager(new EfHeadingDal(), new EfCategoryDal());
        HeadingValidator validator = new HeadingValidator();
        CategoryManager cm = new CategoryManager(new EfCategoryDal());
        WriterManager wm = new WriterManager(new EfWriterDal());
        WriterValidator wvalidator = new WriterValidator();

        MessageManager mm = new MessageManager(new EfMessageDal());
        DraftMessageManager dmm = new DraftMessageManager(new EfDraftMessageDal());

        [HttpGet]
        public ActionResult WriterProfile()
        {
            string WriterMail = (string)Session["WriterMail"];
            var writer = wm.GetWriterIdByMail(WriterMail);
            return View(writer);
        }

        [HttpPost]
        public ActionResult WriterProfile(Writer writer, HttpPostedFileBase imageFile, string NewWriterPassword)
        {
            ValidationResult results = wvalidator.Validate(writer);
            if (results.IsValid)
            {
                var firstWriter = wm.GetWriter(writer.WriterID);

                firstWriter.WriterID = writer.WriterID;
                firstWriter.WriterName = writer.WriterName;
                firstWriter.WriterSurname = writer.WriterSurname;
                firstWriter.WriterTitle = writer.WriterTitle;
                firstWriter.WriterAbout = writer.WriterAbout;

                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    byte[] imageData = new byte[imageFile.ContentLength];
                    imageFile.InputStream.Read(imageData, 0, imageFile.ContentLength);
                    firstWriter.WriterImage = imageData;
                }

                if (NewWriterPassword != "")
                {
                    if (NewWriterPassword.Length < 6)
                    {
                        TempData["validate"] = "Password cannot be shorter than 6 characters!";
                        return View();
                    }
                    if (NewWriterPassword.Length > 50)
                    {
                        TempData["validate"] = "Password cannot exceed 50 characters!";
                        return View();
                    }
                    byte[] passwordBytes = Encoding.UTF8.GetBytes(NewWriterPassword);
                    using (SHA256 sha256 = SHA256.Create())
                    {
                        byte[] hashedPasswordBytes = sha256.ComputeHash(passwordBytes);
                        string hashedPassword = Convert.ToBase64String(hashedPasswordBytes);
                        firstWriter.WriterPassword = hashedPassword;
                    }
                }

                if (firstWriter.WriterMail != writer.WriterMail)
                {
                    var existingUser = wm.GetWriterIdByMail(writer.WriterMail);
                    if (existingUser != null)
                    {
                        ModelState.AddModelError("WriterMail", "This account is already in use by someone else!");
                    }
                    else
                    {
                        var recordsToReceiver = mm.GetListReceiver(firstWriter.WriterMail);
                        var recordsToSender = mm.GetListSender(firstWriter.WriterMail);
                        var recordsTodraftReceiver = dmm.GetListReceiver(firstWriter.WriterMail);
                        var recordsTodraftSender = dmm.GetListSender(firstWriter.WriterMail);
                        foreach (var record in recordsToReceiver)
                        {
                            record.ReceiverMail = writer.WriterMail;
                            mm.UpdateMessage(record);
                        }
                        foreach (var record in recordsToSender)
                        {
                            record.SenderMail = writer.WriterMail;
                            mm.UpdateMessage(record);
                        }
                        foreach (var record in recordsTodraftReceiver)
                        {
                            record.DraftReceiverMail = writer.WriterMail;
                            dmm.UpdateDraftMessage(record);
                        }
                        foreach (var record in recordsTodraftSender)
                        {
                            record.DraftSenderMail = writer.WriterMail;
                            dmm.UpdateDraftMessage(record);
                        }

                        firstWriter.WriterMail = writer.WriterMail;
                        wm.UpdateWriter(firstWriter);
                        FormsAuthentication.SignOut();
                        Session.Abandon();
                        return RedirectToAction("WriterLogin", "Login");
                    }
                }
                else
                {
                    firstWriter.WriterMail = writer.WriterMail;
                    wm.UpdateWriter(firstWriter);
                    ViewBag.updateresult = "true";
                }

                return View();
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
        public ActionResult addHeading(Heading heading, HttpPostedFileBase imageFile)
        {
            heading.HeadingDate = DateTime.Now;
            string WriterMail = (string)Session["WriterMail"];
            var info = wm.GetWriterIdByMail(WriterMail);
            heading.WriterID = info.WriterID;
            ValidationResult result = validator.Validate(heading);
            if (result.IsValid)
            {
                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    byte[] imageData = new byte[imageFile.ContentLength];
                    imageFile.InputStream.Read(imageData, 0, imageFile.ContentLength);
                    heading.HeadingImage = imageData;
                }

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
        public ActionResult updateHeading(Heading heading, HttpPostedFileBase imageFile)
        {
            ValidationResult result = validator.Validate(heading);
            if (result.IsValid)
            {
                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    byte[] imageData = new byte[imageFile.ContentLength];
                    imageFile.InputStream.Read(imageData, 0, imageFile.ContentLength);
                    heading.HeadingImage = imageData;
                }

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
            if (heading.HeadingRemove == true)
            {
                heading.HeadingRemove = false;
            }
            else
            {
                heading.HeadingRemove = true;
            }
            hm.DeleteHeading(heading);
            return RedirectToAction("MyHeading");
        }

        public ActionResult AllHeading()
        {
            var list = hm.GetListActive();
            return View(list);
        }

        public PartialViewResult userCard()
        {
            string WriterMail = (string)Session["WriterMail"];
            var writer = wm.GetWriterIdByMail(WriterMail);
            return PartialView(writer);
        }
    }
}