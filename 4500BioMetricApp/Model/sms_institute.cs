using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4500BioMetricApp.Model
{
    public class sms_institute
    {
        public string institute_name { set; get; }
        public byte[] institute_logo { set; get; }
        public byte[] male_image { set; get; }
        public byte[] female_image { set; get; }
        public string mac { set; get; }
        public string insertion { set; get; }
        public string isMultiPartSMSAccess { set; get; }

        public DateTime date { get; set; }
        public int page_no { get; set; }
        public string month_name { get; set; }
        public string year { get; set; }
    }
}
