CREATE PROCEDURE [acc].[spInitRoleFilter]
	@SearchRegEx NVARCHAR(MAX) = '' OUTPUT,
	@JoinClause NVARCHAR(MAX) = '' OUTPUT,
	@WhereClause NVARCHAR(MAX) = '' OUTPUT,
	@OrderClause NVARCHAR(MAX) = '' OUTPUT,
	@EntityStateIds [udt].[KeyList] READONLY,
	@Search NVARCHAR(MAX)
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
		SET @WhereClause = @WhereClause  + ' ([R].[Name] LIKE @SearchRegEx OR [R].[Description] LIKE @SearchRegEx) ';
	END

	IF @WhereClause != ''
		SET @WhereClause = ' WHERE ' + @WhereClause;

	--> Ordering
	SET @OrderClause = ' [R].[Name] ASC, ';
	SET @OrderClause = @OrderClause + ' [R].[Id] DESC ';
END
