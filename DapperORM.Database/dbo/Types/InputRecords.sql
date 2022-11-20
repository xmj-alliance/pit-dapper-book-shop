CREATE TYPE [dbo].[InputRecords] AS TABLE
(
	[Id] INT NULL, 
    [ReaderID] INT NULL, 
    [BookID] INT NULL, 
    [StartDate] DATETIME NULL, 
    [EndDate] DATETIME NULL,
    [UpdateDate] DATETIME NULL, 
    [DeleteDate] DATETIME NULL
)