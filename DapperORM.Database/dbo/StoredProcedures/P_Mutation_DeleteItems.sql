CREATE PROCEDURE [dbo].[P_Mutation_DeleteItems]
	@tableName NVARCHAR(100) = null,
	@ids NVARCHAR(100) = null,
	@dbNames NVARCHAR(100) = null
AS
BEGIN TRANSACTION

    DECLARE @actualTableName NVARCHAR(100)

    SELECT @actualTableName = QUOTENAME( TABLE_NAME )
    FROM INFORMATION_SCHEMA.TABLES
    WHERE TABLE_NAME = @tableName

    DECLARE @sqlToken NVARCHAR(255);

	IF @ids IS NOT NULL
	BEGIN
		SELECT @sqlToken = 'UPDATE ' + @actualTableName + ' '
					+ 'SET [DeleteDate] = ' + '''' + CAST(GETDATE() as nvarchar(100)) + ''''  + ' '
					+ 'WHERE [Id] IN ( ' + @ids + ' );'
	END
	ELSE IF @dbNames IS NOT NULL
	BEGIN
		SELECT @sqlToken = 'UPDATE ' + @actualTableName + ' '
					+ 'SET [DeleteDate] = ' + '''' + CAST(GETDATE() as nvarchar(100)) + ''''  + ' '
					+ 'WHERE [DBName] IN ( ' + @dbNames + ' );'
	END

    EXEC(@sqlToken)

COMMIT
