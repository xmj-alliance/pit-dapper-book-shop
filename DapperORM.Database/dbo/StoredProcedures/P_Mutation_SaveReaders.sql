CREATE PROCEDURE [dbo].[P_Mutation_SaveReaders]
	@inputReaders dbo.[InputReaders] READONLY
AS
BEGIN TRANSACTION

	SET NOCOUNT ON

	-- Store the inserted IDs later
	DECLARE @affectedIds dbo.IDs

	-- === Fill the inputs with default values ===

	DECLARE @processedInputReaders dbo.[InputReaders]

	INSERT INTO @processedInputReaders
	SELECT *
	FROM @inputReaders

	-- attach guid for null dbnames
	UPDATE @processedInputReaders
	SET [DBName] = NEWID()
	WHERE [DBName] IS NULL

	-- set default values
	UPDATE @processedInputReaders
	SET [IsAdult] = 0
	WHERE [IsAdult] IS NULL

	UPDATE @processedInputReaders
	SET [Credit] = 0.0
	WHERE [Credit] IS NULL

	UPDATE @processedInputReaders
	SET [UpdateDate] = GETDATE()
	WHERE [UpdateDate] IS NULL

	--  === Select Readers without id, then do insert === 
	INSERT INTO Readers
	OUTPUT inserted.Id INTO @affectedIds
	SELECT 
		[DBName],
		[FirstName],
		[LastName],
		[IsAdult],
		[Phone],
		[Credit],
		[UpdateDate],
		[DeleteDate]
	FROM @processedInputReaders
	WHERE [Id] IS NULL

	--  === Select Readers with id, then do replace (update) === 
	UPDATE Readers

	SET [DBName] = inputReaders.[DBName],
		[FirstName] = inputReaders.[FirstName],
		[LastName] = inputReaders.[LastName],
		[IsAdult] = inputReaders.[IsAdult],
		[Phone] = inputReaders.[Phone],
		[Credit] = inputReaders.[Credit],
		[UpdateDate] = GETDATE(),
		[DeleteDate] = inputReaders.[DeleteDate]

	OUTPUT INSERTED.Id INTO @affectedIds

	FROM Readers targetReaders
		INNER JOIN @processedInputReaders inputReaders ON inputReaders.Id = targetReaders.Id

	-- return the inserted/updated ID
	SELECT * FROM @affectedIds

COMMIT