CREATE PROCEDURE [race].[spGetPointItems]
	@UserId INT,
	@ContextOrganizationId INT,
	@EntityStateIds [udt].[KeyList] READONLY,
	@Search NVARCHAR(MAX),
	@Skip INT,
	@Take INT
AS
BEGIN
	DECLARE @PointEntityTypeAccess [udt].[EntityAccess];
	INSERT INTO @PointEntityTypeAccess
	EXEC [acc].[spGetPointEntityTypeAccess] @UserId, @ContextOrganizationId, @EntityStateIds;

	DECLARE @SearchRegEx NVARCHAR(MAX) = '';
	DECLARE @JoinClause NVARCHAR(MAX) = '' ;
	DECLARE @WhereClause NVARCHAR(MAX) = '';
	DECLARE @OrderClause NVARCHAR(MAX) = '';

	EXEC [race].[spInitPointFilter]
		@SearchRegEx OUTPUT,
		@JoinClause OUTPUT,
		@WhereClause OUTPUT,
		@OrderClause OUTPUT,
		@EntityStateIds,
		@Search;

	DECLARE @Sql NVARCHAR(MAX) = N'
		DECLARE @UserOrganizations [udt].[KeyList];
		INSERT INTO @UserOrganizations
		SELECT [OrganizationId] FROM [acc].[fnGetOrganizations] (@UserId, @ContextOrganizationId);

		DECLARE @Filter AS TABLE (
			[Id] INT NOT NULL,
			[OrganizationId] INT NOT NULL,
			[RowNumber] INT NOT NULL
		);

		INSERT INTO @Filter
		SELECT
			[P].[Id],
			[ETA].[OrganizationId],
			ROW_NUMBER() OVER (ORDER BY ' + @OrderClause + N') AS [RowNumber]
		FROM [race].[Point] [P] WITH(NOLOCK)
			INNER JOIN @PointEntityTypeAccess [ETA] ON [ETA].[EntityInfoId] = [P].[EntityInfoId]'
			+ @JoinClause
		+ @WhereClause + N';

		SELECT
			[P].[Id],
			[P].[EntityInfoId],
			[acc].[fnGetOrganizationName]([EO].[OrganizationId], @UserOrganizations) AS [OrganizationName],
			[EO].[OrganizationId],
			[EO].[EntityStateId],
			[P].[Name],
			[A].[FormattedAddress]
		FROM [race].[Point] [P] WITH(NOLOCK)
			INNER JOIN @Filter [F] ON [F].[Id] = [P].[Id]
			INNER JOIN [plt].[EntityOrganization] [EO] WITH(NOLOCK) ON [EO].[EntityInfoId] = [P].[EntityInfoId] AND [EO].[OrganizationId] = [F].[OrganizationId]
			LEFT JOIN [acc].[Address] [A] WITH(NOLOCK) ON [A].[Id] = [P].[AddressId]
		WHERE [F].[RowNumber] > @Skip AND [F].[RowNumber] <= @Skip + @Take
		ORDER BY [F].[RowNumber];

		SELECT
			COUNT(DISTINCT [F].[Id]) AS [Count]
		FROM @Filter [F];
	';

	DECLARE @Params NVARCHAR(MAX) = N'
		@UserId INT,
		@ContextOrganizationId INT,
		@PointEntityTypeAccess [udt].[EntityAccess] READONLY,
		@EntityStateIds [udt].[KeyList] READONLY,
		@SearchRegEx NVARCHAR(MAX),
		@Skip INT,
		@Take INT';

	EXECUTE sp_executesql @Sql, @Params,
		@UserId,
		@ContextOrganizationId,
		@PointEntityTypeAccess,
		@EntityStateIds,
		@SearchRegEx,
		@Skip,
		@Take;
END;