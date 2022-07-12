-- for all orders before now
select * from orders where order_created_day <= 07 and order_created_hour <= 12 and order_created_minute <= 34

-- for all orders of today till now
select * from orders where order_created_day = 07 and order_created_hour <= 12 and order_created_minute <= 34

-- for all orders of last hour
select * from orders where order_created_day = 07 and order_created_hour = 12 and order_created_minute <= 34