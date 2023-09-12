using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace NTUSU.BulkMail.Models
{
    [Serializable]
    [XmlRoot("NewDataSet")]
    public class NewDataSet
    {
        [XmlElement("member")]
        public List<member> member { get; set; }
    }
    public class member
    {
        [XmlElement("IDNO")]
        public string IDNO { get; set; }
        [XmlElement("name")]
        public string name { get; set; }
        [XmlElement("unit")]
        public string unit { get; set; }
        [XmlElement("unicode")]
        public string unicode { get; set; }
        [XmlElement("class")]
        public string tclass { get; set; }
        [XmlElement("title")]
        public string title { get; set; }
        [XmlElement("tel")]
        public string tel { get; set; }
        [XmlElement("email")]
        public string email { get; set; }
        [XmlElement("kind")]
        public string kind { get; set; }
        [XmlElement("unitcode")]
        public string unitcode { get; set; }
    }
}
