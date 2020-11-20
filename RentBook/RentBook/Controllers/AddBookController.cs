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
        [HttpPost]
        public ActionResult SaveNewBook(Books b,BookOutline bo,BooksChapters bc)
        {           

            AddBookFactory factory = new AddBookFactory();

            // 書籍作者資料表
            BooksAuthor ba = new BooksAuthor();
            ba.b_id = factory.自動產生b_id();
            ba.AuthorIdName = Request.Form.GetValues("AuthorIdName");

            // 書籍大綱資料表
            bo.boh_Content = Request.Form["BookOutlineContent"];

            // 書籍資料表
            b.b_id = ba.b_id;

            string photoName = factory.取得圖片附檔名(b);

            if (Request.Form["Booktype"] == "小說")
            {
                // 自動建立書籍編號資料夾
                if (!Directory.Exists(Server.MapPath("../書籍素材/小說素材/" + ba.b_id)))
                {
                    //新增資料夾
                    Directory.CreateDirectory(Server.MapPath("../書籍素材/小說素材/" + ba.b_id));
                }

                // 儲存圖片到路徑
                b.Image.SaveAs(Server.MapPath("../書籍素材/小說素材/" + b.b_id + "/" + photoName));

            } else if(Request.Form["Booktype"] == "漫畫")
            {
                // 自動建立書籍編號資料夾
                if (!Directory.Exists(Server.MapPath("../書籍素材/漫畫素材/" + ba.b_id)))
                {
                    //新增資料夾
                    Directory.CreateDirectory(Server.MapPath("../書籍素材/漫畫素材/" + ba.b_id));
                }
                // 儲存圖片到路徑
                b.Image.SaveAs(Server.MapPath("../書籍素材/漫畫素材/" + b.b_id + "/" + photoName));
            }

            // 存到書籍資料表的 poco 類別
            b.b_Name = Request.Form["BookName"];
            b.b_Info = Request.Form["BookInfo"];
            b.b_Image = photoName;
            b.b_Type = Request.Form["Booktype"];
            b.b_PublishedDate = Convert.ToDateTime(Request.Form["PublishedDate"]);
            b.b_HourPrice = Convert.ToInt32(Request.Form["HourPrice"]);
            b.b_ISBN = Request.Form["ISBN"];
            b.b_AgeRating = Convert.ToInt32(Request.Form["AgeRange"]);
            b.b_Series_yn = Request.Form["Series"];
            if (Request.Form["PublishedIdName"] != null)
            {
                b.PublishedIdName = Request.Form["PublishedIdName"];
                b.p_id = factory.出版社資料解析成編號(b.PublishedIdName);
            }

            //----------------------------------------------------------------------------------
            

            // 建立章節資料夾



            // 上傳各章節的書籍檔案
            if (bc.Files.Count() > 0)
            {
                int i = 1;

                foreach (HttpPostedFileBase uploadFile in bc.Files)
                {
                    if (uploadFile.ContentLength > 0)
                    {
                        // 命名章節檔名
                        string ChapterName = b.b_id + "-" + i + ".txt";

                        uploadFile.SaveAs(Server.MapPath("../書籍素材/小說素材/" + b.b_id + "/" + ChapterName));

                        i++;
                    }
                }
            }

            factory.儲存章節標題及檔名(b,bc,bo);
            factory.Create(b);
            factory.CreateBA(ba);

            return RedirectToAction("AddBook");
        }

        // 顯示要建立的章節
        public ActionResult CreateChapter()
        {
            AddBookFactory factory = new AddBookFactory();
            //ViewBag.pIdName = factory.目前最新章節();
           

            return View();
        }

        // 新增書籍內容
        public ActionResult AddBookOutlineSubHeaders()
        {
            return View();
        }
    }
}