using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Helper
{
    public static class GlobalMemory
    {
        public static User _user { get; set; }
        public static List<User> users { get; set; }
    }
}
