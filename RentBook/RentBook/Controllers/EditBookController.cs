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
            string keyword = Request.Form["txtKeyword"];

            EditBookFactory factory = new EditBookFactory();

            List<EditBookModel> list = null;

            if (string.IsNullOrEmpty(keyword))
            {
                list = factory.列出所有書籍資訊();
            }
            else
            {
                list = factory.getByKeyword(keyword);
            }
            return View(list);            
        }

        public ActionResult EditBookData(string b_id)
        {
            if (b_id != null)
            {
                EditBookFactory factory = new EditBookFactory();

                EditBookModel eb = factory.帶出要修改的書籍資訊(b_id);
                eb.Tags字串 = factory.列出書籍Tags(b_id);
                eb.出版社編號加名稱列表 = factory.傳回出版社編號名稱列表();
                eb.作者編號加名稱列表 = factory.傳回作者編號名稱列表();
                eb.列出本書的作者 = factory.回傳該本書籍的作者List(b_id);

                return View(eb);
            }

            return RedirectToAction("List");
        }

        // 書籍章節清單
        public ActionResult EditChaptersList(string b_id)
        {
            if (b_id != null)
            {
                EditBookFactory factory = new EditBookFactory();

                List<EditBookModel> list = factory.列出書籍所有章節(b_id);

                return View(list);
            }

            return RedirectToAction("EditBookData");
        }

        // 修改章節
        public ActionResult EditChapters(int? bc_id)
        {
            EditBookFactory factory = new EditBookFactory();

            if(bc_id != null)
            {
                EditBookModel eb = factory.帶出要修改的章節資訊((int)bc_id);
                return View(eb);
            }
            else
            {
                return RedirectToAction("EditChaptersList");
            }

            
        }
    }
}