using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RouteOptimization.Controllers
{
    public class WebApiController : ApiController
    {
        [Route("api/GetOrders")]
        [HttpGet]
        public List<QryOrder> GetOrders()
        {
            using (var db = new Optimization_RWEntities())
            {
                var dt_now = DateTime.Now;

                var dateTime = new DateTime(2021, 1, dt_now.Day, dt_now.Hour, dt_now.Minute, dt_now.Second);

                // fetch orders of current hour only
                var orders = db.QryOrders.Where(o => o.order_created_month == 1 &&
                o.order_created_day == dt_now.Day &&
                o.order_created_hour == dt_now.Hour &&
                !(o.order_created_minute > dt_now.Minute) &&
                !o.order_moment_created.Equals("") &&
                !o.order_moment_accepted.Equals("") &&
                !o.order_moment_ready.Equals("") &&
                !o.order_status.Equals("CANCELED")).ToList();

                /**
                 * Consider:
                    * order_moment_created = order recieved
                    * order_moment_accepted = planned
                    * order_moment_ready = out for delivery
                 */

                foreach (var item in orders)
                {
                    var dt_acc = Convert.ToDateTime(item.order_moment_accepted, new CultureInfo("en-US"));
                    var dt_ass = Convert.ToDateTime(item.order_moment_ready, new CultureInfo("en-US"));
                    var dt_del = Convert.ToDateTime(item.order_moment_delivered, new CultureInfo("en-US"));



                    if (dateTime < dt_acc)
                    {
                        item.order_status = "NEW";
                    }
                    else if (dateTime < dt_ass)
                    {
                        item.order_status = "ASSIGNED";
                    }
                    else if (dateTime < dt_del)
                    {
                        item.order_status = "DELIVERING";
                    }
                }



                return orders;
            };
        }
    }
}
