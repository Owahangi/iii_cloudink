using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentBook.Models.AddBook
{
    public class EditBookModel
    {
        // 下拉式選單使用
        public List<string> 出版社編號加名稱 { get; set; }
        public List<string> 作者編號加名稱 { get; set; }
    }
}