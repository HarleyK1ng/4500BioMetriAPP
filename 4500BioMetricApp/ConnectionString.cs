using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4500BioMetricApp
{
   public static class ConnectionString
    {
        public static String con_string = @"Server=" + MainWindow.Server + "; port=" + MainWindow.Port + "; Database=" + MainWindow.Database + "; Uid=" + MainWindow.Uid + "; Pwd=7120020@123; default command timeout=9999;CHARSET=utf8";
    }
}
