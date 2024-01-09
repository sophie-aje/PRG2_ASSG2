using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assg
{
    class Topping
    {
        public string type { get; set; }
        public Topping() { }
        public Topping(string t)
        {
            type = t;
        }
        public override string ToString()
        {
            return "";
        }
    }
}
