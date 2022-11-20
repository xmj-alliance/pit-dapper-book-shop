CREATE TYPE [dbo].[InputReaders] AS TABLE
(
	[Id] INT NULL, 
    [DBName] NVARCHAR(100) NULL, 
    [FirstName] NVARCHAR(50) NULL, 
    [LastName] NVARCHAR(50) NULL, 
    [IsAdult] BIT NULL, 
    [Phone] NVARCHAR(50) NULL, 
    [Credit] DECIMAL NULL,
    [UpdateDate] DATETIME NULL, 
    [DeleteDate] DATETIME NULL
)