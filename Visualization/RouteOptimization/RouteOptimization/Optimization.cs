using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RouteOptimization
{
    public class Optimization
    {
        // location format used for this project is [lat, long]

        // number of kms upto which the program should search for the riders.
        private static int sigma = 5;
        
        // maximum number of orders a rider can have.
        private static int max_orders = 5;

        public static void Optimize(QryOrder order)
        {
            // get restaurant location from the order
            double[] pickup_location = new double[] { (double)order.hub_latitude, (double)order.hub_longitude };

            // get the rides that could possible be the best rider for the job.
            // var possible_riders = Filtering();
        }

        private static List<driver> Filtering(double[] pickup_location)
        {
            // empty list for riders
            List<driver> possible_riders = new List<driver>();

            List<driver> all_riders = new List<driver>();

            using (var db = new Optimization_RWEntities())
            {
                all_riders = db.GetRidersIn5KmRadius(pickup_location[0], pickup_location[1]);
            }

            for (int i = 0; i < possible_riders.Count; i++)
            {

            }

            return null;
        }

        public static void GetShortestRoute()
        {
            // your code here...
        }

        private static void GetFitnessValue()
        {
            // your code here...
        }
    }
}