using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using RentBook.Models;

namespace RentBook.Controllers
{
    public class messageBoardController : Controller
    {
        // int  bm_id 書籍留言序號
        // string b_id 書籍編號
        // string m_id 會員編號
        // string bm_Message 留言內容
        // DateTime bm_MessageTime 留言時間
        // int bm_Score 會員對書籍的評分
        // string m_Alias dbo.Member資料表的會員暱稱

        dbRentBookdbEntities db = new dbRentBookdbEntities();

        public ActionResult DeleteMessageBoard(int BM_ID) 
        {
            BooksMessage bm = db.BooksMessage.FirstOrDefault(m => m.bm_id == BM_ID);
            if (bm != null) 
            {
                db.BooksMessage.Remove(bm);
                db.SaveChanges();
            }
            return RedirectToAction("CreateMessageBoard");
        }


        [HttpPost] //限定使用POST
        //[Authorize] // 會員登入後才可評論
        public ActionResult CreateMessageBoard(BooksMessage bm) 
        {
            string b_id = bm.b_id;
            string m_id = bm.m_id;
            string bm_Message = bm.bm_Message;
            bm.bm_MessageTime = DateTime.Now;
            bm.bm_Score = int.Parse(Request.Form["bm_Score"]);

            db.BooksMessage.Add(bm);
            db.SaveChanges();

            return RedirectToAction("CreateMessageBoard", new { b_id = b_id });
        }

        // GET: messageBoard
        public ActionResult CreateMessageBoard()
        {
            CmessageFactory factory = new CmessageFactory();
            List<CmessageSqlView> list = new List<CmessageSqlView>();
            list = factory.getAllmessageSqlViews();
            return View(list);
        }
    }
}