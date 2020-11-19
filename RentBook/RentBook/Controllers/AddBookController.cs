using RentBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace RentBook.Controllers
{
    public class AddBookController : Controller
    {
        // GET: AddBook
        // 新增 Book 的表單 Controller
        public ActionResult AddBook()
        {
            AddBookFactory factory = new AddBookFactory();
            ViewBag.pIdName = factory.傳回出版社編號名稱();
            ViewBag.aIdName = factory.傳回作者編號名稱();

            return View();
        }

        // 將書籍基本資料除存到資料庫 / 將書籍封面存到實體路徑並命名
        public ActionResult SaveAddBook(Books b)
        {
            AddBookFactory factory = new AddBookFactory();

            BooksAuthor ba = new BooksAuthor();
            ba.b_id = factory.自動產生b_id();
            ba.AuthorIdName = Request.Form.GetValues("AuthorIdName");
            

            // 自動建立書籍編號資料夾
            if (!Directory.Exists(Server.MapPath("../書籍素材/小說素材/" + ba.b_id)))
            {
                //新增資料夾
                Directory.CreateDirectory(Server.MapPath("../書籍素材/小說素材/" + ba.b_id));
            }

            b.b_id = ba.b_id;

            // 取得副檔名
            int point = b.Image.FileName.IndexOf(".");
            string extention = b.Image.FileName.Substring(point, b.Image.FileName.Length - point);
            // 命名封面檔名
            string photoName = b.b_id + "-cover" + extention;

            // 存到 poco 類別
            b.b_Name = Request.Form["BookName"];
            b.b_Info = Request.Form["BookInfo"];
            b.b_Image = photoName;
            b.b_Type = Request.Form["Booktype"];
            b.b_PublishedDate = Convert.ToDateTime(Request.Form["PublishedDate"]);
            b.b_HourPrice = Convert.ToInt32(Request.Form["HourPrice"]);
            b.b_ISBN = Request.Form["ISBN"];
            b.b_AgeRating = Convert.ToInt32(Request.Form["AgeRange"]);
            b.PublishedIdName = Request.Form["PublishedIdName"];
            b.p_id = factory.出版社資料解析成編號(b.PublishedIdName);

            // 儲存圖片到路徑
            b.Image.SaveAs(Server.MapPath("../書籍素材/小說素材/" + b.b_id + "/" + photoName));

            factory.Create(b);
            factory.CreateBA(ba);

            return RedirectToAction("AddBook");
        }

       

        // 新增大綱 大標題、子標題
        public ActionResult AddBookOutline()
        {
            return View();
        }

        // 新增書籍內榮
        public ActionResult AddBookOutlineSubHeaders()
        {
            return View();
        }
    }
}