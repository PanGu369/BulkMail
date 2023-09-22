using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTUST.BulkMail.Models
{
    public class MailGroupViewModal
    {
        public string name { get; set; }
        public string mail { get; set; }
        public string groupmail { get; set; }
    }
    public class MailGroupListViewModal
    {
        public string cname {  get; set; }
        public string name { get; set; }
        public string mail { get; set; }
        public string groupmail { get; set; }
    }

    public class GroupMailViewNews
    {
        public string cname { get; set; }
        public string mail { get; set; }
        public string filename { get; set; }
        public string member { get; set; }
        public string membername { get; set;}
        public string membermail { get; set; }
        public string memtype { get; set;}
    }
}
