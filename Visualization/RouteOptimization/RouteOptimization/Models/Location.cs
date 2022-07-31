using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RouteOptimization.Models
{
    public class Location
    {
        public int index { get; set; }          // serial no of delivery.
        public int orderId { get; set; }
        public char locationType { get; set; }  // P: for pickup and D: for delivery
        public double[] location { get; set; }
        public double temp_distance { get; set; }  // temporary distance from the reference point. just for calculation only.
    }
}