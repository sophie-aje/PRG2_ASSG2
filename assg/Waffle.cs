using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assg
{
    class Waffle : IceCream
    {
        public string waffleFlavour { get; set; }

        public Waffle() { }

        public Waffle(string o, int s, List<Flavour> f, List<Topping> t, string wf)
        {
            option = o;
            scoops = s;
            flavours = f;
            toppings = t;
            waffleFlavour = wf;
        }
        public override double CalculatePrice()
        {

        }
        public override string ToString()
        {
            return $"{option}\t{scoops}\t{flavours}\t{toppings}\t{waffleFlavour}";
        }
    }
}
