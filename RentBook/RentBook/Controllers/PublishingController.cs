using RentBook.Models;
using RentBook.Models.Publishing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RentBook.Controllers
{
    public class PublishingController : Controller
    {
        // GET: Publishing
        public ActionResult List()
        {
            string keyword = Request.Form["txtKeyword"];

            PublishingFactory factory = new PublishingFactory();

            List<PublishingModel> list = null;

            if (string.IsNullOrEmpty(keyword))
            {
                list = (new PublishingFactory()).SeleteAll();
            }
            else
            {
                list = (new PublishingFactory()).getByKeyword(keyword);
            }
            return View(list);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateSave(PublishingModel p)
        {
            PublishingFactory factory = new PublishingFactory();
            p.p_id = factory.自動產生p_id();
            
            factory.Create(p);

            return RedirectToAction("List");
        }

        public ActionResult Edit(string pid)
        {
            PublishingFactory factory = new PublishingFactory();

            if (pid == null)
            {
                return RedirectToAction("List");
            }

            return View(factory.要修改的出版社資料(pid));
        }

        [HttpPost]
        public ActionResult EditSave(PublishingModel p)
        {
            PublishingFactory factory = new PublishingFactory();
            
            factory.Edit(p);

            return RedirectToAction("List");
        }
    }
}