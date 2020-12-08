using RentBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace RentBook.Controllers
{
    public class ReadBooksController : Controller
    {
        RentBookdbEntities2 db = new RentBookdbEntities2();
        // GET: ReadBooks
        public ActionResult List()
        {
            return View();
        }

        // 讀取小說內容
        public ActionResult ReadBookContent(string b_id, int bc_Chapters)
        {
            ReadBooksModel rb = new ReadBooksModel();
            rb.b_id = b_id;
            rb.bc_Chapters = bc_Chapters;

            ReadBooksFactory factory = new ReadBooksFactory();
            rb.小說書籍內容 = factory.ReadfileContent(rb);
            rb.傳回書籍最大章節數 = factory.回傳書籍最大章節數量(b_id);
            rb.傳回書籍章節標題 = factory.傳回目前章節標題(b_id, bc_Chapters);


            return View(rb);

        }

        // 讀取漫畫內容
        public ActionResult ReadComicBookContent(string b_id, int bc_Chapters)
        {

            ReadBooksFactory factory = new ReadBooksFactory();
            ReadBooksModel rb = new ReadBooksModel();
            rb.b_id = b_id;
            rb.bc_Chapters = bc_Chapters;
            rb.傳回書籍最大章節數 = factory.回傳書籍最大章節數量(b_id);
            rb.傳回書籍章節標題 = factory.傳回目前章節標題(b_id, bc_Chapters);

            rb.FilesName = factory.ReadComicBookfileContent(rb);

            //string 路徑 = System.Web.HttpContext.Current.Server.MapPath("~/書籍素材/漫畫素材/" + b_id + "/" + b_id + "-" + chapters + "/");
            rb.FilePath = "../../書籍素材/漫畫素材/" + rb.b_id + "/" + rb.b_id + "-" + rb.bc_Chapters + "/";

            return View(rb);


        }
    }
}