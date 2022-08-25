﻿using RouteOptimization.App_Files;
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
        public List<orders_processed> GetOrders()
        {
            var orders = UtilityClass.GetCurrentHourOrders();
            return orders;
            //using (var db = new Optimization_RWEntities())
            //{


            //    // where order dirver_id = null
            //    //      check order_moment_ready   
            //    //          send for optimization
            //    // else 
            //    //      







            //    foreach (var item in orders)
            //    {
            //        var dt_cre = Convert.ToDateTime(item.order_moment_created, new CultureInfo("en-US"));
            //        var dt_acc = Convert.ToDateTime(item.order_moment_accepted, new CultureInfo("en-US"));
            //        var dt_ass = Convert.ToDateTime(item.order_moment_ready, new CultureInfo("en-US"));
            //        var dt_del = Convert.ToDateTime(item.order_moment_delivered, new CultureInfo("en-US"));



            //        if (dateTime < dt_acc)
            //        {
            //            item.order_status = "NEW";
            //        }
            //        else if (dateTime < dt_acc)
            //        {
            //            item.order_status = "NEW";
            //        }
            //        else if (dateTime < dt_ass)
            //        {
            //            item.order_status = "ASSIGNED";
            //        }
            //        else if (dateTime < dt_del)
            //        {
            //            item.order_status = "DELIVERING";
            //        }
            //    }



            //    return orders;
            //};
        }


        [Route("api/GetLocations")]
        [HttpGet]
        public Locations GetLocations()
        {
            using (var db = new Optimization_RWEntities())
            {
                var customers = db.stores.ToList();
                var restaurants = db.hubs.ToList();
                var riders = db.drivers.Take(10).ToList();

                return new Locations() { restaurants = restaurants };
            }
        }

        [Route("api/GetRestaurants")]
        [HttpGet]
        public List<hub> GetRestaurants()
        {
            using (var db = new Optimization_RWEntities())
            {
                var restaurants = db.hubs.ToList();

                return restaurants;
            }
        }
    }

    public class Locations
    {
        public List<store> customers = new List<store>();
        public List<hub> restaurants = new List<hub>();
        public List<driver> riders = new List<driver>();
    }
}



/*** mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm Old GetOrders code mmmmmmmmmmmmmmmmmmmmmmmmmmmmmm.
 * public List<QryOrder> GetOrders()
        {
            using (var db = new Optimization_RWEntities())
            {
                var dt_now = DateTime.Now;

                var dateTime = new DateTime(2021, 1, dt_now.Day, dt_now.Hour, dt_now.Minute, dt_now.Second);

                // fetch orders of current hour only
                var orders = db.QryOrders.Where(o => o.order_created_month == 1 &&
                o.order_created_day == dt_now.Day &&
                ((o.order_created_hour == dt_now.Hour && !(o.order_created_minute > dt_now.Minute)) ||              // select orders of 
                (o.order_created_hour == (dt_now.Hour - 1) && !(o.order_created_minute < dt_now.Minute))) &&        // last 1 hour.
                !o.order_moment_created.Equals("") &&
                !o.order_moment_accepted.Equals("") &&
                !o.order_moment_ready.Equals("") &&
                !o.order_status.Equals("CANCELED")).ToList();



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

**/