//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RouteOptimization
{
    using System;
    using System.Collections.Generic;
    
    public partial class delivery
    {
        public decimal delivery_id { get; set; }
        public Nullable<decimal> delivery_order_id { get; set; }
        public Nullable<decimal> driver_id { get; set; }
        public string delivery_distance_meters { get; set; }
        public string delivery_status { get; set; }
    }
}