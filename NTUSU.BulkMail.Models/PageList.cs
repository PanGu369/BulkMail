using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTUST.BulkMail.Models
{
    public class PageList
    {
        public int FirstItemOnPage { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool IsFirstPage { get; set; }
        public bool IsLastPage { get; set; }
        public int LastItemOnPage { get; set; }
        public int PageCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItemCount { get; set; }
        public IEnumerable<Object> ItemList { get; set; }
    }

    public class ResultMessage
    {
        [Display(Name = "狀態")]
        public string Status { get; set; }
        [Display(Name = "JsonResult 訊息")]
        public string Message { get; set; }
        [Display(Name = "Object Data")]
        public Object Body { get; set; }
    }
}
