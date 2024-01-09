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
        public override double CalculatePrice(double price)
        {
            if (scoops == 1)
            {
                price = 7.00;
            }
            else if (scoops == 2)
            {
                price = 8.50;
            }
            else if (scoops == 3)
            {
                price = 9.50;
            }
            else if (waffleFlavour == 'Pandan', 'charcoal','red velvet')
            {
                price += 3.00;
            }
        }
        public override string ToString()
        {
            return $"{base.ToString()}\t{waffleFlavour}";

        }
    }
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

        public override double CalculatePrice()
        {

        }

        public override string ToString()
        {
            return $"{option}\t{scoops}\t{flavours}\t{toppings}";
        }
    }
}
