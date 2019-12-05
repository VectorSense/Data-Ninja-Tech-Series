-- Exercise 1
--System Catalog Views
SELECT 	t.name AS table_name, s.name as schema_name, p.distribution_policy_desc
FROM sys.tables t JOIN sys.schemas s
	ON t.schema_Id = s.schema_id
JOIN sys.pdw_table_distribution_properties p
	ON t.object_id = p.object_id
WHERE s.name = 'labs';


--Exercise 2
--Session Requests
SELECT s.session_id, s.login_name, r.request_id, r.status, r.command 
FROM sys.dm_pdw_exec_sessions s JOIN sys.dm_pdw_exec_requests r 
ON s.session_id = r.session_id 
WHERE s.session_id ='<your sessions id>';

-- Table Mappings


SELECT t.name as table_name, i.name as index_name, m.physical_name
FROM sys.tables t JOIN sys.schemas s ON t.schema_id = s.schema_id
JOIN sys.indexes i ON t.object_id=i.object_id 
JOIN sys.pdw_table_mappings m ON  t.object_id=m.object_id
JOIN sys.pdw_nodes_tables n ON n.name =m.physical_name  
WHERE s.name ='labs';

-- Labeled Queries

INSERT INTO [labs].[DimDate]
           ([DateKey]
           ,[FullDateAlternateKey]
           ,[DayNumberOfWeek]
           ,[EnglishDayNameOfWeek]
           ,[SpanishDayNameOfWeek]
           ,[FrenchDayNameOfWeek]
           ,[DayNumberOfMonth]
           ,[DayNumberOfYear]
           ,[WeekNumberOfYear]
           ,[EnglishMonthName]
           ,[SpanishMonthName]
           ,[FrenchMonthName]
           ,[MonthNumberOfYear]
           ,[CalendarQuarter]
           ,[CalendarYear]
           ,[CalendarSemester]
           ,[FiscalQuarter]
           ,[FiscalYear]
           ,[FiscalSemester])
SELECT * from dbo.dimdate
OPTION (LABEL='Lab step query');

-- Lab Step Query

SELECT  * 
FROM    sys.dm_pdw_exec_requests r
WHERE   r.[label] = 'Lab step query';

--Exercise 3
--Distribution columns

SELECT	t.name AS table_name, 
		s.name as schema_name, 
		c.name AS col_name, 
		p.distribution_policy_desc, 
		d.distribution_ordinal
FROM sys.tables t JOIN sys.schemas s
	ON t.schema_Id = s.schema_id
JOIN sys.pdw_table_distribution_properties p
	ON t.object_id = p.object_id
JOIN sys.columns c ON t.object_id=c.object_id
JOIN sys.pdw_column_distribution_properties d 
	ON c.object_id = d.object_id and c.column_id=d.column_id 
WHERE s.name = 'labs' and d.distribution_ordinal =1;

-- Exercise 4
--CTAS

CREATE TABLE labs.FactInternetSales_CTAS
WITH
(
	DISTRIBUTION = HASH ( OrderDateKey ),
	CLUSTERED COLUMNSTORE INDEX,
	PARTITION
	(
		[OrderDateKey] RANGE RIGHT FOR VALUES (20000101, 20010101, 20020101, 20030101, 20040101, 20050101, 20060101, 20070101, 20080101, 20090101, 20100101, 20110101, 20120101, 20130101, 20140101, 20150101, 20160101, 20170101, 20180101, 20190101, 20200101, 20210101, 20220101, 20230101, 20240101, 20250101, 20260101, 20270101, 20280101, 20290101)
	)
)

AS SELECT * FROM dbo.FactInternetSales 
OPTION (LABEL='CTAS - FactInternetSales');

--Space used
DBCC PDW_SHOWSPACEUSED('labs.FactInternetSales_CTAS');

--Exercise 5
--Create Dates table

CREATE TABLE labs.dates
    (
        DateId INT NOT NULL,
        DayNum TINYINT NOT NULL,
		MonthNum TINYINT NOT NULL,
        YearNum INT NOT NULL
    )
WITH
    (
        CLUSTERED COLUMNSTORE INDEX,
        DISTRIBUTION = ROUND_ROBIN
);


