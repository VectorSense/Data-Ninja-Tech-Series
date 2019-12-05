--Be sure to connect to your labs database, 
--not the master database

--Lab 04 Exercise 2 code

--Create external dependencies
CREATE MASTER KEY;
CREATE DATABASE SCOPED CREDENTIAL LabStorageCredential
WITH
    IDENTITY = 'user',
    SECRET = '<key>'
;

CREATE EXTERNAL DATA SOURCE LabStorage
WITH (
    TYPE = HADOOP,
    LOCATION = 'wasbs://<container>@<account>.blob.core.windows.net',
    CREDENTIAL = LabStorageCredential
);

CREATE EXTERNAL FILE FORMAT TextFile
WITH (
    FORMAT_TYPE = DelimitedText,
    FORMAT_OPTIONS (FIELD_TERMINATOR = ',')
);

--Lab 04 Exercise 3 code

--Create external table for raw data
CREATE EXTERNAL TABLE dbo.BeachSensorsExternal (
	StationName VARCHAR(50) NOT NULL,
	MeasurementTimestamp VARCHAR(50) NOT NULL,
	AirTemperature DECIMAL(9,2) NULL,
	WetBulbTemperature DECIMAL(9,2) NULL,
	Humidity DECIMAL(9,2) NULL,
	RainIntensity DECIMAL(9,2) NULL,
	IntervalRain DECIMAL(9,2) NULL,
	TotalRain DECIMAL(9,2) NULL,
	PrecipitationType DECIMAL(9,2) NULL,
	WindDirection DECIMAL(9,2) NULL,
	WindSpeed DECIMAL(9,2) NULL,
	MaximumWindSpeed DECIMAL(9,2) NULL,
	BarometricPressure DECIMAL(9,2) NULL,
	SolarRadiation DECIMAL(9,2) NULL,
	Heading DECIMAL(9,2) NULL,
	BatteryLife DECIMAL(9,2) NULL,
	MeasurementTimestampLabel VARCHAR(50) NOT NULL,
	MeasurementID VARCHAR(100) NOT NULL
)
WITH (
    LOCATION='/', -- If a folder was specified during upload, reference it here
    DATA_SOURCE=LabStorage,
    FILE_FORMAT=TextFile
);
--Test external table
SELECT TOP 10 * FROM dbo.BeachSensorsExternal;

--Create new table for transformed data using CTAS
CREATE TABLE dbo.BeachSensor
WITH
(   
    CLUSTERED COLUMNSTORE INDEX,
    DISTRIBUTION = ROUND_ROBIN
)
AS
SELECT StationName,
	CAST(MeasurementTimestamp as DATETIME) AS MeasurementDateTime,
	AirTemperature,
	WetBulbTemperature,
	Humidity,
	RainIntensity,
	IntervalRain,
	TotalRain,
	PrecipitationType,
	WindDirection,
	WindSpeed,
	MaximumWindSpeed,
	BarometricPressure,
	SolarRadiation,
	Heading,
	BatteryLife
FROM dbo.BeachSensorsExternal;

SELECT COUNT(*)
FROM dbo.BeachSensor;

