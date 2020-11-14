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

        public ActionResult ReadBookContent(string b_id,string c_FileName)
        {
            ReadBooksFactory factory = new ReadBooksFactory();
            List<string> fileContent = factory.ReadfileContent("111", "第一章 秦羽");
            
            ViewBag.BookContent = fileContent;

            return View();
        }

        public ActionResult ReadComicBookContent(string b_id,string c_FileName)
        {

            return View();
        }
    }
}