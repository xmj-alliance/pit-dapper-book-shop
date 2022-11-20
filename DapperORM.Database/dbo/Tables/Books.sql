CREATE TABLE [dbo].[Books]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [DBName] NVARCHAR(50) NOT NULL, 
    [Title] NVARCHAR(50) NULL, 
    [Rating] FLOAT NOT NULL ,
    [UpdateDate] DATETIME NOT NULL , 
    [DeleteDate] DATETIME NULL, 
    CONSTRAINT UC_Books UNIQUE (DBName)
)
