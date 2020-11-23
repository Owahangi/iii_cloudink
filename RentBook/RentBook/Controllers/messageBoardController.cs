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
        // bm_id 書籍留言序號
        // b_id 書籍編號
        // m_id 會員編號
        // bm_Message 留言內容
        // bm_MessageTime 留言時間
        // bm_score 會員對書籍的評分
        // m_Name dbo.Member資料表的會員暱稱
        public ActionResult DeleteMessageBoard(int? bm_id) 
        {
            if (bm_id != null) 
            {
                CmessageBoard dCust = new CmessageBoard() { bm_id = (int)bm_id };
                (new CmessageFactory()).delete_BooksMessage(dCust);
            }
            return RedirectToAction("書籍資訊");
        }
        public ActionResult SaveMessage() 
        {
            CmessageBoard x = new CmessageBoard();
            //待改
            //x.bm_id
            //x.b_id
            //x.m_id
            x.bm_Message = Request.Form["txtMessage"];
            x.bm_score = int.Parse(Request.Form["radioScore"]);

            (new CmessageFactory()).create__BooksMessage(x);
            return RedirectToAction("書籍資訊");

        }
        // GET: messageBoard
        public ActionResult CreateMessageBoard()
        {
            return View();
        }
    }
}