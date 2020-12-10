using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RentBook.Models;
using RentBook.Models.Userpage;
using RentBook.Models.Userpage.bookshelf;
using RentBook.Models.Userpage.userpage;
using System.Web.Script.Serialization;
using System.IO;
using RentBook.Models.Userpage.myWallet;

namespace RentBook.Controllers
{
    public class UserpageController : Controller
    {
        public ActionResult test() {
            return View();
        }
        public ActionResult Personal()
        {
            if (Session["member"] == null)
            {
                return RedirectToAction("xxx", "CV");
            }
            string x = Session["member"].ToString();
            List<userpageClass> User = (new userpageFac()).getUserInfo(x);
            List<bookBriefClass> recentReading = (new bookBriefFac()).getBookInfo(x);
            doubleClass1 myView = new doubleClass1();
            myView.userInfo = User;
            myView.bookInfo = recentReading;
            return View(myView);
        }

        public ActionResult MyBookshelf()
        {
            if (Session["member"] == null)
            {
                return RedirectToAction("xxx", "CV");
            }
            string x = Session["member"].ToString();
            List<bookBriefClass> unexpired= (new getBookshelfFac()).getUnexpiredBook(x);
            List<bookBriefClass> expired= (new getBookshelfFac()).getExpiredBook(x);
            doubleClass2 myshelf = new doubleClass2();
            myshelf.unexpiredBook = unexpired;
            myshelf.expiredBook = expired;
            return View(myshelf);
        }
        public ActionResult WishList()
        {
            if (Session["member"] == null)
            {
                return RedirectToAction("xxx", "CV");
            }
            string x = Session["member"].ToString();
            List<wishlistClass> wishlist = (new wishlistFac()).getWishlist(x);

            return View(wishlist);
        }
        public ActionResult MyComment()
        {
            if (Session["member"] == null)
            {
                return RedirectToAction("xxx", "CV");
            }
            string x = Session["member"].ToString();
            List<commentClass> cmts = (new commentFac()).getMyComment(x);//存到list後就到view處理了
            return View(cmts);//
        }
        public ActionResult MySetting()
        {
            if (Session["member"] == null)
            {
                return RedirectToAction("xxx", "CV");
            }
            string x = Session["member"].ToString();
            List<settingClass> settings = (new settingFac()).getMySetting(x);//存到list後就到view處理了
            List<string> gender = new List<string>() { "Male", "Female", "Lesbian", "Gay", "Bisexual", "Transgender", "Queer" };
            int genderIndex = Int32.Parse(settings[0].m_Gender) - 1;
            ViewBag.genderDefault = gender[genderIndex];
            //
            ViewBag.m_id = settings[0].m_id;
            //
            return View(settings);//
        }

        [HttpPost]
        public ActionResult MySetting(settingClass s)//帶資料
        {
            //
            if (Session["member"] == null)
            {
                return RedirectToAction("xxx", "CV");
            }
            string userMail = Session["member"].ToString();
            (new settingFac()).editMySetting(s, userMail);//ADO 寫法
            TempData["message"] = "更新成功";
            return RedirectToAction("MySetting");
        }
        //頭貼
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase m_Image)
        {
            if (Session["member"] == null)
            {
                return RedirectToAction("xxx", "CV");
            }
            string userMail = Session["member"].ToString();

            if ((m_Image!=null) && (m_Image.ContentLength > 0))
            {
                int point = m_Image.FileName.IndexOf(".");
                string extention = m_Image.FileName.Substring(point, m_Image.FileName.Length - point);//取得副檔名
                string photoName = Guid.NewGuid().ToString() + extention;
                //var fileName = Guid.NewGuid().ToString(); //Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/會員照片"), photoName);
                m_Image.SaveAs(path);
                (new settingFac()).uploadImg(photoName, userMail);
            }
            return RedirectToAction("MySetting");
        }

        public ActionResult MyPurchase()
        {
            if (Session["member"] == null)
            {
                return RedirectToAction("xxx", "CV");
            }
            string userMail = Session["member"].ToString();
            ViewBag.myBalance = (new myWalletFac()).getUserBalance(userMail);
            ViewBag.myDatetime = (new myWalletFac()).getUserDatetime(userMail);
            doubleClass3 myWallet = new doubleClass3();
            myWallet.comsumption = (new myWalletFac()).getConsumptionBySql(userMail);
            myWallet.Valueadded = (new myWalletFac()).getAddValueBySql(userMail);
            return View(myWallet);
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("xxx", "CV");
        }
    }
}