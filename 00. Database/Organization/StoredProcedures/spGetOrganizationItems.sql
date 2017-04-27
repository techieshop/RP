CREATE PROCEDURE [acc].[spGetOrganizationItems]
	@UserId INT,
	@ContextOrganizationId INT,
	@EntityStateIds [udt].[KeyList] READONLY,
	@Search NVARCHAR(MAX),
	@Skip INT,
	@Take INT
AS
BEGIN
	DECLARE @OrganizationEntityTypeAccess [udt].[EntityAccess];
	INSERT INTO @OrganizationEntityTypeAccess
	EXEC [acc].[spGetOrganizationEntityTypeAccess] @UserId, @ContextOrganizationId, @EntityStateIds;

	DECLARE @SearchRegEx NVARCHAR(MAX) = '';
	DECLARE @JoinClause NVARCHAR(MAX) = '' ;
	DECLARE @WhereClause NVARCHAR(MAX) = '';
	DECLARE @OrderClause NVARCHAR(MAX) = '';

	EXEC [acc].[spInitOrganizationFilter]
		@SearchRegEx OUTPUT,
		@JoinClause OUTPUT,
		@WhereClause OUTPUT,
		@OrderClause OUTPUT,
		@EntityStateIds,
		@Search;

	DECLARE @Sql NVARCHAR(MAX) = N'
		DECLARE @MemberEntityTypeAccess [udt].[EntityAccess];
		INSERT INTO @MemberEntityTypeAccess
		EXEC [acc].[spGetMemberEntityTypeAccess] @UserId, @ContextOrganizationId;

		DECLARE @PigeonEntityTypeAccess [udt].[EntityAccess];
		INSERT INTO @PigeonEntityTypeAccess
		EXEC [acc].[spGetPigeonEntityTypeAccess] @UserId, @ContextOrganizationId;

		DECLARE @UserOrganizations [udt].[KeyList];
		INSERT INTO @UserOrganizations
		SELECT [OrganizationId] FROM [acc].[fnGetOrganizations] (@UserId, @ContextOrganizationId);

		DECLARE @Filter AS TABLE (
			[Id] INT NOT NULL,
			[RowNumber] INT NOT NULL
		);

		INSERT INTO @Filter
		SELECT
			[O].[Id],
			ROW_NUMBER() OVER (ORDER BY ' + @OrderClause + N') AS [RowNumber]
		FROM [acc].[Organization] [O] WITH(NOLOCK)
			INNER JOIN @OrganizationEntityTypeAccess [ETA] ON [ETA].[EntityInfoId] = [O].[EntityInfoId]'
			+ @JoinClause
		+ @WhereClause + N';

		SELECT
			[O].[Id],
			[O].[EntityInfoId],
			[acc].[fnGetOrganizationName]([O].[Id], @UserOrganizations) AS [Name],
			[EO].[EntityStateId],
			[EO].[OrganizationId],
			(SELECT
				COUNT(DISTINCT [M].[Id])
			FROM [acc].[Member] [M] WITH(NOLOCK)
				INNER JOIN [plt].[EntityOrganization] [EO] WITH(NOLOCK) ON [EO].[EntityInfoId] = [M].[EntityInfoId]
				INNER JOIN @MemberEntityTypeAccess [META] ON
					[META].[EntityInfoId] = [EO].[EntityInfoId] AND
					[META].[OrganizationId] = [EO].[OrganizationId] AND
					[META].[EntityStateId] = [EO].[EntityStateId] AND
					[META].[EntityTypeId] = [EO].[EntityTypeId]
			WHERE [EO].[OrganizationId] = [O].[Id]
			) AS [MemberCount]
		FROM [acc].[Organization] [O] WITH(NOLOCK)
			INNER JOIN @Filter [F] ON [F].[Id] = [O].[Id]
			INNER JOIN [plt].[EntityOrganization] [EO] WITH(NOLOCK) ON [EO].[EntityInfoId] = [O].[EntityInfoId]
		WHERE [F].[RowNumber] > @Skip AND [F].[RowNumber] <= @Skip + @Take
		ORDER BY [F].[RowNumber];

		SELECT
			COUNT(DISTINCT [F].[Id]) AS [Count]
		FROM @Filter [F];

		SELECT
			[OMT].[OrganizationId],
			[OMT].[MemberId],
			[OMT].[MemberTypeId],
			[M].[FirstName],
			[M].[LastName],
			[M].[MiddleName]
		FROM [acc].[Member] [M] WITH(NOLOCK)
			INNER JOIN [plt].[EntityOrganization] [EO] WITH(NOLOCK) ON [EO].[EntityInfoId] = [M].[EntityInfoId]
			INNER JOIN @Filter [F] ON [F].[Id] = [EO].[OrganizationId]
			INNER JOIN @MemberEntityTypeAccess [META] ON
				[META].[EntityInfoId] = [EO].[EntityInfoId] AND
				[META].[OrganizationId] = [EO].[OrganizationId] AND
				[META].[EntityStateId] = [EO].[EntityStateId] AND
				[META].[EntityTypeId] = [EO].[EntityTypeId]
			INNER JOIN [acc].[OrganizationMemberType] [OMT] WITH(NOLOCK) ON [OMT].[MemberId] = [M].[Id] AND [OMT].[OrganizationId] = [EO].[OrganizationId]
		WHERE [F].[RowNumber] > @Skip AND [F].[RowNumber] <= @Skip + @Take;

		SELECT
			COUNT(DISTINCT [M].[Id]) AS [MemberCount]
		FROM [acc].[Member] [M] WITH(NOLOCK)
			INNER JOIN [plt].[EntityOrganization] [EO] WITH(NOLOCK) ON [EO].[EntityInfoId] = [M].[EntityInfoId]
			INNER JOIN @MemberEntityTypeAccess [META] ON
				[META].[EntityInfoId] = [EO].[EntityInfoId] AND
				[META].[OrganizationId] = [EO].[OrganizationId] AND
				[META].[EntityStateId] = [EO].[EntityStateId] AND
				[META].[EntityTypeId] = [EO].[EntityTypeId];
		
		SELECT
			COUNT(DISTINCT [P].[Id]) AS [PigeonCount]
		FROM [acc].[Pigeon] [P] WITH(NOLOCK)
			INNER JOIN [plt].[EntityOrganization] [EO] WITH(NOLOCK) ON [EO].[EntityInfoId] = [P].[EntityInfoId]
			INNER JOIN @PigeonEntityTypeAccess [PETA] ON
				[PETA].[EntityInfoId] = [EO].[EntityInfoId] AND
				[PETA].[OrganizationId] = [EO].[OrganizationId] AND
				[PETA].[EntityStateId] = [EO].[EntityStateId] AND
				[PETA].[EntityTypeId] = [EO].[EntityTypeId];
	';

	DECLARE @Params NVARCHAR(MAX) = N'
		@UserId INT,
		@ContextOrganizationId INT,
		@OrganizationEntityTypeAccess [udt].[EntityAccess] READONLY,
		@EntityStateIds [udt].[KeyList] READONLY,
		@SearchRegEx NVARCHAR(MAX),
		@Skip INT,
		@Take INT';

	EXECUTE sp_executesql @Sql, @Params,
		@UserId,
		@ContextOrganizationId,
		@OrganizationEntityTypeAccess,
		@EntityStateIds,
		@SearchRegEx,
		@Skip,
		@Take;
END