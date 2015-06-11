using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Lib
{
    public class Offset
    {
        public static Dictionary<int, double> GetDictionary()
        {
             Dictionary<int, double> dictionary = new Dictionary<int, double>();
            dictionary.Add(60, 68);
            dictionary.Add(40,110);
            dictionary.Add(20,262);
            dictionary.Add(65,65);
            dictionary.Add(70,55);
            dictionary.Add(75,50);
            dictionary.Add(80,29);
            dictionary.Add(55,78);
            dictionary.Add(50,85);
            dictionary.Add(45,96);
            dictionary.Add(35, 135);
            dictionary.Add(30, 164);
            return dictionary;
        }

        public static int GetClosestOffset(double radius)
        {
            Tuple<double, KeyValuePair<int, double>> bestMatch = null;
            int closestOffset = int.MaxValue;
            double diff = 0;
            foreach( var e in GetDictionary() )
            {
                double t  = Math.Abs(radius - e.Value);
                if (t < diff || diff == 0)
                {
                    diff = t;
                    closestOffset = e.Key;
                }
            }
            return closestOffset;
        }

    }
}
