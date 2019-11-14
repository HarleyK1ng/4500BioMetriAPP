using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4500BioMetricApp
{
    public class user_data
    {
        public int id { get; set; }
        public string finger_name { get; set; }
        public byte[] template { get; set; }
    }
}
