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
        [XmlElement("educode")]
        public List<educode> educode { get; set; }
        [XmlElement("student")]
        public List<student> student { get; set; }
        [XmlElement("GradStudent")]
        public List<GradStudent> GradStudent { get; set; }
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
    public class educode
    {
        [XmlElement("EduCode")]
        public string EduCode { get; set; }
        [XmlElement("College")]
        public string College { get; set; }
        [XmlElement("Period")]
        public string Period { get; set; }
        [XmlElement("Department")]
        public string Department { get; set; }
        [XmlElement("Grade")]
        public string Grade { get; set; }
        [XmlElement("Class")]
        public string Class { get; set; }
        [XmlElement("Team")]
        public string Team { get; set; }
    }
    public class student
    {
        [XmlElement("身分證字號")]
        public string idno { get; set; }
        [XmlElement("姓名")]
        public string name { get; set; }
        [XmlElement("學號1")]
        public string stuNo1 { get; set; }
        [XmlElement("性別")]
        public string sex { get; set; }
        [XmlElement("生日")]
        public string bdate { get; set; }
        [XmlElement("群組")]
        public string group { get; set; }
        [XmlElement("住址")]
        public string addr { get; set; }
        [XmlElement("電話")]
        public string tel { get; set; }
        [XmlElement("學號2")]
        public string stuNo2 { get; set; }
        [XmlElement("educode")]
        public string educode { get; set; }
        [XmlElement("foreignermark")]
        public string foreignermark { get; set; }
        [XmlElement("國籍身分別")]
        public string nationalityIdentity { get; set; }
    }
    public class GradStudent
    {
        [XmlElement("身分證字號")]
        public string idno { get; set; }
        [XmlElement("姓名")]
        public string name { get; set; }
        [XmlElement("學號1")]
        public string stuNo1 { get; set; }
        [XmlElement("性別")]
        public string sex { get; set; }
        [XmlElement("生日")]
        public string bdate { get; set; }
        [XmlElement("群組")]
        public string group { get; set; }
        [XmlElement("住址")]
        public string addr { get; set; }
        [XmlElement("電話")]
        public string tel { get; set; }
        [XmlElement("學號2")]
        public string stuNo2 { get; set; }
        [XmlElement("educode")]
        public string educode { get; set; }
        [XmlElement("email1")]
        public string email1 { get; set; }
        [XmlElement("畢業學年期")]
        public string graduationYear { get; set; }
        [XmlElement("foreignermark")]
        public string foreignermark { get; set; }
        [XmlElement("china_mark")]
        public string chinaMark { get; set; }
        [XmlElement("AbroadMark")]
        public string abroadMark { get; set; }
        [XmlElement("Replay")]
        public string replay { get; set; }
    }
}
