using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFish
{
    public class CustomException : Exception
    {
        private string meg;
        public string m_msg { get { return meg; } }
        public CustomException() { }
        public CustomException(string a) { meg = a; }
    }
}
