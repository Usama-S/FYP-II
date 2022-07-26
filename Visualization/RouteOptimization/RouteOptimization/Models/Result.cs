using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RouteOptimization.Models
{
    public class Result
    {
        // riders
        driver driver;

        // each rider has a list of orders
        List<QryOrder> rider_orders = new List<QryOrder>();
    }
}