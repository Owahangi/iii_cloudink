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

        // GET: ReadBooks
        public ActionResult List()
        {
            ReadBooksFactory factory = new ReadBooksFactory();
            string fileName =  factory.Readfilename("111");

            BooksChapters chapter = new BooksChapters();
            chapter.c_FileName = fileName;

            return View(chapter);
        }

        // 讀取小說內容
        public ActionResult ReadBookContent(string b_id,string c_FileName)
        {
            ReadBooksFactory factory = new ReadBooksFactory();
            List<string> fileContent = factory.ReadfileContent("111", "第一章 秦羽");
            
            ViewBag.BookContent = fileContent;

            return View();
        }

        // 讀取漫畫內容
        public ActionResult ReadComicBookContent(string b_id,int? chapters)
        {
            //string chaptersfoldername = b_id + "-" + chapters.ToString();

            chapters = 1;

            ReadBooksFactory factory = new ReadBooksFactory();
            List<string> 章節檔名 = factory.ReadComicBookfileContent("123", 1);

            //System.Web.HttpContext.Current.Server.MapPath("~/書籍素材/小說素材/" + bookname + "/" + chapters + ".txt"), System.Text.Encoding.UTF8)
            //ViewBag.Path = b_id + @"/" + b_id + "-" + chapters + @"/";
            //string 路徑 = System.Web.HttpContext.Current.Server.MapPath("~/書籍素材/漫畫素材/" + b_id + @"/" + b_id + "-" + chapters);
            string 路徑 = "../../書籍素材/漫畫素材/123/123-1";
            ViewBag.Path = 路徑;
            ViewBag.ComicBook = 章節檔名;
            return View();
        }
    }
}