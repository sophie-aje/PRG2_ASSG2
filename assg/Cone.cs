using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assg
{
	class Cone : IceCream
	{
		public bool dipped { get; set; }
		public Cone() { }

        public Cone(bool d, int s, List<Flavour> t, List<Topping> f, string o)
        {
            dipped = d;
            scoops = s;
            toppings = t;
            flavours = f;
            option = o;
        }

        public override double CalculatePrice()
        {

        }
    }
}
