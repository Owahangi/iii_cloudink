using RentBook.Models.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RentBook.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            TestFactory factory = new TestFactory();
            List<TestModel> list =  factory.ReturnAll();


            return View(list);
        }
    }
}