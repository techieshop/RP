CREATE PROCEDURE [acc].[spGetRoleItems]
	@UserId INT,
	@ContextOrganizationId INT,
	@EntityStateIds [udt].[KeyList] READONLY,
	@Search NVARCHAR(MAX),
	@Skip INT,
	@Take INT
AS
BEGIN
	DECLARE @RoleEntityTypeAccess [udt].[EntityAccess];
	INSERT INTO @RoleEntityTypeAccess
	EXEC [acc].[spGetRoleEntityTypeAccess] @UserId, @ContextOrganizationId, @EntityStateIds;

	DECLARE @SearchRegEx NVARCHAR(MAX) = '';
	DECLARE @JoinClause NVARCHAR(MAX) = '' ;
	DECLARE @WhereClause NVARCHAR(MAX) = '';
	DECLARE @OrderClause NVARCHAR(MAX) = '';

	EXEC [acc].[spInitRoleFilter]
		@SearchRegEx OUTPUT,
		@JoinClause OUTPUT,
		@WhereClause OUTPUT,
		@OrderClause OUTPUT,
		@EntityStateIds,
		@Search;

	DECLARE @Sql NVARCHAR(MAX) = N'
		DECLARE @Filter AS TABLE (
			[Id] INT NOT NULL,
			[RowNumber] INT NOT NULL
		);

		INSERT INTO @Filter
		SELECT
			[R].[Id],
			ROW_NUMBER() OVER (ORDER BY ' + @OrderClause + N') AS [RowNumber]
		FROM [acc].[Role] [R] WITH(NOLOCK)
			INNER JOIN @RoleEntityTypeAccess [ETA] ON [ETA].[EntityInfoId] = [R].[EntityInfoId]'
			+ @JoinClause
		+ @WhereClause + N';

		SELECT
			[R].[Id],
			[R].[EntityInfoId],
			[EO].[OrganizationId],
			[EO].[EntityStateId],
			[R].[Name],
			[R].[Description]
		FROM [acc].[Role] [R] WITH(NOLOCK)
			INNER JOIN @Filter [F] ON [F].[Id] = [R].[Id]
			INNER JOIN [plt].[EntityOrganization] [EO] WITH(NOLOCK) ON [EO].[EntityInfoId] = [R].[EntityInfoId]
		WHERE [F].[RowNumber] > @Skip AND [F].[RowNumber] <= @Skip + @Take
		ORDER BY [F].[RowNumber];

		SELECT
			COUNT(DISTINCT [F].[Id]) AS [Count]
		FROM @Filter [F];
	';

	DECLARE @Params NVARCHAR(MAX) = N'
		@UserId INT,
		@ContextOrganizationId INT,
		@RoleEntityTypeAccess [udt].[EntityAccess] READONLY,
		@EntityStateIds [udt].[KeyList] READONLY,
		@SearchRegEx NVARCHAR(MAX),
		@Skip INT,
		@Take INT';

	EXECUTE sp_executesql @Sql, @Params,
		@UserId,
		@ContextOrganizationId,
		@RoleEntityTypeAccess,
		@EntityStateIds,
		@SearchRegEx,
		@Skip,
		@Take;
END;