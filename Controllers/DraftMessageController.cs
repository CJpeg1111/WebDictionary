using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace WebDictionary.Controllers
{
    public class DraftMessageController : Controller
    {
        DraftMessageManager dmm = new DraftMessageManager(new EfDraftMessageDal());
        MessageManager mm = new MessageManager(new EfMessageDal());
        DraftMessageValidator dvalidator = new DraftMessageValidator();

        public ActionResult draftMessage()
        {
            string SenderMail = (string)Session["AdminUserName"];
            var list = dmm.GetList(SenderMail);
            return View(list);
        }

        public ActionResult draftMessageDelete(int id)
        {
            var draft = dmm.GetDraftMessage(id);
            dmm.DeleteDraftMessage(draft);
            return RedirectToAction("draftMessage");
        }

        [HttpGet]
        public ActionResult updateDraft(int id)
        {
            var draft = dmm.GetDraftMessage(id);
            return View(draft);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult updateDraft(DraftMessage draftMessage, Message message, FormCollection form)
        {
            ValidationResult result = dvalidator.Validate(draftMessage);
            if (result.IsValid)
            {
                string action = string.Empty;
                string cont = string.Empty;
                if (form["btnDraft"] != null)
                {
                    dmm.UpdateDraftMessage(draftMessage);
                    action = "draftMessage";
                    cont = "DraftMessage";
                }
                else if (form["btnMessage"] != null)
                {
                    string SenderMail = form["DraftSenderMail"];
                    string ReceiverMail = form["DraftReceiverMail"];
                    string Subject = form["DraftSubject"];
                    string MessageContent = form["DraftMessageContent"];
                    DateTime Date = DateTime.Now;

                    message.ReceiverMail = ReceiverMail;
                    message.SenderMail = SenderMail;
                    message.Subject = Subject;
                    message.MessageContent = MessageContent;
                    message.MessageDate = Date;
                    mm.AddMessage(message);

                    dmm.DeleteDraftMessage(draftMessage);

                    action = "Sendbox";
                    cont = "Message";
                }
                return RedirectToAction(action, cont);
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
    }
}