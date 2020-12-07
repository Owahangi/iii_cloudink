using RentBook.Models.Point;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RentBook.Controllers
{
    public class PointController : Controller
    {
        // 儲值頁面
        public ActionResult AddPoint(string m_id)
        {
            PointModel p = new PointModel();
            p.m_id = m_id;
            return View(p);
        }

        // 儲存儲值資料
        [HttpPost]
        public ActionResult SaveAddPoint(PointModel p)
        {
            if (Request.Form["mad_AddPoint"] != null && Request.Form["m_id"] != null)
            {
                p.m_id = Request.Form["m_id"];
                p.mad_AddPoint = Convert.ToInt32(Request.Form["mad_AddPoint"]);

                PointFactory factory = new PointFactory();
                factory.儲存儲值資料(p);
            }
        
            return RedirectToAction("AddPoint", new { m_id = p.m_id });

        }

        // 可以租書籍的頁面
        public ActionResult List(string m_id, string b_id)
        {
            PointFactory factory = new PointFactory();
            PointModel p = new PointModel();
            if (b_id != null)
                p.b_DatePrice = factory.找出書籍一日價格(b_id);
            p.b_id = b_id;
            p.m_id = m_id;

            return View(p);
        }

        // 儲存消費資料
        [HttpPost]
        public ActionResult ShopBook(PointModel p)
        {
            PointFactory factory = new PointFactory();

            int 使用者剩餘點數 = factory.回傳目前此會員的剩餘點數(p.m_id);

            if (使用者剩餘點數 > Convert.ToInt32(Request.Form["msd_CostPoint"]))
            {
                p.m_id = Request.Form["m_id"];
                p.b_id = Request.Form["b_id"];
                p.b_DatePrice = Convert.ToInt32(Request.Form["b_DatePrice"]);
                p.msd_CostPoint = Convert.ToInt32(Request.Form["msd_CostPoint"]);
                p.購買天數 = factory.判斷使用者購買書籍天數(p);
            }
            else
            {
                // 使用者錢不夠
            }

            factory.儲存消費資料(p);

            return RedirectToAction("List");
        }

        public ActionResult ShopMonthlyCard(string m_id)
        {
            PointFactory factory = new PointFactory();
            if(m_id == null)
            {
                m_id = "沒登入";
            }
            List<PointModel> list = factory.列出月卡方案(m_id);

            return View(list);
        }

        // 儲存月卡資料
        public ActionResult SaveMonthlyCard(string m_id, string bmc_id, string bmc_Name, int bmc_Date, int bmc_Price)
        {
            PointFactory factory = new PointFactory();
            PointModel p = new PointModel();

            p.m_id = m_id;
            p.bmc_id = bmc_id;
            p.bmc_Name = bmc_Name;
            p.bmc_Date = bmc_Date;
            p.bmc_Price = bmc_Price;

            int 使用者剩餘點數 = factory.回傳目前此會員的剩餘點數(m_id);

            if (使用者剩餘點數 > bmc_Price)
            {
                factory.儲存月卡資料(p);
            }
            else
            {
                // 使用者錢不夠
            }
            return RedirectToAction("ShopMonthlyCard", new { m_id = p.m_id });
        }
    }
}