CREATE PROCEDURE [dbo].[P_Query_GetItems]
	@tableName NVARCHAR(100) = null,
	@ids NVARCHAR(100) = null,
	@dbNames NVARCHAR(100) = null
AS
BEGIN
    DECLARE @actualTableName NVARCHAR(100)

    SELECT @actualTableName = QUOTENAME( TABLE_NAME )
    FROM INFORMATION_SCHEMA.TABLES
    WHERE TABLE_NAME = @tableName

    DECLARE @sqlToken NVARCHAR(255) = 'SELECT TOP 0 * FROM ' + @actualTableName + ';'

	IF @ids IS NOT NULL
	BEGIN
		SELECT @sqlToken = 'SELECT * FROM ' + @actualTableName + ' '
					+ 'WHERE [Id] IN ( ' + @ids + ' )' + ' '
					+ 'AND [DeleteDate] IS NULL;'
	END
	ELSE IF @dbNames IS NOT NULL
	BEGIN
		SELECT @sqlToken = 'SELECT * FROM ' + @actualTableName + ' '
					+ 'WHERE [DBName] IN ( ' + @dbNames + ' )' + ' '
					+ 'AND [DeleteDate] IS NULL;'
	END
    EXEC(@sqlToken)
END
