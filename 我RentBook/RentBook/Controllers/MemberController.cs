using RentBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RentBook.Controllers
{
    public class MemberController : Controller
    {
        // GET: Member
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }
        public ActionResult List()
        {
            string keyword = Request.Form["txtKeyword"];
            List<CMember> list = null;
            if (string.IsNullOrEmpty(keyword))
                list = (new CMemberFactory()).getAll();
            else
                //list = (new CMemberFactory()).getByKeyword(keyword);
                list = (new CMemberFactory()).getAll();
            return View(list);
        }

        public ActionResult Edit(int? m_id)
        {
            if (m_id == null)
                return RedirectToAction("List");
            CMember x = (new CMemberFactory()).getById((int)m_id);
            return View(x);
        }

        [HttpPost]
        public ActionResult Edit(CMember target)
        {

            (new CMemberFactory()).update(target);
            return RedirectToAction("List");
        }

        public ActionResult Save()
        {
            CMember x = new CMember();
            //x.m_id = "M00001";
            x.m_id = new CMemberFactory().get_m_id();
            x.m_Name = Request.Form["txt_m_Name"];
            //x.m_Birth = Request.Form["txt_m_Birth"];
            x.m_Birth = Convert.ToDateTime(Request.Form["txt_m_Birth"]);
            x.m_Gender = Request.Form["txt_m_Gender"];
            string s_Pwd = Request.Form["txt_s_Pwd"];
            //x.fName = Request.Form["txtName"];
            //x.fPhone = Request.Form["txtPhone"];
            x.m_Email = Request.Form["txt_m_Email"];
            //x.fAddress = Request.Form["txtAddress"];
            //x.fPassword = Request.Form["txtPassword"];

            (new CMemberFactory()).create(x, s_Pwd);
            return RedirectToAction("Create");
            //return View();
        }

    }
}