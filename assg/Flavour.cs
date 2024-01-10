using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assg
{
    class Flavour
    {
        public string type { get; set; }
        public bool premium { get; set; }
        public int quantity { get; set; }
        public Flavour() { }
        public Flavour(string t, bool p, int q)
        {
            type = t;
            premium = p;
            quantity = q;
        }
        public override string ToString()
        {
            return $"{type}\t{premium}\t{quantity}";
        }

    }
}
