CREATE TABLE [dbo].[Records]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [ReaderID] INT NOT NULL, 
    [BookID] INT NOT NULL, 
    [StartDate] DATETIME NULL DEFAULT GETDATE(), 
    [EndDate] DATETIME NULL,
    [UpdateDate] DATETIME NULL DEFAULT GETDATE(), 
    [DeleteDate] DATETIME NULL,
    CONSTRAINT [FK_Record_ToBooks] FOREIGN KEY ([BookID]) REFERENCES [Books]([Id]),
    CONSTRAINT [FK_Record_ToReaders] FOREIGN KEY ([ReaderID]) REFERENCES [Readers]([Id])
)
