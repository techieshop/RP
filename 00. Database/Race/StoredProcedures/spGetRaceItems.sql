CREATE PROCEDURE [race].[spGetRaceItems]
	@UserId INT,
	@ContextOrganizationId INT,
	@EntityStateIds [udt].[KeyList] READONLY,
	@Search NVARCHAR(MAX),
	@Skip INT,
	@Take INT
AS
BEGIN
	DECLARE @RaceEntityTypeAccess [udt].[EntityAccess];
	INSERT INTO @RaceEntityTypeAccess
	EXEC [acc].[spGetRaceEntityTypeAccess] @UserId, @ContextOrganizationId, @EntityStateIds;

	DECLARE @SearchRegEx NVARCHAR(MAX) = '';
	DECLARE @JoinClause NVARCHAR(MAX) = '' ;
	DECLARE @WhereClause NVARCHAR(MAX) = '';
	DECLARE @OrderClause NVARCHAR(MAX) = '';

	EXEC [race].[spInitRaceFilter]
		@SearchRegEx OUTPUT,
		@JoinClause OUTPUT,
		@WhereClause OUTPUT,
		@OrderClause OUTPUT,
		@EntityStateIds,
		@Search;

	DECLARE @Sql NVARCHAR(MAX) = N'
		DECLARE @PigeonEntityTypeAccess [udt].[EntityAccess];
		INSERT INTO @PigeonEntityTypeAccess
		EXEC [acc].[spGetPigeonEntityTypeAccess] @UserId, @ContextOrganizationId;

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

		INSERT INTO @Filter
		SELECT
			[R].[Id],
			[ETA].[OrganizationId],
			ROW_NUMBER() OVER (ORDER BY ' + @OrderClause + N') AS [RowNumber]
		FROM [race].[Race] [R] WITH(NOLOCK)
			INNER JOIN @RaceEntityTypeAccess [ETA] ON [ETA].[EntityInfoId] = [R].[EntityInfoId]
			INNER JOIN [race].[Season] [S] WITH(NOLOCK) ON [S].[Id] = [R].[SeasonId]'
			+ @JoinClause
		+ @WhereClause + N';

		SELECT
			[R].[Id],
			[R].[EntityInfoId],
			[acc].[fnGetOrganizationName]([EO].[OrganizationId], @UserOrganizations) AS [OrganizationName],
			[EO].[OrganizationId],
			[EO].[EntityStateId],
			[R].[Name],
			[R].[DarknessBeginTime],
			[R].[DarknessEndTime],
			[R].[EndСompetitionTime],
			[R].[StartRaceTime],
			[R].[StartСompetitionTime],
			[R].[Weather],
			[R].[PointId],
			[R].[RaceTypeId],
			[R].[SeasonId],
			[S].[Year],
			[S].[SeasonTypeId],
			[P].[Name] AS [PointName],
			[A].[FormattedAddress],
			(SELECT
				COUNT(DISTINCT [M].[Id])
			FROM [acc].[Pigeon] [P] WITH(NOLOCK)
				INNER JOIN [race].[Statement] [St] WITH(NOLOCK) ON [St].[PigeonId] = [P].[Id] AND [St].[SeasonId] = [S].[Id]
				INNER JOIN [race].[RacePigeon] [RP] WITH(NOLOCK) ON [RP].[PigeonId] = [P].[Id] AND [RP].[RaceId] = [R].[Id]
				INNER JOIN [plt].[EntityOrganization] [PEO] WITH(NOLOCK) ON
					[PEO].[EntityInfoId] = [P].[EntityInfoId] AND
					[PEO].[OrganizationId] = [F].[OrganizationId]
				INNER JOIN @PigeonEntityTypeAccess [PETA] ON
					[PETA].[EntityInfoId] = [PEO].[EntityInfoId] AND
					[PETA].[OrganizationId] = [PEO].[OrganizationId] AND
					[PETA].[EntityStateId] = [PEO].[EntityStateId] AND
					[PETA].[EntityTypeId] = [PEO].[EntityTypeId]
				INNER JOIN [acc].[Member] [M] WITH(NOLOCK) ON [M].[Id] = [P].[MemberId]
				INNER JOIN [plt].[EntityOrganization] [MEO] WITH(NOLOCK) ON
					[MEO].[EntityInfoId] = [M].[EntityInfoId] AND
					[MEO].[OrganizationId] = [F].[OrganizationId]
				INNER JOIN @MemberEntityTypeAccess [META] ON
					[META].[EntityInfoId] = [MEO].[EntityInfoId] AND
					[META].[OrganizationId] = [MEO].[OrganizationId] AND
					[META].[EntityStateId] = [MEO].[EntityStateId] AND
					[META].[EntityTypeId] = [MEO].[EntityTypeId]
			) AS [MemberCount],
			(SELECT
				COUNT(DISTINCT [P].[Id])
			FROM [acc].[Pigeon] [P] WITH(NOLOCK)
				INNER JOIN [race].[Statement] [St] WITH(NOLOCK) ON [St].[PigeonId] = [P].[Id] AND [St].[SeasonId] = [S].[Id]
				INNER JOIN [race].[RacePigeon] [RP] WITH(NOLOCK) ON [RP].[PigeonId] = [P].[Id] AND [RP].[RaceId] = [R].[Id]
				INNER JOIN [plt].[EntityOrganization] [PEO] WITH(NOLOCK) ON
					[PEO].[EntityInfoId] = [P].[EntityInfoId] AND
					[PEO].[OrganizationId] = [F].[OrganizationId]
				INNER JOIN @PigeonEntityTypeAccess [PETA] ON
					[PETA].[EntityInfoId] = [PEO].[EntityInfoId] AND
					[PETA].[OrganizationId] = [PEO].[OrganizationId] AND
					[PETA].[EntityStateId] = [PEO].[EntityStateId] AND
					[PETA].[EntityTypeId] = [PEO].[EntityTypeId]
				INNER JOIN [acc].[Member] [M] WITH(NOLOCK) ON [M].[Id] = [P].[MemberId]
				INNER JOIN [plt].[EntityOrganization] [MEO] WITH(NOLOCK) ON
					[MEO].[EntityInfoId] = [M].[EntityInfoId] AND
					[MEO].[OrganizationId] = [F].[OrganizationId]
				INNER JOIN @MemberEntityTypeAccess [META] ON
					[META].[EntityInfoId] = [MEO].[EntityInfoId] AND
					[META].[OrganizationId] = [MEO].[OrganizationId] AND
					[META].[EntityStateId] = [MEO].[EntityStateId] AND
					[META].[EntityTypeId] = [MEO].[EntityTypeId]
			) AS [PigeonCount]
		FROM [race].[Race] [R] WITH(NOLOCK)
			INNER JOIN @Filter [F] ON [F].[Id] = [R].[Id]
			INNER JOIN [plt].[EntityOrganization] [EO] WITH(NOLOCK) ON [EO].[EntityInfoId] = [R].[EntityInfoId] AND [EO].[OrganizationId] = [F].[OrganizationId]
			INNER JOIN [race].[Season] [S] WITH(NOLOCK) ON [S].[Id] = [R].[SeasonId]
			INNER JOIN [race].[Point] [P] WITH(NOLOCK) ON [P].[Id] = [R].[PointId]
			INNER JOIN [acc].[Address] [A] WITH(NOLOCK) ON [A].[Id] = [P].[AddressId]
		WHERE [F].[RowNumber] > @Skip AND [F].[RowNumber] <= @Skip + @Take
		ORDER BY [F].[RowNumber];

		SELECT
			COUNT(DISTINCT [F].[Id]) AS [Count]
		FROM @Filter [F];
	';

	DECLARE @Params NVARCHAR(MAX) = N'
		@UserId INT,
		@ContextOrganizationId INT,
		@RaceEntityTypeAccess [udt].[EntityAccess] READONLY,
		@EntityStateIds [udt].[KeyList] READONLY,
		@SearchRegEx NVARCHAR(MAX),
		@Skip INT,
		@Take INT';

	EXECUTE sp_executesql @Sql, @Params,
		@UserId,
		@ContextOrganizationId,
		@RaceEntityTypeAccess,
		@EntityStateIds,
		@SearchRegEx,
		@Skip,
		@Take;
END;