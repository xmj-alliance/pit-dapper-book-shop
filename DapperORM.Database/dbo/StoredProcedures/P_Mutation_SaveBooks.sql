CREATE PROCEDURE [dbo].[P_Mutation_SaveBooks]
	@inputBooks dbo.[InputBooks] READONLY
AS
BEGIN TRANSACTION

	SET NOCOUNT ON

	-- Store the inserted IDs later
	DECLARE @affectedIds dbo.IDs

	-- === Fill the inputs with default values ===

	DECLARE @processedInputBooks dbo.[InputBooks]

	INSERT INTO @processedInputBooks
	SELECT *
	FROM @inputBooks

	-- attach guid for null dbnames
	UPDATE @processedInputBooks
	SET DBName = NEWID()
	WHERE DBName IS NULL

	-- set default values
	UPDATE @processedInputBooks
	SET [Rating] = 0.0
	WHERE [Rating] IS NULL

	UPDATE @processedInputBooks
	SET [UpdateDate] = GETDATE()
	WHERE [UpdateDate] IS NULL

	--  === Select books without id, then do insert === 
	INSERT INTO Books
	OUTPUT inserted.Id INTO @affectedIds
	SELECT 
		[DBName],
		[Title],
		[Rating],
		[UpdateDate],
		[DeleteDate]
	FROM @processedInputBooks
	WHERE Id is null

	--  === Select books with id, then do replace (update) === 
	UPDATE Books

	SET [DBName] = inputBooks.[DBName],
		[Title] = inputBooks.[Title],
		[Rating] = inputBooks.[Rating],
		[UpdateDate] = GETDATE(),
		[DeleteDate] = inputBooks.[DeleteDate]

	OUTPUT INSERTED.Id INTO @affectedIds

	FROM Books targetBooks
		INNER JOIN @processedInputBooks inputBooks ON inputBooks.Id = targetBooks.Id

	-- return the inserted/updated ID
	SELECT * FROM @affectedIds

COMMIT