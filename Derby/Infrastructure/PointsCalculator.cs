using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Derby.Infrastructure
{
    public class PointsCalculator
    {
        public static int Calculate(int laneCount, int place)
        {
            // Needs to return the reward value of the place. 
            // If there are 6 lanes, racer gets 1st place, the net score would be 6.
            // The last place always gets one point
            return (laneCount + 1) - place;
        }
    }
}