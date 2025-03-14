﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assg
{
    class PointCard
    {
        public int points { get; set; }
        public int punchCard { get; set; }
        public string tier { get; set; }

        public PointCard() { }
        public PointCard(int po, int pu)
        {
            points = po;
            punchCard = pu;
        }
        public void AddPoints(int add)
        {
            int earnedPoints = Convert.ToInt32(Math.Floor(add * 0.72));
            points += earnedPoints;
        }

        public void RedeemPoints(double totalBill)
        {
            if (tier == "Gold" || tier == "Silver")
            {
                bool validInput = false;

                while (!validInput)
                {
                    Console.WriteLine("How many points do you want to use to offset the bill?");
                    try
                    {
                        double amount = Convert.ToDouble(Console.ReadLine());

                        if (amount > points)
                        {
                            Console.WriteLine("Offset amount cannot exceed total points. Please enter a valid amount.");
                        }
                        else
                        {
                            validInput = true; // Set flag to exit the loop
                            points -= Convert.ToInt32(amount); // Deduct redeemed points from the total points
                            totalBill -= amount * 0.02;
                            Console.WriteLine($"Final Total Bill Amount: ${totalBill:F2}");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Please enter a valid number.");
                    }


                }
            }
            else
            {
                Console.WriteLine("Membership status not applicable to redeem points yet");
                // or throw an exception, depending on your design
            }
        }



        public void Punch()
        {
            punchCard++;
            if (punchCard > 10)
            {
                punchCard = 0;
            }
        }


        public override string ToString()
        {
            return $"";
        }

    }

}
