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

        public override double CalculatePrice(double price)
        {
            price = 0.0;
            if (dipped == true)
            {
                price += 2.00;
                if (scoops == 1)
                {
                    price = 4.00;
                }
                else if (scoops == 2)
                {
                    price = 5.50;
                }
                else if (scoops == 3)
                {
                    price = 6.50;
                }

            }
            else
            {

            }
        }

        public override string ToString()
        {
            return $"{option}\t{scoops}\t{flavours}\t{toppings}\t{CalculatePrice()}";
        }
    }
}
