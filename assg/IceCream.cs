﻿using assg;

abstract class IceCream
{
    public string option { get; set; }
    public int scoops { get; set; }
    public List<Flavour> flavours { get; set; } = new List<Flavour>();

    public List<Topping> toppings { get; set; } = new List<Topping>();

    public IceCream() { }
    public IceCream(string o, int s, List<Flavour> f, List<Topping> t)
    {
        option = o;
        scoops = s;
        flavours = f;
        toppings = t;
    }
    public abstract double CalculatePrice();

    public override string ToString()
    {
        return $"{option}\t{scoops}\t{flavours}\t{toppings}";
    }
}