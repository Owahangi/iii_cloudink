using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RentBook.Models;
using RentBook.ViewModels;

namespace RentBook.Controllers
{
    public class CommonController : Controller
    {
        // GET: Common
        //public ActionResult Index()
        //{
        //    return View();
        //}
        public ActionResult Home()
        {
            //if (Session[CDictionary.SK_LOGINED_USER] == null)
            //    return RedirectToAction("Login");
            if (Session[CDictionary.SK_LOGINED_USER] == null)
                return RedirectToAction("HomeFailure");

            return View();
        }

        public ActionResult HomeFailure()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(CLoginViewModel login)
        {
            if (string.IsNullOrEmpty(login.txtAccount) ||
                string.IsNullOrEmpty(login.txtPassword)
                )
                return View();

            //帳密驗證
            CMember cust = ((new CMemberFactory()).authenticated(
                 login.txtAccount, login.txtPassword));

            if (cust != null)
            {
                //存進Session
                Session[CDictionary.SK_LOGINED_USER] = cust;

                //重新導向到 首頁
                return RedirectToAction("xxx","CV");
                //return RedirectToAction("Home");
            }
            return View();
        }


        public ActionResult test()
        {
            return View();
        }

        public ActionResult MemberHome()
        {
            return View();
        }

        public ActionResult MemberLove()
        {
            return View();
        }

        public ActionResult LogOut()
        {
            Session.Clear();
            //return View();
            return RedirectToAction("xxx","CV");
        }
    }

    
}
