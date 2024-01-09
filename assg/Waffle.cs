using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assg
{
    internal class Waffle:IceCream
    {
        public string waffleFlavour { get; set; }
        public Waffle() { } 
        public Waffle(string wf, int s, List<Topping> t, List<Flavour> f,string o)
        {
            waffleFlavour = wf;
            scoops = s;
            toppings = t;
            flavours = f;
            option = o;
        }
        public override double CalculatePrice()
        {

        }
        public override string ToString()
        {
            return base.ToString() + 
                "Waffle Flavour: " + waffleFlavour;

        }
    }
}
