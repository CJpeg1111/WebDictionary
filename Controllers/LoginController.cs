using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace WebDictionary.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        AdminManager am = new AdminManager(new EfAdminDal());
        WriterManager wm = new WriterManager(new EfWriterDal());

        public class CaptchaResponse
        {
            [JsonProperty("success")]
            public bool Success { get; set; }

            [JsonProperty("error-codes")]
            public List<string> ErrorCodes { get; set; }
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string AdminUserName, string AdminPassword)
        {
            var response = Request["g-recaptcha-response"];
            const string secret = "6LfotoglAAAAAPIBvMHQ48rtJF5x-IfSwXHdzWUs";

            var client = new WebClient();
            var reply = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, response));

            var captchaResponse = JsonConvert.DeserializeObject<CaptchaResponse>(reply);

            string act = string.Empty;
            string cont = string.Empty;
            if (!captchaResponse.Success)
            {
                TempData["Message"] = "Lütfen güvenliği doğrulayınız.";
                act = "Index";
                cont = "Login";
            }
            else
            {
                var admin = am.ControlAdmin(AdminUserName, AdminPassword);
                if (admin != null)
                {
                    FormsAuthentication.SetAuthCookie(admin.AdminUserName, false);
                    Session["AdminUserName"] = admin.AdminUserName;
                    act = "Index";
                    cont = "AdminCategory";
                }
                else
                {
                    TempData["validate"] = "Please make sure your username or password is correct!";
                    act = "Index";
                    cont = "Login";
                }                
            }
            return RedirectToAction(act, cont); 
        }

        [HttpGet]
        public ActionResult WriterLogin()
        {
            return View();
        }
       

        [HttpPost]
        public ActionResult WriterLogin(string WriterMail, string WriterPassword)
        {
            var response = Request["g-recaptcha-response"];
            const string secret = "6LfotoglAAAAAPIBvMHQ48rtJF5x-IfSwXHdzWUs";

            var client = new WebClient();
            var reply = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, response));

            var captchaResponse = JsonConvert.DeserializeObject<CaptchaResponse>(reply);

            string act = string.Empty;
            string cont = string.Empty;
            if (!captchaResponse.Success)
            {
                TempData["Message"] = "Lütfen güvenliği doğrulayınız.";
            }
            else
            {              
                var writer = wm.ControlWriter(WriterMail, WriterPassword);
                if (writer != null)
                {
                    FormsAuthentication.SetAuthCookie(writer.WriterMail, false);
                    Session["WriterMail"] = writer.WriterMail;
                    act = "WriterProfile";
                    cont = "WriterPanel";
                }
                else
                {
                    TempData["validate"] = "Please make sure your username or password is correct!";
                    act = "WriterLogin";
                    cont = "Login";
                }                
            }
            return RedirectToAction(act, cont);
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Home", "ShowCase");
        }
    }
}