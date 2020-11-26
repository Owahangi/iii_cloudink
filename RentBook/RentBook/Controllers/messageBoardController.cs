﻿using System;
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
        public ActionResult CreateMessageBoard(string BM_MESSAGE, int rate) 
        {
            //var M_ID = HttpContext.Session.SessionID;

            //var message = new Models.CmessageBoard()
            //{
            //    bm_id = BM_ID,
            //    b_id = B_ID,
            //    m_id = M_ID,
            //    bm_Message = BM_MESSAGE,
            //    bm_MessageTime = DateTime.Now,
            //    bm_Score = rate
            //};
            //db.BooksMessage.Add(message);
            //db.SaveChanges();
            //return RedirectToAction("CreateMessageBoard", new { B_ID = B_ID });

            BooksMessage x = new BooksMessage();
            //x.b_id = B_ID;
            //x.m_id = M_ID;
            x.bm_Message = BM_MESSAGE;
            x.bm_MessageTime = DateTime.Now;
            x.bm_Score = rate;

            db.BooksMessage.Add(x);
            db.SaveChanges();
            return RedirectToAction("CreateMessageBoard");
            //return RedirectToAction("CreateMessageBoard", new { B_ID = B_ID });
        }

        // GET: messageBoard
        public ActionResult CreateMessageBoard()
        {

            //CmessageFactory factory = new CmessageFactory();
            //list = factory.getAllmessageSqlViews();
            return View();
        }
    }
}