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

    public class epageMail
    {
        public string pt_name { get; set; }
        public string pt_all_url { get; set; }
        public string pt_desc { get; set; }
        public DateTime pt_rel_date { get; set; }
        public string pt__user1 { get; set; }
        public string membermail { get; set; }
        public string membername { get; set; }
        public string sendDate { get; set; }
        public DateTime pt_added { get; set; }
        public bool sended { get; set; }
    }
}
