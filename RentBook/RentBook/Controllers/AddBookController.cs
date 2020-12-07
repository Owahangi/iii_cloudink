﻿using RentBook.Models.AddBook;
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
            EditBookModel ab = new EditBookModel();
            ab.出版社編號加名稱 = factory.傳回出版社編號名稱();
            ab.作者編號加名稱 = factory.傳回作者編號名稱();
            
            return View(ab);
        }

        /** 尚未加入後端資料驗證 **/


        // 將書籍基本資料除存到資料庫 / 將書籍封面存到實體路徑並命名
        [HttpPost]
        public string SaveNewBook(AddBooks b, AddBooksChapters bc, AddBooksFiles bf,AddBooksTags bt)
        {

            AddBookFactory factory = new AddBookFactory();

            // 書籍作者資料表
            AddBooksAuthor ba = new AddBooksAuthor();
            ba.b_id = factory.自動產生b_id();
            ba.AuthorIdName = Request.Form.GetValues("AuthorIdName");

            // 書籍大綱資料表
            bc.bc_Content = Request.Form["bcContent"];

            // 書籍資料表
            b.b_id = ba.b_id;

            // 書籍標籤資料表
            bt.Tags = factory.Tags轉成陣列(Request.Form["Tags"]);
            bt.b_id = b.b_id;

            string photoName = factory.書籍封面圖片命名(b);

            // 存到書籍資料表的 poco 類別
            b.b_Name = Request.Form["BookName"];
            b.b_Info = Request.Form["BookInfo"];
            b.b_Image = photoName;
            b.b_Type = Request.Form["Booktype"];
            b.b_PublishedDate = Convert.ToDateTime(Request.Form["PublishedDate"]);
            b.b_DatePrice = Convert.ToInt32(Request.Form["DatePrice"]);
            b.b_ISBN = Request.Form["ISBN"];
            b.b_AgeRating = Request.Form["AgeRange"];
            b.b_Series_yn = Request.Form["Series"];
            if (Request.Form["PublishedIdName"] != null)
            {
                b.PublishedIdName = Request.Form["PublishedIdName"];
                b.p_id = factory.出版社資料解析成編號(b.PublishedIdName);
            }

            string 書籍編號資料夾路徑 = Server.MapPath("../書籍素材/" + b.b_Type + "素材/" + ba.b_id);
            string 書籍章節資料夾路徑 = Server.MapPath("../書籍素材/" + b.b_Type + "素材/" + ba.b_id + "/" + ba.b_id + "-1");
            string 儲存書籍封面路徑 = Server.MapPath("../書籍素材/" + b.b_Type + "素材/" + b.b_id + "/" + photoName);

            // 新增小說 書籍編號資料夾
            if (!Directory.Exists(書籍編號資料夾路徑))
                Directory.CreateDirectory(書籍編號資料夾路徑);

            // 新增小說章節資料夾
            if (!Directory.Exists(書籍章節資料夾路徑))
                Directory.CreateDirectory(書籍章節資料夾路徑);

            // 儲存書籍封面到路徑
            b.Image.SaveAs(儲存書籍封面路徑);

            //----------------------------------------------------------------------------------

            // 儲存該章節的書籍檔案到路徑
            bc.bc_Chapters = 1;
            bf.FilesName = new List<string>();

            // 在儲存檔案時 自動更改檔名
            // 在上傳時需注意：
            // 1.請將每個檔案名稱 依照順序重新命名 例：1.txt、2.txt、3.txt .....
            // 2.確認要上傳的那個資料夾 Windows 的排序是依據 1.名稱 2.遞增
            if (bf.Files.Count() > 0)
            {
                int i = 1;
                foreach (HttpPostedFileBase uploadFile in bf.Files)
                {
                    if (uploadFile.ContentLength > 0)
                    {
                        // 這個陣列用來 將多筆章節檔名 儲存到資料庫使用
                        bf.FilesName.Add(b.b_id + "-1-" + i + factory.回傳書籍章節檔案副檔名(uploadFile));

                        // 上傳時自動編名
                        uploadFile.SaveAs(Server.MapPath("../書籍素材/" + b.b_Type + "素材/" + b.b_id + "/" + b.b_id + "-1/" + b.b_id + "-1-" + i + factory.回傳書籍章節檔案副檔名(uploadFile)));

                        // 維持原上傳檔名
                        // uploadFile.SaveAs(Server.MapPath("../書籍素材/" + b.b_Type + "素材/" + b.b_id + "/" + b.b_id + "-1/" + uploadFile.FileName));
                    }
                    i++;
                }
            }

            factory.儲存章節標題及檔名(b, bf, bc);
            factory.Create(b);
            factory.CreateBA(ba);
            factory.儲存到標籤資料表(bt);

            // 要帶到繼續新增章節的頁面使用
            string s = b.b_id + "|" + b.b_Type;

            return (s);
        }
    }
}