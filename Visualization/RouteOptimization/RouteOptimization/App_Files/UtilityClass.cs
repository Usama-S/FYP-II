using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace RouteOptimization.App_Files
{
    public static class UtilityClass
    {
        // constant for Order statuses
        private const string ORDER_STATUS_NEW = "NEW";
        private const string ORDER_STATUS_DELIVERING = "DELIVERING";
        private const string ORDER_STATUS_FINISHED = "FINISHED";

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

        // get all the orders of last one hour.
        public static List<orders_processed> GetCurrentHourOrders()
        {
            using (var db = new Optimization_RWEntities())
            {
                var dt_now = DateTime.Now;

                //var dateTime = new DateTime(2021, 1, dt_now.Day, dt_now.Hour, dt_now.Minute, dt_now.Second);
                var dateTime = new DateTime(2021, 1, dt_now.Day, 21, dt_now.Minute, dt_now.Second);

                db.Database.CommandTimeout = 500;

                // fetch orders of current hour only
                var orders = db.orders_processed.Where(o => o.order_created_month == 1 &&
                o.order_created_day == dt_now.Day &&
                ((o.order_created_hour == dt_now.Hour && !(o.order_created_minute > dt_now.Minute)) ||              // select orders of 
                (o.order_created_hour == (dt_now.Hour - 1) && !(o.order_created_minute < dt_now.Minute))) &&        // last 1 hour.
                !o.order_moment_created.Equals("") &&
                !o.order_moment_accepted.Equals("") &&
                !o.order_moment_ready.Equals("")).ToList();


                /***
                 * removed from above:
                    .OrderBy(o => o.order_created_day)
                    .ThenBy(t => t.order_created_hour)
                    .ThenBy(th => th.order_created_minute)
                 * 
                 * */


                foreach (var item in orders)
                {
                    // get all comparable times of the order.
                    var dt_cre = Convert.ToDateTime(item.order_moment_created, new CultureInfo("en-US"));
                    var dt_acc = Convert.ToDateTime(item.order_moment_accepted, new CultureInfo("en-US"));
                    var dt_ass = Convert.ToDateTime(item.order_moment_ready, new CultureInfo("en-US"));
                    var dt_del = Convert.ToDateTime(item.order_moment_delivered, new CultureInfo("en-US"));


                    // if no driver id is assigned and time is less than order_moment_accepted, then the order is NEW
                    if (item.driver_id == null && dateTime < dt_acc)
                        item.order_status = ORDER_STATUS_NEW;
                    else if (item.driver_id == null && dateTime >= dt_acc)  // if the time is greater than order_moment_accepted, send the order to optimization algorithm.
                        Optimization.Optimize(item);
                    else
                    {
                        // if the order is assigned some driver id, set the status according to time.
                        if (dateTime < dt_del)
                        {
                            item.order_status = ORDER_STATUS_DELIVERING;
                        }
                        else if (dateTime >= dt_del)
                        {
                            item.order_status = ORDER_STATUS_FINISHED;
                        }
                    }
                }

                // save the changes in the database.
                // db.SaveChanges();

                return orders;
            }
        }
    }
}