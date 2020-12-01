

namespace RentBook
{
    using System;
    using System.Web;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [MetadataType(typeof(buildBook_MD))]//MD這個是自己命名
    public partial class Books
    {
        public class buildBook_MD
        {
          
            public int b_id { get; set; }
            [Required(ErrorMessage = "必填欄位")]
            public string b_Name { get; set; }
            [Required(ErrorMessage = "必填欄位")]
            public string b_Info { get; set; }
            [Required(ErrorMessage = "必填欄位")]
            public string b_Image { get; set; }
            //public HttpPostedFileBase image { get; set; }
            [Required(ErrorMessage = "必填欄位")]
            public string b_Type { get; set; }
            [Required(ErrorMessage = "必填欄位")]
            public Nullable<System.DateTime> b_PublishedDate { get; set; }
            [Required(ErrorMessage = "必填欄位")]
            public Nullable<int> b_DatePrice { get; set; }
            [Required(ErrorMessage = "必填欄位")]
            public string b_ISBN { get; set; }
            [Required(ErrorMessage = "必填欄位")]
            public Nullable<int> b_AgeRating { get; set; }
            [Required(ErrorMessage = "必填欄位")]
            public Nullable<int> b_Likes { get; set; }
            [Required(ErrorMessage = "必填欄位")]
            public string b_Series_yn { get; set; }
            [Required(ErrorMessage = "必填欄位")]
            public string b_Put_yn { get; set; }
            [Required(ErrorMessage = "必填欄位")]
            public Nullable<int> p_id { get; set; }

        }
    }
}