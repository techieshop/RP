CREATE PROCEDURE [acc].[spInitPigeonFilter]
	@SearchRegEx NVARCHAR(MAX) = '' OUTPUT,
	@JoinClause NVARCHAR(MAX) = '' OUTPUT,
	@WhereClause NVARCHAR(MAX) = '' OUTPUT,
	@OrderClause NVARCHAR(MAX) = '' OUTPUT,
	@EntityStateIds [udt].[KeyList] READONLY,
	@Search NVARCHAR(MAX),
	@SexIds [udt].[KeyList] READONLY
AS
BEGIN
	SET @SearchRegEx = '%' + @Search + '%';

	--> Filtering
	IF EXISTS (SELECT * FROM @EntityStateIds)
	BEGIN
		IF @WhereClause != '' SET @WhereClause = @WhereClause + ' AND ';
		SET @WhereClause = @WhereClause + ' ([ETA].[EntityStateId] IN (SELECT * FROM @EntityStateIds)) ';
	END

	IF @Search IS NOT NULL AND @Search != ''
	BEGIN
		IF @WhereClause != '' SET @WhereClause = @WhereClause + ' AND ';
		SET @WhereClause = @WhereClause  + ' ([P].[Code] LIKE @SearchRegEx) ';
	END

	IF EXISTS (SELECT * FROM @SexIds)
	BEGIN
		IF @WhereClause != '' SET @WhereClause = @WhereClause + ' AND ';
		SET @WhereClause = @WhereClause  + ' ([P].[SexId] IN (SELECT [Id] FROM @SexIds)) ';
	END

	IF @WhereClause != ''
		SET @WhereClause = ' WHERE ' + @WhereClause;

	--> Ordering
	SET @OrderClause = @OrderClause + ' [P].[Code] ASC, ';
	SET @OrderClause = @OrderClause + ' [P].[Id] DESC ';
END
