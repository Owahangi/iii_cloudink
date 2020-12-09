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
            RentBookdbEntities2 db = new RentBookdbEntities2();
            var bkb = from a in db.BooksTags
                      group a by a.t_id into g
                      select new { b = g.Count(), g.Key };
            var list = from a in bkb
                       join b in db.Tags on a.Key equals b.t_id
                       select new { a.b, a.Key, b.t_Name };
            var list2 = list.OrderByDescending(a => a.b).Take(12);
            var list3 = list2.ToList();
            List<int> bkb1 = new List<int>();
            List<int?> bkb2 = new List<int?>();
            List<string> bkb3 = new List<string>();
            for (var i = 0; i < 12; i++)
            {
                bkb1.Add(list3[i].b);
                bkb2.Add(list3[i].Key);
                bkb3.Add(list3[i].t_Name);
            }
            return View(new ViewModels.Tags
            {
                count = bkb1,
                tid = bkb2,
                tname = bkb3
            });
        }

        public ActionResult MemberMessage()
        {
            return View();
        }

        public ActionResult MemberRecord()
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
