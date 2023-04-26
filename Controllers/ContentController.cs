using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebDictionary.Controllers
{
    public class ContentController : Controller
    {
        ContentManager cm = new ContentManager(new EfContentDal());

        public ActionResult Index()
        {
            var values = cm.GetListAll();        
            return View(values);
        }

        public ActionResult ContentByHeading(int id)
        {
            var list = cm.GetListById(id);
            return View(list);
        }
    }
}