using RentBook.Models.EditBook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RentBook.Controllers
{
    public class EditBookController : Controller
    {
        // GET: EditBook
        public ActionResult List()
        {
            EditBookFactory factory = new EditBookFactory();

            List<EditBookList> list = factory.列出所有書籍資訊();
            
            return View(list);
        }

        public ActionResult EditBookData()
        {
            EditBookFactory factory = new EditBookFactory();
            EditBookModel eb = new EditBookModel();
            eb.出版社編號加名稱 = factory.傳回出版社編號名稱();
            eb.作者編號加名稱 = factory.傳回作者編號名稱();

            List<SelectListItem> item = new List<SelectListItem>();


            return View();
        }
    }
}