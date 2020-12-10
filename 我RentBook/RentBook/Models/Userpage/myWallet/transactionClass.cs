using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentBook.Models.Userpage
{
    public class transactionClass
    {
        public int orderId { get; set; }
        public int amount { get; set; }
        public DateTime time { get; set; }
    }
}