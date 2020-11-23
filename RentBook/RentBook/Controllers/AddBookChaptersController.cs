using RentBook.Models.AddChapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RentBook.Controllers
{
    public class AddBookChaptersController : Controller
    {
        // GET: AddBookChapters
        public ActionResult AddChapters(string b_id,string b_Type)
        {
            // 須從前端回傳要上傳的 書籍編號 與 書籍分類
            AddBookChaptersModel ac = new AddBookChaptersModel();
            ac.b_id = b_id;
            ac.b_Type = b_Type;


            if (b_id != null)
            {
                // 將目前書籍要新增的章節傳到 View
                AddChaptersFactory factory = new AddChaptersFactory();
                ac.bc_Chapters = factory.目前最新章節(ac) + 1;
            }
            
            return View(ac);
        }

        [HttpPost]
        public ActionResult SaveChapters(AddBookChaptersModel ac)
        {
            ac.b_id = Request.Form["bId"];
            ac.b_Type = Request.Form["bType"];
            ac.bc_Content = Request.Form["bcContent"];
            ac.bc_Chapters = Convert.ToInt32(Request.Form["bcChapters"]);

            if(ac.bc_id != null && ac.b_Type !=null && ac.bc_Content != null && ac.bc_Chapters > 0)
            {
                AddChaptersFactory factory = new AddChaptersFactory();

                // 儲存該章節的書籍檔案到路徑
                ac.FilesName = new List<string>();

                // 在儲存檔案時 自動更改檔名
                // 在上傳時需注意：
                // 1.請將每個檔案名稱 依照順序重新命名 例：1.txt、2.txt、3.txt .....
                // 2.確認要上傳的那個資料夾 Windows 的排序是依據 1.名稱 2.遞增
                if (ac.Files.Count() > 0)
                {
                    int i = 1;
                    foreach (HttpPostedFileBase uploadFile in ac.Files)
                    {
                        if (uploadFile.ContentLength > 0)
                        {
                            // 這個陣列用來 將多筆章節檔名 儲存到資料庫使用
                            ac.FilesName.Add(ac.b_id + "-1-" + i + factory.回傳書籍章節檔案副檔名(uploadFile));

                            // 上傳時自動編名
                            uploadFile.SaveAs(Server.MapPath("../書籍素材/" + ac.b_Type + "素材/" + ac.b_id + "/" + ac.b_id + "-1/" + ac.b_id + "-1-" + i + factory.回傳書籍章節檔案副檔名(uploadFile)));

                            // 維持原上傳檔名
                            // uploadFile.SaveAs(Server.MapPath("../書籍素材/" + b.b_Type + "素材/" + b.b_id + "/" + b.b_id + "-1/" + uploadFile.FileName));
                        }
                        i++;
                    }
                }

                factory.儲存章節標題及檔名(ac);
            }

            return RedirectToAction("AddBook");
        }
    }
}