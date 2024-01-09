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

        public Cone(string o, int s, List<Flavour> f, List<Topping> t, bool d)
        {
            option = o;
            scoops = s;
            flavours = f;
            toppings = t;
            dipped = d;
        }

        public override double CalculatePrice()
        {

        }

        public override string ToString()
        {
            return $"{option}\t{scoops}\t{flavours}\t{toppings}\t{dipped}";
        }
    }
}
