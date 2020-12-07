using RentBook.Models.EditBook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RentBook.Controllers
{
    public class EditBookController : Controller
    {
        // GET: EditBook
        public ActionResult List()
        {
            string keyword = Request.Form["txtKeyword"];

            EditBookFactory factory = new EditBookFactory();

            List<EditBookModel> list = null;

            if (string.IsNullOrEmpty(keyword))
            {
                list = factory.列出所有書籍資訊();
            }
            else
            {
                list = factory.getByKeyword(keyword);
            }
            return View(list);
        }

        // 要編輯的書籍的 <form> 表單
        public ActionResult EditBookData(string b_id)
        {
            if (b_id != null)
            {
                EditBookFactory factory = new EditBookFactory();

                EditBookModel eb = factory.帶出要修改的書籍資訊(b_id);
                eb.Tags字串 = factory.列出書籍Tags(b_id);
                eb.出版社編號加名稱列表 = factory.傳回出版社編號名稱列表();
                eb.作者編號加名稱列表 = factory.傳回作者編號名稱列表();
                eb.列出本書的作者 = factory.回傳該本書籍的作者List(b_id);

                return View(eb);
            }

            return RedirectToAction("List");
        }

        // 儲存書籍資料
        [HttpPost]
        public ActionResult SaveEditBookData(EditBookModel eb)
        {
            EditBookFactory factory = new EditBookFactory();

            // 接收書籍資料表欄位
            eb.b_id = Request.Form["b_id"];
            eb.b_Type = Request.Form["b_Type"];
            eb.b_Name = Request.Form["b_Name"];
            eb.b_Info = Request.Form["b_Info"];
            eb.b_Image = Request.Form["b_Image"];
            eb.b_PublishedDate = Request.Form["b_PublishedDate"];
            eb.b_DatePrice = Convert.ToInt32(Request.Form["b_DatePrice"]);
            eb.b_ISBN = Request.Form["b_ISBN"];
            eb.b_AgeRating = Request.Form["b_AgeRating"];
            eb.b_Series_yn = Request.Form["Series"];
            eb.b_Put_yn = Request.Form["b_Put_yn"];

            if (Request.Form["PublishedIdName"] != null)
            {
                eb.PublishedIdName = Request.Form["PublishedIdName"];
                eb.p_id = factory.出版社資料解析成編號(eb.PublishedIdName);
            }

            if (eb.Image != null)
            {
                string deleteresult = factory.傳回原書籍封面照片檔名(eb.b_id);

                string 刪除舊圖片路徑 = "../書籍素材/" + eb.b_Type + "素材/" + eb.b_id + "/" + eb.b_id + "-cover.jpg";

                if (System.IO.File.Exists(Server.MapPath(刪除舊圖片路徑)))
                {
                    try
                    {
                        // 刪除舊封面圖片
                        System.IO.File.Delete(Server.MapPath(刪除舊圖片路徑));
                    }
                    catch
                    {
                        deleteresult = "修改失敗";
                    }
                }

                if (deleteresult != "修改失敗")
                {
                    // 將新封面圖片儲存到路徑
                    string photoName = factory.書籍封面圖片命名(eb);
                    eb.b_Image = photoName;

                    string 儲存書籍封面路徑 = Server.MapPath("../書籍素材/" + eb.b_Type + "素材/" + eb.b_id + "/" + photoName);
                    eb.Image.SaveAs(儲存書籍封面路徑);
                }
            }

            // 處理 tags
            eb.Tags字串 = Request.Form["Tag"];
            eb.Tags = factory.Tags轉成陣列(eb.Tags字串);
            factory.移除此書籍的標籤(eb.b_id);

            // 處理書籍作者
            eb.AuthorIdName = Request.Form.GetValues("AuthorIdName");
            factory.移除此書籍的作者(eb.b_id);

            //儲存到資料庫
            factory.SaveBookData_Books(eb);
            factory.儲存到標籤資料表(eb);
            factory.SaveBooksAuthor(eb);

            return RedirectToAction("List");
        }


        // 書籍章節清單
        public ActionResult EditChaptersList(string b_id)
        {
            if (b_id != null)
            {
                EditBookFactory factory = new EditBookFactory();

                List<EditBookModel> list = factory.列出書籍所有章節(b_id);

                return View(list);
            }

            return RedirectToAction("EditBookData");
        }

        // 修改章節
        public ActionResult EditChapters(int? bc_id)
        {
            EditBookFactory factory = new EditBookFactory();

            if (bc_id != null)
            {
                EditBookModel eb = factory.帶出要修改的章節資訊((int)bc_id);
                return View(eb);
            }
            else
            {
                return RedirectToAction("EditChaptersList");
            }
        }

        // 儲存修改的章節內容
        public ActionResult SaveEditChapters(EditBookModel eb)
        {
            EditBookFactory factory = new EditBookFactory();

            if (Request.Form["bc_id"] != null)
            {
                // 儲存章節標題
                eb.bc_id = Convert.ToInt32(Request.Form["bc_id"]);
                eb.bc_Content = Request.Form["bc_Content"];

                factory.SaveEditBookChapters(eb);

                // 刪除這個章節的所有檔案 (與資料庫)
                if (eb.Files.Count() > 0)
                {
                    eb.b_id = Request.Form["bId"];
                    eb.b_Type = Request.Form["bType"];
                    eb.bc_Chapters = Convert.ToInt32(Request.Form["bcChapters"]);

                    List<string> 該章節所有檔名 = factory.回傳所有該章節的檔名(eb);

                    string 刪除舊檔案路徑 = "../書籍素材/" + eb.b_Type + "素材/" + eb.b_id + "/" + eb.b_id + "-" + eb.bc_Chapters + "/";

                    bool 刪除結果 = false;

                    foreach (string p in 該章節所有檔名)
                    {
                        if (System.IO.File.Exists(Server.MapPath(刪除舊檔案路徑 + p)))
                        {
                            try
                            {
                                // 刪除舊檔案
                                System.IO.File.Delete(Server.MapPath(刪除舊檔案路徑 + p));
                                刪除結果 = true;
                            }
                            catch
                            {
                                // 刪除失敗
                            }
                        }
                    }

                    if (刪除結果 == true)
                    {
                        factory.刪除該章節在資料庫裡的所有的檔名(eb.bc_id);

                        if (eb.b_id != null && eb.b_Type != null && eb.bc_Chapters > 0)
                        {
                            // 儲存新檔案到實體路徑
                            eb.FilesName = new List<string>();

                            int i = 1;
                            foreach (HttpPostedFileBase uploadFile in eb.Files)
                            {
                                if (uploadFile.ContentLength > 0)
                                {
                                    // 這個陣列用來 將多筆章節檔名 儲存到資料庫使用
                                    eb.FilesName.Add(eb.b_id + "-" + eb.bc_Chapters + "-" + i + factory.回傳書籍章節檔案副檔名(uploadFile));

                                    // 上傳時自動編名
                                    uploadFile.SaveAs(Server.MapPath("../書籍素材/" + eb.b_Type + "素材/" + eb.b_id + "/" + eb.b_id + "-" + eb.bc_Chapters + "/" + eb.b_id + "-" + eb.bc_Chapters + "-" + i + factory.回傳書籍章節檔案副檔名(uploadFile)));
                                }
                                i++;
                            }

                            // 儲存新章節檔名進資料庫
                            factory.儲存該章節所有檔名到資料庫(eb);
                        }
                    }
                }
            }

            return RedirectToAction("EditChaptersList");
        }
    }
}