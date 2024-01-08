using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assg
{
    class IceCream
    {
        public string option { get; set; }
        public int scoops { get; set; }
        public List<Flavour> flavours { get; set; }
        = new List<Flavour>();

        public List<Topping> toppings { get; set; }

        public IceCream() { }

    }
}
