using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assg
{
	class Cup : IceCream
	{
		public Cup() { }
        public Cup(string o, int s, List<Flavour> f, List<Topping> t)
        {
            option = o;
            scoops = s;
            flavours = f;
            toppings = t;
        }
    }
}
