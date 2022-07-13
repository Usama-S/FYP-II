-- for all orders before now
select * from orders where order_created_day <= 07 and order_created_hour <= 12 and order_created_minute <= 34

-- for all orders of today till now
select * from QryOrders 
where order_created_day = 13 and
order_created_hour <= 22 and
not (order_created_hour = 22 and order_created_minute > 45)

-- for all orders of last hour
select * from orders where order_created_day = 07 and order_created_hour = 12 and order_created_minute <= 34

-- Query for View (QryOrders)
SELECT        dbo.orders.order_id, dbo.orders.store_id, dbo.stores.store_name, dbo.stores.store_latitude, dbo.stores.store_longitude, dbo.stores.hub_id, dbo.hubs.hub_name, dbo.hubs.hub_city, dbo.hubs.hub_state, dbo.hubs.hub_latitude, 
                         dbo.hubs.hub_longitude, dbo.deliveries.delivery_id, dbo.deliveries.delivery_distance_meters, dbo.deliveries.delivery_status, dbo.deliveries.driver_id, dbo.drivers.current_latitude, dbo.drivers.current_longitude, 
                         dbo.drivers.driver_rating, dbo.drivers.current_orders, dbo.orders.order_status, dbo.orders.order_created_hour, dbo.orders.order_created_minute, dbo.orders.order_created_day, dbo.orders.order_created_month, 
                         dbo.orders.order_created_year, dbo.orders.order_moment_created, dbo.orders.order_moment_accepted, dbo.orders.order_moment_ready, dbo.orders.order_moment_in_expedition, dbo.orders.order_moment_collected, 
                         dbo.orders.order_moment_delivering, dbo.orders.order_moment_delivered, dbo.orders.order_moment_finished, dbo.orders.order_metric_collected_time, dbo.orders.order_metric_paused_time, 
                         dbo.orders.order_metric_production_time, dbo.orders.order_metric_walking_time, dbo.orders.order_metric_expediton_speed_time, dbo.orders.order_metric_transit_time, dbo.orders.order_metric_cycle_time
FROM            dbo.orders INNER JOIN
                         dbo.stores ON dbo.orders.store_id = dbo.stores.store_id INNER JOIN
                         dbo.drivers INNER JOIN
                         dbo.deliveries ON dbo.drivers.driver_id = dbo.deliveries.driver_id ON dbo.orders.order_id = dbo.deliveries.delivery_order_id INNER JOIN
                         dbo.hubs ON dbo.stores.hub_id = dbo.hubs.hub_id








