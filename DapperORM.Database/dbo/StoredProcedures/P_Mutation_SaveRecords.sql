CREATE PROCEDURE [dbo].[P_Mutation_SaveRecords]
	@inputRecords dbo.[InputRecords] READONLY
AS
BEGIN TRANSACTION

	SET NOCOUNT ON

	-- Store the inserted IDs later
	DECLARE @affectedIds dbo.IDs

	-- === Fill the inputs with default values ===

	DECLARE @processedInputRecords dbo.[InputRecords]

	INSERT INTO @processedInputRecords
	SELECT *
	FROM @inputRecords

	-- set default values
	UPDATE @processedInputRecords
	SET [StartDate] = GETDATE()
	WHERE [StartDate] IS NULL

	UPDATE @processedInputRecords
	SET [UpdateDate] = GETDATE()
	WHERE [UpdateDate] IS NULL

	--  === Select Records without id, then do insert === 
	INSERT INTO Records
	OUTPUT inserted.Id INTO @affectedIds
	SELECT 
		[ReaderID],
		[BookID],
		[StartDate],
		[EndDate],
		[UpdateDate],
		[DeleteDate]
	FROM @processedInputRecords
	WHERE [Id] IS NULL

	--  === Select Records with id, then do replace (update) === 
	UPDATE Records
	SET
		[ReaderID] = inputRecords.[ReaderID],
		[BookID] = inputRecords.[BookID],
		[StartDate] = inputRecords.[StartDate],
		[EndDate] = inputRecords.[EndDate],
		[UpdateDate] = GETDATE(),
		[DeleteDate] = inputRecords.[DeleteDate]

	OUTPUT INSERTED.Id INTO @affectedIds

	FROM Records targetRecords
		INNER JOIN @processedInputRecords inputRecords ON inputRecords.Id = targetRecords.Id

	-- return the inserted/updated ID
	SELECT * FROM @affectedIds

COMMIT