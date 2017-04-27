CREATE PROCEDURE [race].[spGetRaceDetails]
	@UserId INT,
	@ContextOrganizationId INT,
	@RaceId INT
AS
BEGIN
	DECLARE @RaceEntityTypeAccess [udt].[EntityAccess];
	INSERT INTO @RaceEntityTypeAccess
	EXEC [acc].[spGetRaceEntityTypeAccess] @UserId, @ContextOrganizationId;

	DECLARE @RaceEntityInfoId INT;
	DECLARE @AccessTypeId INT;
	DECLARE @OrganizationId INT;
	DECLARE @SeasonId INT;

	SELECT 
		@RaceEntityInfoId = [R].[EntityInfoId],
		@AccessTypeId = [ETA].[AccessTypeId],
		@OrganizationId = [ETA].[OrganizationId],
		@SeasonId = [R].[SeasonId]
	FROM [race].[Race] [R] WITH(NOLOCK)
		INNER JOIN @RaceEntityTypeAccess [ETA] ON [ETA].[EntityInfoId] = [R].[EntityInfoId]
	WHERE [R].[Id] = @RaceId;

	IF (@RaceEntityInfoId IS NOT NULL AND @RaceEntityInfoId > 0)
	BEGIN
		DECLARE @PigeonEntityTypeAccess [udt].[EntityAccess];
		INSERT INTO @PigeonEntityTypeAccess
		EXEC [acc].[spGetPigeonEntityTypeAccess] @UserId, @ContextOrganizationId;

		DECLARE @MemberEntityTypeAccess [udt].[EntityAccess];
		INSERT INTO @MemberEntityTypeAccess
		EXEC [acc].[spGetMemberEntityTypeAccess] @UserId, @ContextOrganizationId;

		DECLARE @UserOrganizations [udt].[KeyList];
		INSERT INTO @UserOrganizations
		SELECT [OrganizationId] FROM [acc].[fnGetOrganizations] (@UserId, @ContextOrganizationId);

		SELECT
			[R].[Id],
			[R].[EntityInfoId],
			@AccessTypeId AS [AccessTypeId],
			[EO].[EntityTypeId],
			[EO].[EntityStateId],
			[EO].[OrganizationId],
			[acc].[fnGetOrganizationName]([EO].[OrganizationId], @UserOrganizations) AS [OrganizationName],
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
			(SELECT
				COUNT(DISTINCT [M].[Id])
			FROM [acc].[Pigeon] [P] WITH(NOLOCK)
				INNER JOIN [race].[Statement] [S] WITH(NOLOCK) ON [S].[PigeonId] = [P].[Id] AND [S].[SeasonId] = @SeasonId
				INNER JOIN [race].[RacePigeon] [RP] WITH(NOLOCK) ON [RP].[PigeonId] = [P].[Id] AND [RP].[RaceId] = [R].[Id]
				INNER JOIN [plt].[EntityOrganization] [PEO] WITH(NOLOCK) ON
					[PEO].[EntityInfoId] = [P].[EntityInfoId] AND
					[PEO].[OrganizationId] = @OrganizationId
				INNER JOIN @PigeonEntityTypeAccess [PETA] ON
					[PETA].[EntityInfoId] = [PEO].[EntityInfoId] AND
					[PETA].[OrganizationId] = [PEO].[OrganizationId] AND
					[PETA].[EntityStateId] = [PEO].[EntityStateId] AND
					[PETA].[EntityTypeId] = [PEO].[EntityTypeId]
				INNER JOIN [acc].[Member] [M] WITH(NOLOCK) ON [M].[Id] = [P].[MemberId]
				INNER JOIN [plt].[EntityOrganization] [MEO] WITH(NOLOCK) ON
					[MEO].[EntityInfoId] = [M].[EntityInfoId] AND
					[MEO].[OrganizationId] = @OrganizationId
				INNER JOIN @MemberEntityTypeAccess [META] ON
					[META].[EntityInfoId] = [MEO].[EntityInfoId] AND
					[META].[OrganizationId] = [MEO].[OrganizationId] AND
					[META].[EntityStateId] = [MEO].[EntityStateId] AND
					[META].[EntityTypeId] = [MEO].[EntityTypeId]
			) AS [MemberCount],
			(SELECT
				COUNT(DISTINCT [P].[Id])
			FROM [acc].[Pigeon] [P] WITH(NOLOCK)
				INNER JOIN [race].[Statement] [S] WITH(NOLOCK) ON [S].[PigeonId] = [P].[Id] AND [S].[SeasonId] = @SeasonId
				INNER JOIN [race].[RacePigeon] [RP] WITH(NOLOCK) ON [RP].[PigeonId] = [P].[Id] AND [RP].[RaceId] = [R].[Id]
				INNER JOIN [plt].[EntityOrganization] [PEO] WITH(NOLOCK) ON
					[PEO].[EntityInfoId] = [P].[EntityInfoId] AND
					[PEO].[OrganizationId] = @OrganizationId
				INNER JOIN @PigeonEntityTypeAccess [PETA] ON
					[PETA].[EntityInfoId] = [PEO].[EntityInfoId] AND
					[PETA].[OrganizationId] = [PEO].[OrganizationId] AND
					[PETA].[EntityStateId] = [PEO].[EntityStateId] AND
					[PETA].[EntityTypeId] = [PEO].[EntityTypeId]
				INNER JOIN [acc].[Member] [M] WITH(NOLOCK) ON [M].[Id] = [P].[MemberId]
				INNER JOIN [plt].[EntityOrganization] [MEO] WITH(NOLOCK) ON
					[MEO].[EntityInfoId] = [M].[EntityInfoId] AND
					[MEO].[OrganizationId] = @OrganizationId
				INNER JOIN @MemberEntityTypeAccess [META] ON
					[META].[EntityInfoId] = [MEO].[EntityInfoId] AND
					[META].[OrganizationId] = [MEO].[OrganizationId] AND
					[META].[EntityStateId] = [MEO].[EntityStateId] AND
					[META].[EntityTypeId] = [MEO].[EntityTypeId]
			) AS [PigeonCount],
			(SELECT
				COUNT([P].[Id])
			FROM [race].[RacePigeon] [RP] WITH(NOLOCK)
				INNER JOIN [acc].[Pigeon] [P] WITH(NOLOCK) ON [P].[Id] = [RP].[PigeonId] AND [RP].[RaceId] = [R].[Id]
				INNER JOIN [race].[PigeonReturnTime] [PRT] WITH(NOLOCK) ON [PRT].[PigeonId] = [P].[Id] AND [PRT].[RaceId] = [RP].[RaceId]
				INNER JOIN [plt].[EntityOrganization] [PEO] WITH(NOLOCK) ON
					[PEO].[EntityInfoId] = [P].[EntityInfoId] AND
					[PEO].[OrganizationId] = @OrganizationId
				INNER JOIN @PigeonEntityTypeAccess [PETA] ON
					[PETA].[EntityInfoId] = [PEO].[EntityInfoId] AND
					[PETA].[OrganizationId] = [PEO].[OrganizationId] AND
					[PETA].[EntityStateId] = [PEO].[EntityStateId] AND
					[PETA].[EntityTypeId] = [PEO].[EntityTypeId]
				INNER JOIN [acc].[Member] [M] WITH(NOLOCK) ON [M].[Id] = [P].[MemberId]
				INNER JOIN [plt].[EntityOrganization] [MEO] WITH(NOLOCK) ON
					[MEO].[EntityInfoId] = [M].[EntityInfoId] AND
					[MEO].[OrganizationId] = @OrganizationId
				INNER JOIN @MemberEntityTypeAccess [META] ON
					[META].[EntityInfoId] = [MEO].[EntityInfoId] AND
					[META].[OrganizationId] = [MEO].[OrganizationId] AND
					[META].[EntityStateId] = [MEO].[EntityStateId] AND
					[META].[EntityTypeId] = [MEO].[EntityTypeId]
			) AS [ReturnPigeonCount],
			(SELECT CASE
				WHEN EXISTS (SELECT * FROM [race].[RaceResult] [RR] WITH(NOLOCK) WHERE [RR].[RaceId] = [R].[Id])
				THEN CAST(1 AS BIT)
				ELSE CAST(0 AS BIT)
				END
			) AS [HasResults],
			(SELECT CASE
				WHEN EXISTS (SELECT * FROM [race].[RaceDistance] [RD] WITH(NOLOCK) WHERE [RD].[RaceId] = [R].[Id])
				THEN CAST(1 AS BIT)
				ELSE CAST(0 AS BIT)
				END
			) AS [HasCalculatedDistances]
		FROM [race].[Race] [R] WITH(NOLOCK)
			INNER JOIN [plt].[EntityOrganization] [EO] WITH(NOLOCK) ON [EO].[EntityInfoId] = [R].[EntityInfoId]
			INNER JOIN [race].[Season] [S] WITH(NOLOCK) ON [S].[Id] = [R].[SeasonId]
			INNER JOIN [race].[Point] [P] WITH(NOLOCK) ON [P].[Id] = [R].[PointId]
		WHERE [R].[Id] = @RaceId

		SELECT
			[A].[Id],
			[A].[City],
			[A].[PostalCode],
			[A].[Street],
			[A].[Number],
			[A].[FormattedAddress],
			[A].[Latitude],
			[A].[Longitude]
		FROM [race].[Race] [R] WITH(NOLOCK)
			INNER JOIN [race].[Point] [P] WITH(NOLOCK) ON [P].[Id] = [R].[PointId]
			INNER JOIN [acc].[Address] [A] WITH(NOLOCK) ON [A].[Id] = [P].[AddressId]
		WHERE [R].[Id] = @RaceId;

		SELECT DISTINCT
			[M].[Id],
			[M].[LastName],
			[M].[FirstName],
			[M].[MiddleName]
		FROM [acc].[Member] [M] WITH(NOLOCK)
			INNER JOIN [plt].[EntityOrganization] [MEO] WITH(NOLOCK) ON
				[MEO].[EntityInfoId] = [M].[EntityInfoId] AND
				[MEO].[OrganizationId] = @OrganizationId
			INNER JOIN @MemberEntityTypeAccess [META] ON
				[META].[EntityInfoId] = [MEO].[EntityInfoId] AND
				[META].[OrganizationId] = [MEO].[OrganizationId] AND
				[META].[EntityStateId] = [MEO].[EntityStateId] AND
				[META].[EntityTypeId] = [MEO].[EntityTypeId]
			INNER JOIN [acc].[Pigeon] [P] WITH(NOLOCK) ON [P].[MemberId] = [M].[Id]
			INNER JOIN [race].[Statement] [S] WITH(NOLOCK) ON [S].[PigeonId] = [P].[Id] AND [S].[SeasonId] = @SeasonId
			INNER JOIN [plt].[EntityOrganization] [PEO] WITH(NOLOCK) ON
				[PEO].[EntityInfoId] = [P].[EntityInfoId] AND
				[PEO].[OrganizationId] = @OrganizationId
			INNER JOIN @PigeonEntityTypeAccess [PETA] ON
				[PETA].[EntityInfoId] = [PEO].[EntityInfoId] AND
				[PETA].[OrganizationId] = [PEO].[OrganizationId] AND
				[PETA].[EntityStateId] = [PEO].[EntityStateId] AND
				[PETA].[EntityTypeId] = [PEO].[EntityTypeId]

		SELECT DISTINCT
			[P].[Id],
			[P].[Year],
			[P].[Code],
			[P].[Number],
			[P].[MemberId]
		FROM [acc].[Pigeon] [P] WITH(NOLOCK)
			INNER JOIN [race].[Statement] [S] WITH(NOLOCK) ON [S].[PigeonId] = [P].[Id] AND [S].[SeasonId] = @SeasonId
			INNER JOIN [plt].[EntityOrganization] [PEO] WITH(NOLOCK) ON
				[PEO].[EntityInfoId] = [P].[EntityInfoId] AND
				[PEO].[OrganizationId] = @OrganizationId
			INNER JOIN @PigeonEntityTypeAccess [PETA] ON
				[PETA].[EntityInfoId] = [PEO].[EntityInfoId] AND
				[PETA].[OrganizationId] = [PEO].[OrganizationId] AND
				[PETA].[EntityStateId] = [PEO].[EntityStateId] AND
				[PETA].[EntityTypeId] = [PEO].[EntityTypeId]
			INNER JOIN [acc].[Member] [M] WITH(NOLOCK) ON [M].[Id] = [P].[MemberId]
			INNER JOIN [plt].[EntityOrganization] [MEO] WITH(NOLOCK) ON
				[MEO].[EntityInfoId] = [M].[EntityInfoId] AND
				[MEO].[OrganizationId] = @OrganizationId
			INNER JOIN @MemberEntityTypeAccess [META] ON
				[META].[EntityInfoId] = [MEO].[EntityInfoId] AND
				[META].[OrganizationId] = [MEO].[OrganizationId] AND
				[META].[EntityStateId] = [MEO].[EntityStateId] AND
				[META].[EntityTypeId] = [MEO].[EntityTypeId]

		SELECT
			[P].[Id]
		FROM [race].[RacePigeon] [RP] WITH(NOLOCK)
			INNER JOIN [acc].[Pigeon] [P] WITH(NOLOCK) ON [P].[Id] = [RP].[PigeonId]
			INNER JOIN [plt].[EntityOrganization] [PEO] WITH(NOLOCK) ON
				[PEO].[EntityInfoId] = [P].[EntityInfoId] AND
				[PEO].[OrganizationId] = @OrganizationId
			INNER JOIN @PigeonEntityTypeAccess [PETA] ON
				[PETA].[EntityInfoId] = [PEO].[EntityInfoId] AND
				[PETA].[OrganizationId] = [PEO].[OrganizationId] AND
				[PETA].[EntityStateId] = [PEO].[EntityStateId] AND
				[PETA].[EntityTypeId] = [PEO].[EntityTypeId]
			INNER JOIN [acc].[Member] [M] WITH(NOLOCK) ON [M].[Id] = [P].[MemberId]
			INNER JOIN [plt].[EntityOrganization] [MEO] WITH(NOLOCK) ON
				[MEO].[EntityInfoId] = [M].[EntityInfoId] AND
				[MEO].[OrganizationId] = @OrganizationId
			INNER JOIN @MemberEntityTypeAccess [META] ON
				[META].[EntityInfoId] = [MEO].[EntityInfoId] AND
				[META].[OrganizationId] = [MEO].[OrganizationId] AND
				[META].[EntityStateId] = [MEO].[EntityStateId] AND
				[META].[EntityTypeId] = [MEO].[EntityTypeId]
		WHERE [RP].[RaceId] = @RaceId;
	END;
END;
