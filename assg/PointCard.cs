using System;
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
        public PointCard(int Points, int PunchCard)
        {
            points = Points;
            punchCard = PunchCard;
        }
        public void AddPoints(int add)
        {
            points = points + add;
        }

        public void RedeemPoints(int redeem)
        {
            points = points + redeem;
        }

        public void Punch() { }

        public override string ToString()
        {
            return "";
        }
    }

}
