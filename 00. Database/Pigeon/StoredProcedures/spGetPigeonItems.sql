CREATE PROCEDURE [acc].[spGetPigeonItems]
	@UserId INT,
	@ContextOrganizationId INT,
	@EntityStateIds [udt].[KeyList] READONLY,
	@Search NVARCHAR(MAX),
	@SexIds [udt].[KeyList] READONLY,
	@Skip INT,
	@Take INT
AS
BEGIN
	DECLARE @PigeonEntityTypeAccess [udt].[EntityAccess];
	INSERT INTO @PigeonEntityTypeAccess
	EXEC [acc].[spGetPigeonEntityTypeAccess] @UserId, @ContextOrganizationId, @EntityStateIds;

	DECLARE @SearchRegEx NVARCHAR(MAX) = '';
	DECLARE @JoinClause NVARCHAR(MAX) = '' ;
	DECLARE @WhereClause NVARCHAR(MAX) = '';
	DECLARE @OrderClause NVARCHAR(MAX) = '';

	EXEC [acc].[spInitPigeonFilter]
		@SearchRegEx OUTPUT,
		@JoinClause OUTPUT,
		@WhereClause OUTPUT,
		@OrderClause OUTPUT,
		@EntityStateIds,
		@Search,
		@SexIds;

	DECLARE @Sql NVARCHAR(MAX) = N'
		DECLARE @OrganizationEntityTypeAccess [udt].[EntityAccess];
		INSERT INTO @OrganizationEntityTypeAccess
		EXEC [acc].[spGetOrganizationEntityTypeAccess] @UserId, @ContextOrganizationId;

		DECLARE @MemberEntityTypeAccess [udt].[EntityAccess];
		INSERT INTO @MemberEntityTypeAccess
		EXEC [acc].[spGetMemberEntityTypeAccess] @UserId, @ContextOrganizationId;

		DECLARE @UserOrganizations [udt].[KeyList];
		INSERT INTO @UserOrganizations
		SELECT [OrganizationId] FROM [acc].[fnGetOrganizations] (@UserId, @ContextOrganizationId);

		DECLARE @Filter AS TABLE (
			[Id] INT NOT NULL,
			[OrganizationId] INT NOT NULL,
			[RowNumber] INT NOT NULL
		);

		DECLARE @AC BIT = 1;

		INSERT INTO @Filter
		SELECT
			[P].[Id],
			[ETA].[OrganizationId],
			ROW_NUMBER() OVER (ORDER BY ' + @OrderClause + N') AS [RowNumber]
		FROM [acc].[Pigeon] [P] WITH(NOLOCK)
			INNER JOIN @PigeonEntityTypeAccess [ETA] ON [ETA].[EntityInfoId] = [P].[EntityInfoId]'
			+ @JoinClause
		+ @WhereClause + N';

		SELECT
			[P].[Id],
			[P].[EntityInfoId],
			[EO].[OrganizationId],
			[EO].[EntityStateId],
			[P].[Year],
			[P].[Code],
			[P].[Number],
			[M].[LastName] AS [OwnerLastName],
			[M].[FirstName] AS [OwnerFirstName],
			[M].[MiddleName] AS [OwnerMiddleName],
			[P].[SexId],
			[acc].[fnGetOrganizationName]([EO].[OrganizationId], @UserOrganizations) AS [OrganizationName],
			(SELECT
				COUNT([RR].[Id])
			FROM [race].[RaceResult] [RR] WITH(NOLOCK)
			WHERE[RR].[PigeonId] = [P].[Id] AND [RR].[Ac] = @AC
			) AS [PrizeCount]
		FROM [acc].[Pigeon] [P] WITH(NOLOCK)
			INNER JOIN @Filter [F] ON [F].[Id] = [P].[Id]
			INNER JOIN [plt].[EntityOrganization] [EO] WITH(NOLOCK) ON [EO].[EntityInfoId] = [P].[EntityInfoId]
			INNER JOIN [acc].[Member] [M] WITH(NOLOCK) ON [M].[Id] = [P].[MemberId]
		WHERE [F].[RowNumber] > @Skip AND [F].[RowNumber] <= @Skip + @Take
		ORDER BY [F].[RowNumber];

		SELECT
			COUNT(DISTINCT [F].[Id]) AS [Count]
		FROM @Filter [F];

		SELECT
			COUNT(DISTINCT [O].[Id]) AS [OrganizationCount]
		FROM [acc].[Organization] [O] WITH(NOLOCK)
			INNER JOIN [plt].[EntityOrganization] [EO] WITH(NOLOCK) ON [EO].[EntityInfoId] = [O].[EntityInfoId]
			INNER JOIN @OrganizationEntityTypeAccess [OETA] ON
				[OETA].[EntityInfoId] = [EO].[EntityInfoId] AND
				[OETA].[OrganizationId] = [EO].[OrganizationId] AND
				[OETA].[EntityStateId] = [EO].[EntityStateId] AND
				[OETA].[EntityTypeId] = [EO].[EntityTypeId];

		SELECT
			COUNT(DISTINCT [M].[Id]) AS [MemberCount]
		FROM [acc].[Member] [M] WITH(NOLOCK)
			INNER JOIN [plt].[EntityOrganization] [EO] WITH(NOLOCK) ON [EO].[EntityInfoId] = [M].[EntityInfoId]
			INNER JOIN @MemberEntityTypeAccess [META] ON
				[META].[EntityInfoId] = [EO].[EntityInfoId] AND
				[META].[OrganizationId] = [EO].[OrganizationId] AND
				[META].[EntityStateId] = [EO].[EntityStateId] AND
				[META].[EntityTypeId] = [EO].[EntityTypeId];
	';

		DECLARE @Params NVARCHAR(MAX) = N'
		@UserId INT,
		@ContextOrganizationId INT,
		@PigeonEntityTypeAccess [udt].[EntityAccess] READONLY,
		@EntityStateIds [udt].[KeyList] READONLY,
		@SearchRegEx NVARCHAR(MAX),
		@SexIds [udt].[KeyList] READONLY,
		@Skip INT,
		@Take INT';

	EXECUTE sp_executesql @Sql, @Params,
		@UserId,
		@ContextOrganizationId,
		@PigeonEntityTypeAccess,
		@EntityStateIds,
		@SearchRegEx,
		@SexIds,
		@Skip,
		@Take;
END
