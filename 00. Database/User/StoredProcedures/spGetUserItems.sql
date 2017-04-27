CREATE PROCEDURE [acc].[spGetUserItems]
	@UserId INT,
	@ContextOrganizationId INT,
	@EntityStateIds [udt].[KeyList] READONLY,
	@Search NVARCHAR(MAX),
	@Skip INT,
	@Take INT
AS
BEGIN
	DECLARE @UserEntityTypeAccess [udt].[EntityAccess];
	INSERT INTO @UserEntityTypeAccess
	EXEC [acc].[spGetUserEntityTypeAccess] @UserId, @ContextOrganizationId, @EntityStateIds;

	DECLARE @SearchRegEx NVARCHAR(MAX) = '';
	DECLARE @JoinClause NVARCHAR(MAX) = '' ;
	DECLARE @WhereClause NVARCHAR(MAX) = '';
	DECLARE @OrderClause NVARCHAR(MAX) = '';

	EXEC [acc].[spInitUserFilter]
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
			[U].[Id],
			ROW_NUMBER() OVER (ORDER BY ' + @OrderClause + N') AS [RowNumber]
		FROM [acc].[User] [U] WITH(NOLOCK)
			INNER JOIN @UserEntityTypeAccess [ETA] ON [ETA].[EntityInfoId] = [U].[EntityInfoId]'
			+ @JoinClause
		+ @WhereClause + N';

		SELECT
			[U].[Id],
			[U].[EntityInfoId],
			[EO].[OrganizationId],
			[EO].[EntityStateId],
			[U].[FirstName],
			[U].[MiddleName],
			[U].[LastName],
			[U].[Phone],
			[U].[Mobile],
			[U].[Email],
			[A].[FormattedAddress]
		FROM [acc].[User] [U] WITH(NOLOCK)
			INNER JOIN @Filter [F] ON [F].[Id] = [U].[Id]
			INNER JOIN [plt].[EntityOrganization] [EO] WITH(NOLOCK) ON [EO].[EntityInfoId] = [U].[EntityInfoId]
			LEFT JOIN [acc].[Address] [A] WITH(NOLOCK) ON [A].[Id] = [U].[AddressId]
		WHERE [F].[RowNumber] > @Skip AND [F].[RowNumber] <= @Skip + @Take
		ORDER BY [F].[RowNumber];

		SELECT
			COUNT(DISTINCT [F].[Id]) AS [Count]
		FROM @Filter [F];
	';

	DECLARE @Params NVARCHAR(MAX) = N'
		@UserId INT,
		@ContextOrganizationId INT,
		@UserEntityTypeAccess [udt].[EntityAccess] READONLY,
		@EntityStateIds [udt].[KeyList] READONLY,
		@SearchRegEx NVARCHAR(MAX),
		@Skip INT,
		@Take INT';

	EXECUTE sp_executesql @Sql, @Params,
		@UserId,
		@ContextOrganizationId,
		@UserEntityTypeAccess,
		@EntityStateIds,
		@SearchRegEx,
		@Skip,
		@Take;
END;