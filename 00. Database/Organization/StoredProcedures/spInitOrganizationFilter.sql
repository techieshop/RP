CREATE PROCEDURE [acc].[spInitOrganizationFilter]
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
		SET @WhereClause = @WhereClause  + ' ([O].[Name] LIKE @SearchRegEx) ';
	END

	IF @WhereClause != ''
		SET @WhereClause = ' WHERE ' + @WhereClause;

	--> Ordering
	SET @OrderClause = @OrderClause + ' [acc].[fnGetOrganizationName]([O].[Id], @UserOrganizations) ASC, ';
	SET @OrderClause = @OrderClause + ' [O].[Id] DESC ';
END
