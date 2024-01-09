using System;

public class Class1
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
