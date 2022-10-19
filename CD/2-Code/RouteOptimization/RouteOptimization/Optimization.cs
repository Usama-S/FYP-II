using RouteOptimization.App_Files;
using RouteOptimization.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace RouteOptimization
{
    public static class Optimization
    {
        // location format used for this project is [lat, long]

        // constant for changing the status of order to "ASSIGNED"
        private const string ORDER_STATUS_ASSIGNED = "ASSIGNED";

        // number of kms upto which the program should search for the riders.
        private const int SIGMA = 5;

        // maximum number of orders a rider can have.\
        private const int MAX_ORDERS = 5;

        // main optimization algorithm.
        public static void Optimize(orders_processed2 order)
        {
            // get restaurant location from the order
            double[] pickup_location = new double[] { (double)order.hub_latitude, (double)order.hub_longitude };

            // get the rides that could possible be the best rider for the job.
            var possible_riders = Filtering(pickup_location);

            // empty array for storing fitness values of the riders.
            var rider_fitnesses = new List<RiderFitness>();

            // for every rider...
            foreach (var item in possible_riders)
            {
                // get the shortest route length after this order.
                var shortestRouteLength = GetShortestRoute(item, order);

                // get the fitness value for this rider with this order.
                var item_fitness = GetFitnessValue((short)item.driver_rating, shortestRouteLength.Sum(s => s.temp_distance));

                // add to the fitness values array for comparison.
                rider_fitnesses.Add(new RiderFitness() { driver_id = item.driver_id, rider_fitness = item_fitness });
            }

            // sort the array of fitness values according to their fitness values and select the first one.
            var rider_id = rider_fitnesses.OrderBy(o => o.rider_fitness).FirstOrDefault().driver_id;

            // assign the current order to the selected rider and change the order status to "ASSIGNED".
            order.driver_id = rider_id;
            order.order_status = ORDER_STATUS_ASSIGNED;
        }

        // Filter out the rider that could possibly be the best rider.
        private static List<driver> Filtering(double[] pickup_location)
        {
            // empty list for riders to be returned
            List<driver> possible_riders = new List<driver>();

            // empty list of riders for all riders got from the database.
            List<driver> all_riders = new List<driver>();

            using (var db = new Optimization_RWEntities())
            {
                // define parameters for the stored procedure to be called.
                SqlParameter[] param = new SqlParameter[] {
                    // new SqlParameter("@lat", -30.0474147),       // hard coded referene points
                    // new SqlParameter("@long", -51.2135086),      // for testing only.
                    new SqlParameter("@lat", pickup_location[0]),
                    new SqlParameter("@long", pickup_location[1])
                };
                // call stored procedure that will return all the riders within an area of 5 km in the form of a list.
                all_riders = db.Database.SqlQuery<driver>("GetRidersIn5KmRadius @lat,@long", param).ToList();

                // filter only the riders having orders less than 5
                for (int i = 0, j = 0; i < all_riders.Count; i++)
                {
                    var rider_id = all_riders[i].driver_id;     // get rider id

                    //// get the order count for this rider
                    //var order_count = db.orders_processed.Where(w => w.driver_id == rider_id &&
                    //!w.order_status.Equals("FINISHED")).Count();

                    // define parameters for the stored procedure to be called.
                    SqlParameter[] param2 = new SqlParameter[] { new SqlParameter("@rider_id", rider_id) };
                    // call stored procedure that will return all the riders within an area of 5 km in the form of a list.
                    // var order_count = db.Database.SqlQuery<Item>("GetRidersCurrentOrderCount @rider_id", param2);

                    var data = db.Database.SqlQuery<int>(@"declare @num int exec @num = GetRidersCurrentOrderCount @rider_id select @num", param2);

                    var order_count = data.First();

                    // if the orders of this rider is less than 5, then add to the possible riders array
                    if (order_count < MAX_ORDERS)
                    {
                        // add current rider to the possible riders array.
                        possible_riders.Add(all_riders[i]);
                        j++;
                    }

                    // j is used to limit the number of rider that are going to be returned.
                    if (j >= 10)
                        break;
                }
            }

            // return the possible riders.
            return possible_riders;
        }

        // get the shortest path/sequence of deliveries for a rider
        public static List<Location> GetShortestRoute(driver rider, orders_processed2 new_order = null)
        {
            // empty list for all the locations to be searched.
            List<Location> locations = new List<Location>();

            // empty list to keep the sequence of locations for the shortest path.
            List<Location> path_sequence = new List<Location>();

            // index variable for path_sequence
            int i = 0;

            // current location of this rider. (format => lat, long)
            double[] ref_location = new double[2] { (double)rider.current_latitude, (double)rider.current_longitude };

            // get all the current orders of this rider.
            // var current_orders = db.orders_processed.Where(w => !w.delivery_status.Equals("FINISHED")).ToList();
            List<orders_processed2> current_orders = UtilityClass.GetUnfinishedOrdersForRider(rider.driver_id);

            // if there is any new order, add it to the list of current orders
            if (new_order != null)
            {
                current_orders.Add(new_order);
            }

            // add all the pickups of the orders to the locations list
            foreach (var item in current_orders)
            {
                locations.Add(new Location()
                {
                    orderId = item.order_id,
                    locationType = 'P',
                    location = new double[2] { (double)item.hub_latitude, (double)item.hub_longitude }
                });
            }

            // loop on until location array is empty
            while (locations.Count != 0)
            {
                // find distance from the reference point for every location.
                foreach (var item in locations)
                {
                    item.temp_distance = Coordinates.distance(ref_location[0], item.location[0], ref_location[1], item.location[1]);
                }

                // get the location with the minimum location from the reference point.
                var min_location = locations.OrderBy(o => o.temp_distance).FirstOrDefault();


                if (min_location != null)
                {
                    // if the last minimum location is a pickup location, then add its respective delivery location.
                    if (min_location.locationType == 'P')
                    {
                        // get the respective delivery location data.
                        var order_delivery = current_orders.FirstOrDefault(f => f.order_id == min_location.orderId);

                        // add the delivery location data to the locations array to be evaluated.
                        locations.Add(new Location()
                        {
                            orderId = order_delivery.order_id,
                            locationType = 'D',
                            location = new double[2] { Convert.ToDouble(order_delivery.store_latitude),
                                Convert.ToDouble(order_delivery.hub_longitude) }
                        });
                    }

                    // change the reference location to the last minimum location.
                    ref_location = min_location.location;

                    // add the current minimum location with its serial number to the path_sequence array.
                    min_location.index = i;
                    path_sequence.Add(min_location);

                    // remove the current minimum from locations array.
                    locations.Remove(min_location);

                    // increment index variable.
                    i++;
                }
            }

            return path_sequence;
        }

        // get the fitness value for a rider.
        private static double GetFitnessValue(short driver_rating, double shortestRouteLength)
        {
            // we are going to minimize the following function.
            // f = total route length / rider rating
            //  -- where  total route length = current route lenght of all orders +
            //                                  route lenght of pickup + 
            //                                  route length of delivery location.

            return (shortestRouteLength / driver_rating);
        }
    }
}