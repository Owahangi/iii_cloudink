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
            var bkb = from a in db.BooksTags
                      group a by a.t_id into g
                      select new { b = g.Count(), g.Key };
            var list = from a in bkb
                       join b in db.Tags on a.Key equals b.t_id
                       select new { a.b, a.Key, b.t_Name };
            var list2 = list.OrderByDescending(a => a.b).Take(12);
            var list3 = list2.ToList();
            var bm = db.BooksMessage.ToList();
            List<int> bkb1 = new List<int>();
            List<int?> bkb2 = new List<int?>();
            List<string> bkb3 = new List<string>();
            for (var i = 0; i < 12; i++)
            {
                bkb1.Add(list3[i].b);
                bkb2.Add(list3[i].Key);
                bkb3.Add(list3[i].t_Name);
            }
            CmessageFactory factory = new CmessageFactory();
            List<CmessageSqlView> cm = new List<CmessageSqlView>();
            
            cm = factory.getAllmessageSqlViews();
            var cm2 = cm.ToList();
            

            if (Session[CDictionary.SK_LOGINED_USER] == null)
            {
                return View(new ViewModels.Tags
                {
                    Books = result2,
                    count = bkb1,
                    tid = bkb2,
                    tname = bkb3,
                    BooksMessages = bm,
                    CmessageSqlViews = cm2
                });
            }

            else
            {
                ViewBag.Session = Session[CDictionary.SK_LOGINED_USER];
                return View(new ViewModels.Tags
                {
                    Books = result2,
                    count = bkb1,
                    tid = bkb2,
                    tname = bkb3,
                    BooksMessages = bm,
                    CmessageSqlViews = cm2
                });
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
                        ViewBag.count = result.Count();
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
                    ViewBag.count = result.Count();
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
                        ViewBag.count = result.Count();
                        ViewBag.drop = drop;
                        var result2 = result.ToPagedList(currentPage, pageSize);
                        return View(result2);

                    }
                }
                else
                {
                    ViewBag.count = list.Count();
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
            ViewBag.count = latest.Count();
            int currentPage = page < 1 ? 1 : page;
            var latest2 = latest.ToPagedList(currentPage, pageSize);
            return View(latest2);
        }

        public ActionResult BookPage(string bid)
        {
            var user = (Session[CDictionary.SK_LOGINED_USER] as CMember);
            
            CmessageFactory factory = new CmessageFactory();
            //星星平均
            int AvgSore = factory.getAvgSorce(bid);
            ViewBag.AVGSORE = AvgSore; // 丟ViewBag.AVGSORE到Views

            bool 有沒有留過言 = false;

            if (user != null)
            {
                //判斷有沒留過言
                List<BooksMessage> ListBooksMessage = new List<BooksMessage>();
                有沒有留過言 = factory.getOneMessage(bid, user.m_id);


                //個人星星
                int MemberStar = factory.getMemberStar(bid, user.m_id);
                ViewBag.MEMBERSTAR = MemberStar; // 丟ViewBag.MEMBERSTAR到Views

            }



            //

            var book = db.Books.FirstOrDefault(b => b.b_id == bid);
            if (book == null)
                return RedirectToAction("SeachItem");
            var chap = db.BooksChapters.Where(c => c.b_id == bid);
            var msg = db.BooksMessage.Where(m => m.b_id == bid);

            var bkb = db.BookCaseBooks.Where(b => b.bc_id == user.bc_id);

            var result = db.Books.ToList();
            var result2 = result.OrderByDescending(r => r.b_PublishedDate).ToList();

            //Tag
            var tags = from a in db.BooksTags
                       group a by a.t_id into g
                       select new { b = g.Count(), g.Key };
            var tlist = from a in tags
                        join b in db.Tags on a.Key equals b.t_id
                        select new { a.b, a.Key, b.t_Name };
            var tlist2 = tlist.OrderByDescending(a => a.b).Take(12);
            var tlist3 = tlist2.ToList();
            List<int> bkb1 = new List<int>();
            List<int?> bkb2 = new List<int?>();
            List<string> bkb3 = new List<string>();
            for (var i = 0; i < 12; i++)
            {
                bkb1.Add(tlist3[i].b);
                bkb2.Add(tlist3[i].Key);
                bkb3.Add(tlist3[i].t_Name);
            }

            var wishlist = db.BooksWishlist.Where(w => w.b_id == bid);
            //Member
            //CmessageFactory factory = new CmessageFactory();
            List<CmessageSqlView> list = new List<CmessageSqlView>();
            list = factory.getAllmessageSqlViews();
            var list2 = list.Where(m => m.b_id == bid);

            //有幾個人投票留言
            ViewBag.msgcount = db.BooksMessage.Where(b=>b.b_id==bid&&b.bm_Score>0).Count();

            //帶出書籍價格
            if (bid != null)
                book.b_DatePrice = (new RentBook.Models.Point.PointFactory()).找出書籍一日價格(bid);
            if (Session[RentBook.Models.CDictionary.SK_LOGINED_USER] == null)
            {
                return View(new Models.BooksChap
                {
                    Books = book,
                    Chapters = chap.ToList(),
                    Messages = msg.ToList(),
                    CmessageSqlViews = list2.ToList(),
                    log = false,
                    LatestBooks = result2,                     
                    BooksWishlists = wishlist.ToList(),
                    count = bkb1,
                    tid = bkb2,
                    tname = bkb3,
                    listbm = 有沒有留過言

                });
            }
            else
            {
                return View(new Models.BooksChap
                {
                    Books = book,
                    Chapters = chap.ToList(),
                    Messages = msg.ToList(),
                    CmessageSqlViews = list2.ToList(),
                    BookCaseBooks = bkb.ToList(),
                    log = true,
                    cmember = user,
                    LatestBooks = result2,
                    BooksWishlists = wishlist.ToList(),
                    count = bkb1,
                    tid = bkb2,
                    tname = bkb3,
                    listbm = 有沒有留過言

                });
            }

        }
        [HttpPost] //限定使用POST
        //[Authorize] // 會員登入後才可評論
        public ActionResult SAVEBookPage(BooksMessage bm)
        {

            bm.bm_MessageTime = DateTime.Now;
            int bm_Sorce = (int)bm.bm_Score;
            db.BooksMessage.Add(bm);
            db.SaveChanges();

            return RedirectToAction("BookPage", new { bid = bm.b_id });
        }

        public ActionResult SAVEwishlist(int bcid, string bid, DateTime bwaddtime)
        {
            BooksWishlist bw = new BooksWishlist();
            bw.bc_id = bcid;
            bw.b_id = bid;
            bw.bw_AddTime = bwaddtime;
            db.BooksWishlist.Add(bw);
            db.SaveChanges();

            return RedirectToAction("BookPage", new { bid = bid });
        }
        public ActionResult DELETEwishlist(int bcid, string bid)
        {
            var dbw = db.BooksWishlist.FirstOrDefault(d => d.bc_id == bcid && d.b_id == bid);
            if (dbw != null)
            {
                db.BooksWishlist.Remove(dbw);
                db.SaveChanges();
            }
            return RedirectToAction("BookPage", new { bid = bid });
        }

        public ActionResult TAGsearch(string tagname, int page = 1)
        {
            int pageSize = 10;
            int currentPage = page < 1 ? 1 : page;

            var a1 = db.Tags.Where(a => a.t_Name == tagname);
            


            var list = from a in db.BooksTags
                       join b in db.Tags on a.t_id equals b.t_id
                       where b.t_Name.Contains(tagname)
                       select new { a, b };
            var list2 = from a in list
                        join b in db.Books on a.a.b_id equals b.b_id
                        select new { a, b };
            var list3 = list2.ToList();
            
            List<TAGsearch> tags = new List<TAGsearch>();
            foreach (var item in list3)
            {
                TAGsearch x = new TAGsearch();
                x.bid = item.b.b_id;
                x.bimage = item.b.b_Image;
                x.binfo = item.b.b_Info;
                x.bname = item.b.b_Name;
                x.btype = item.b.b_Type;

                tags.Add(x);

            }

            return View(tags);
        }

        public ActionResult news1()
        {
            return View();
        }
        public ActionResult news2()
        {
            return View();
        }
        public ActionResult news3()
        {
            return View();
        }
        public ActionResult news4()
        {
            return View();
        }
        public ActionResult news5()
        {
            return View();
        }
        public ActionResult Allnews()
        {
            return View();
        }
    }
}