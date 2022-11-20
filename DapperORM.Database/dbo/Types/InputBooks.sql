CREATE TYPE [dbo].[InputBooks] AS TABLE
(
	[Id] INT NULL, 
    [DBName] NVARCHAR(50) NULL, 
    [Title] NVARCHAR(50) NULL, 
    [Rating] FLOAT NULL,
    [UpdateDate] DATETIME NULL, 
    [DeleteDate] DATETIME NULL
)