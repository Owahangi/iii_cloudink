using RentBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RentBook.Controllers
{
    public class AddBookController : Controller
    {
        // GET: AddBook
        public ActionResult AddBook()
        {
            return View();
        }

        // 儲存書籍基本資料
        public ActionResult SaveAddBook()
        {
            BooksAuthor ba = new BooksAuthor();
            ba.b_id = Convert.ToInt32(Request.Form["BookName"]);
            ba.a_id = Convert.ToInt32(Request.Form["AuthorName"]);

            Books b = new Books();
            b.b_Name = Request.Form["BookName"];
            b.b_Info = Request.Form["BookInfo"];
            b.b_Image = Request.Form["BookImage"];
            b.b_Type = Request.Form["Booktype"];
            b.b_PublishedDate = Convert.ToDateTime(Request.Form["PublishedDate"]);
            b.b_HourPrice = Convert.ToInt32(Request.Form["HourPrice"]);
            b.b_ISBN = Request.Form["ISBN"];
            b.b_AgeRating = Convert.ToInt32(Request.Form["AgeRange"]);
            b.p_id = Convert.ToInt32(Request.Form["PublishedId"]);


            AddBookFactory factory = new AddBookFactory();
            factory.Create(b,ba);

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