using RentBook.Models.Author;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RentBook.Controllers
{
    public class AuthorController : Controller
    {
        // GET: Author
        public ActionResult List()
        {
            AuthorFactory factory = new AuthorFactory();

            return View(factory.SeleteAll());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateSave(AuthorModel a)
        {
            AuthorFactory factory = new AuthorFactory();
            a.a_id = factory.自動產生a_id();
            a.a_Birth = Request.Form["a_Birth"];

            int point = a.image.FileName.LastIndexOf(".");
            //副檔名
            string extention = a.image.FileName.Substring(point, a.image.FileName.Length - point);
            //這一行會幫我們在上傳到資料庫的時候 重新命名，例如 FB 的檔名那樣，16進制的編碼
            string photoName = Guid.NewGuid().ToString() + extention;
            //把 SaveAs 變成相對路徑的方法
            a.image.SaveAs(Server.MapPath("../作者照片/" + photoName));
            a.a_Image = "../作者照片/" + photoName;

            factory.Create(a);

            return RedirectToAction("Create");
        }

        public ActionResult Edit(string aid)
        {
            AuthorFactory factory = new AuthorFactory();

            if (aid == null)
            {
                return RedirectToAction("List");
            }

            return View(factory.要修改的作者資料(aid));
        }

        [HttpPost]
        public ActionResult EditSave(AuthorModel a)
        {
            AuthorFactory factory = new AuthorFactory();

            a.a_id = Request.Form["a_id"];
            a.a_Birth = Request.Form["a_Birth"];

            if (a.image != null)
            {
                string deleteresult = factory.刪除舊照片(a.a_id);

                if (deleteresult != "修改失敗")
                {
                    int point = a.image.FileName.LastIndexOf(".");
                    //副檔名
                    string extention = a.image.FileName.Substring(point, a.image.FileName.Length - point);
                    //這一行會幫我們在上傳到資料庫的時候 重新命名，例如 FB 的檔名那樣，16進制的編碼
                    string photoName = Guid.NewGuid().ToString() + extention;
                    //把 SaveAs 變成相對路徑的方法
                    a.image.SaveAs(Server.MapPath("../作者照片/" + photoName));
                    a.a_Image = "../作者照片/" + photoName;
                }
            }

            factory.Edit(a);

            return RedirectToAction("List");
        }
    }
}
