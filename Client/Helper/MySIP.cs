using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Helper
{
    public static class MySIP 
    {
        public static string INVITE
        {
            get
            {
                return "INVITE";
            }
        }
        public static string ACK
        {
            get
            {
                return "ACK";
            }
        }
        public static string BYE
        {
            get
            {
                return "BYE";
            }
        }
        public static string CANCEL
        {
            get
            {
                return "CANCEL";
            }
        }
    }
}
