using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RentBook.Models;
using System.Web.SessionState;



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
            int? bm_Score =bm.bm_Score;
            db.BooksMessage.Add(bm);
            db.SaveChanges();

            return RedirectToAction("CreateMessageBoard", new { b_id = b_id });
        }

        // GET: messageBoard
        public ActionResult CreateMessageBoard(string b_id)
        {
            //var UserEmail = db.Member
            //    .Where(m => m.m_Email == mem.m_Email)
            //    .FirstOrDefault();

            //Session["SK_LOGINED_USER"] = UserEmail;

            var book = db.Books.FirstOrDefault(b => b.b_id == b_id);
            if (book == null)
                return RedirectToAction("SeachItem");
            //var chap = db.BooksChapters.Where(c => c.b_id == b_id);
            var msg = db.BooksMessage.Where(m => m.b_id == b_id);

            CmessageFactory factory = new CmessageFactory();
            List<CmessageSqlView> list = new List<CmessageSqlView>();// join tables
            list = factory.getAllmessageSqlViews();
            var list2 = list.Where(m => m.b_id == b_id);

            //星星平均
            int AvgSore = factory.getAvgSorce(b_id);
            ViewBag.AVGSORE = AvgSore; // 丟ViewBag.AVGSORE到Views

            Member c = Session[CDictionary.SK_LOGINED_USER] as Member;

            //判斷有沒流過言
            List<BooksMessage> ListBooksMessage = new List<BooksMessage>();
            string m_id = ""; //待刪除
            ListBooksMessage = factory.getOneMessage(b_id, m_id);
            
            return View(list);
        }

    }
}