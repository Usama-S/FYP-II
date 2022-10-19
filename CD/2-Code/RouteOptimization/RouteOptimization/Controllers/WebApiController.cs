using RouteOptimization.App_Files;
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
        public List<orders_processed2> GetOrders()
        {
            var orders = UtilityClass.GetCurrentHourOrders();
            return orders;
        }


        // NOT BEING USED CURRENTLY
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

        [Route("api/GetRiders")]
        [HttpGet]
        public Object GetRiders()
        {
            using (var db = new Optimization_RWEntities())
            {
                // to initiate the optimization process
                var orders = UtilityClass.GetCurrentHourOrders();

                var riders = UtilityClass.GetCurrentActiveDrivers();

                return new { riders = riders, orders = orders };
            }
        }


        [Route("api/GetDeliveryLocations")]
        [HttpGet]
        public List<driver> GetDeliveryLocations()
        {
            using (var db = new Optimization_RWEntities())
            {
                var riders = UtilityClass.GetCurrentActiveDrivers();

                return riders;
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
