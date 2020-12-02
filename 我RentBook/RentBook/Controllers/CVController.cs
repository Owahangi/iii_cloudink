using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PagedList;
using RentBook.Models;

namespace RentBook.Controllers
{
    public class CVController : Controller
    {
        private RentBookdbEntities2 db = new RentBookdbEntities2();
        public ActionResult xxx()
        {
            var result = db.Books.ToList();
            var result2 = result.OrderByDescending(r => r.b_PublishedDate).ToList();


            if (Session[CDictionary.SK_LOGINED_USER] == null)
                return View(result2);
            else
            {
                ViewBag.Session = Session[CDictionary.SK_LOGINED_USER];
                return View(result2);
            }
           


        }


        public ActionResult List()
        {
            var list = db.Books.ToList();
            return View(list);
        }
        
        public ActionResult SeachItem(string searchstring, string drop, int page = 1)
        {
            int pageSize = 10;
            var list = db.Books.ToList();


            if (!String.IsNullOrEmpty(drop))
            {

                if (!String.IsNullOrEmpty(searchstring))
                {
                    int currentPage = page < 1 ? 1 : page;
                    using (db)
                    {
                        var result = (from s in db.Books
                                      where s.b_Name.Contains(searchstring) && s.b_Type == drop
                                      select s).ToList();

                        ViewBag.drop = drop;
                        var result2 = result.ToPagedList(currentPage, pageSize);
                        return View(result2);

                    }
                }
                else
                {
                    int currentPage = page < 1 ? 1 : page;
                    var result = (from s in db.Books
                                  where s.b_Type == drop
                                  select s).ToList();

                    ViewBag.drop = drop;
                    var result2 = result.ToPagedList(currentPage, pageSize);
                    return View(result2);
                }
            }
            else
            {
                
                if (!String.IsNullOrEmpty(searchstring))
                {
                    int currentPage = page < 1 ? 1 : page;
                    using (db)
                    {
                        var result = (from s in db.Books
                                      where s.b_Name.Contains(searchstring)
                                      select s).ToList();

                        ViewBag.drop = drop;
                        var result2 = result.ToPagedList(currentPage, pageSize);
                        return View(result2);

                    }
                }
                else 
                {
                    ViewBag.drop = drop;
                    int currentPage = page < 1 ? 1 : page;
                    var list2 = list.ToPagedList(currentPage, pageSize);
                    return View("SeachItem", list2);
                }
            }

        }

        public ActionResult LatestItem(int page = 1)
        {
            int pageSize = 10;
            var latest = db.Books.OrderByDescending(l => l.b_PublishedDate);

            int currentPage = page < 1 ? 1 : page;
            var latest2 = latest.ToPagedList(currentPage, pageSize);
            return View(latest2);
        }

        public ActionResult BookPage(string bid)
        {
            var book = db.Books.FirstOrDefault(b => b.b_id == bid);
            if (book == null)
                return RedirectToAction("SeachItem");            
            var chap = db.BooksChapters.Where(c => c.b_id == bid);
            var msg = db.BooksMessage.Where(m => m.b_id == bid);


            //Member
            CmessageFactory factory = new CmessageFactory();
            List<CmessageSqlView> list = new List<CmessageSqlView>();
            list = factory.getAllmessageSqlViews();
            var list2 = list.Where(m => m.b_id == bid);
            return View(new Models.BooksChap
            {
                Books = book,
                Chapters = chap.ToList(),
                Messages = msg.ToList(),
                CmessageSqlViews = list2.ToList()

            });
        }
        [HttpPost] //限定使用POST
        //[Authorize] // 會員登入後才可評論
        public ActionResult BookPage(BooksMessage bm)
        {
            bm.bm_MessageTime = DateTime.Now;
            int bm_Sorce = (int)bm.bm_Score;
            db.BooksMessage.Add(bm);
            db.SaveChanges();

            return RedirectToAction("BookPage", new { b_id = bm.b_id });
        }
    }
}