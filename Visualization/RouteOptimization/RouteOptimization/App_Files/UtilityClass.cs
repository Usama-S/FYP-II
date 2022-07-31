using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RouteOptimization.App_Files
{
    public static class UtilityClass
    {
        // get all the unfinished orders of currentRider
        public static List<orders_processed> GetUnfinishedOrdersForRider(decimal driver_id)
        {
            using (var db = new Optimization_RWEntities())
            {
                var orders = db.orders_processed.Where(w => !w.delivery_status.Equals("FINISHED") &&
                w.driver_id == driver_id).ToList();

                return orders;
            }
        }
    }
}