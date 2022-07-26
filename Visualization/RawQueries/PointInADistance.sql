-- formula to convert degrees to radians
-- deg * π/180


-- query to find points within a distance from the given points.
--	-- this query accepts inputs in radians
--	-- reference points = (lat, lon) =(1.3963 rad, -0.6981 rad)
--	-- Lat and Lon are the column names
-- SELECT * FROM Places WHERE acos(sin(1.3963) * sin(Lat) + cos(1.3963) * cos(Lat) * cos(Lon - (-0.6981))) * 6371 <= 1000;

DECLARE @ref_lat AS float = -30.0374145507813 * (PI()/180)
DECLARE @ref_long AS float = -51.2035217285156 * (PI()/180)


SELECT * FROM drivers 
WHERE acos(sin(@ref_lat) * sin(current_latitude * (PI()/180)) + 
cos(@ref_lat) * cos(current_latitude * (PI()/180)) * cos(current_longitude * (PI()/180) - (@ref_long))) * 6371 <= 5;

--select * from drivers



-- create stored procedure for getting the riders
CREATE PROCEDURE GetRidersIn5KmRadius
    -- Add the parameters for the stored procedure here
    @_lat AS float = -30.0374145507813,
	@_long AS float = -51.2035217285156
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

	DECLARE @ref_lat AS float = @_lat * (PI()/180);
	DECLARE @ref_long AS float = @_long * (PI()/180);

    -- Insert statements for procedure here
	SELECT * FROM drivers 
	WHERE acos(sin(@ref_lat) * sin(current_latitude * (PI()/180)) + 
	cos(@ref_lat) * cos(current_latitude * (PI()/180)) * cos(current_longitude * (PI()/180) - (@ref_long))) * 6371 <= 5;
END


-- query to call the stored procedure
EXEC GetRidersIn5KmRadius @_lat = -30.0374145507813, @_long = -51.2035217285156;

-- query to get riders with another hub
EXEC GetRidersIn5KmRadius @_lat = -23.592004776001, @_long = -46.6365051269531;