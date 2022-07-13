using System;
using System.Collections.Generic;
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
        public List<QryOrder> getOrders()
        {
            using (var db = new Optimization_RWEntities())
            {
                var dt = DateTime.Now;
                
                var orders = db.QryOrders.Where(o => o.order_created_day == dt.Date.ToString()
                && Convert.ToInt32(o.order_created_hour) <= dt.Hour &&
                !(Convert.ToInt32(o.order_created_hour) == dt.Hour && Convert.ToInt32(o.order_created_minute) > dt.Minute)
                ).ToList();


                return orders;
            };
        }
    }
}
