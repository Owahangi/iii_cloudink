using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace RentBook.Controllers
{
    public class HomeController : Controller
    {
        RentBookdbEntities2 db = new RentBookdbEntities2();
        int pageSize = 10;
        // GET: Home
        public ActionResult Index(int page = 1)
        {
            int currentPage = page < 1 ? 1 : page;
            var book = db.Books.OrderBy(m => m.b_id).ToList();
            var result = book.ToPagedList(currentPage, pageSize);
            return View(result);
        }
    }
}