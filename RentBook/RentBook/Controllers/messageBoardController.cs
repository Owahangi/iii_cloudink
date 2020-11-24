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

        // string m_Alias dbo.Member資料表的會員暱稱 需要join

        dbRentBookdbEntities db = new dbRentBookdbEntities();

        public ActionResult DeleteMessageBoard(string M_ID) 
        {
            BooksMessage bm = db.BooksMessage.FirstOrDefault(m => m.m_id == M_ID);
            if (bm != null) 
            {
                db.BooksMessage.Remove(bm);
                db.SaveChanges();
            }
            return RedirectToAction("CreateMessageBoard");
        }


        [HttpPost] //限定使用POST
        [Authorize] // 會員登入後才可評論
        public ActionResult CreateMessageBoard(string B_ID, string MESSAGE, int rate) 
        {
            var M_ID = HttpContext.Session.SessionID;

            //待改 join m_Alias
            BooksMessage x = new BooksMessage();
            x.b_id = B_ID;
            x.m_id = M_ID;
            x.bm_Message = MESSAGE;
            x.bm_MessageTime = DateTime.Now;
            x.bm_Score = rate;

            db.BooksMessage.Add(x);
            db.SaveChanges();
            return RedirectToAction("CreateMessageBoard", new { B_ID = B_ID });
        }

        // GET: messageBoard
        public ActionResult CreateMessageBoard()
        {
            return View();
        }
    }
}