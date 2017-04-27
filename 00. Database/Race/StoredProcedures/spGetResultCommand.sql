CREATE PROCEDURE [race].[spGetResultCommand]
	@UserId INT,
	@ContextOrganizationId INT,
	@RaceId INT
AS
BEGIN
	DECLARE @RaceEntityTypeAccess [udt].[EntityAccess];
	INSERT INTO @RaceEntityTypeAccess
	EXEC [acc].[spGetRaceEntityTypeAccess] @UserId, @ContextOrganizationId;

	DECLARE @RaceEntityInfoId INT;
	DECLARE @OrganizationId INT;
	DECLARE @SeasonId INT;
	DECLARE @StartRaceTime DATETIME;

	SELECT
		@RaceEntityInfoId = [R].[EntityInfoId],
		@OrganizationId = [ETA].[OrganizationId],
		@SeasonId = [R].[SeasonId],
		@StartRaceTime = [R].[StartRaceTime]
	FROM [race].[Race] [R] WITH(NOLOCK)
		INNER JOIN @RaceEntityTypeAccess [ETA] ON [ETA].[EntityInfoId] = [R].[EntityInfoId]
	WHERE [R].[Id] = @RaceId;

	IF (@RaceEntityInfoId IS NOT NULL AND @RaceEntityInfoId > 0)
	BEGIN
		DECLARE @UserOrganizations [udt].[KeyList];
		INSERT INTO @UserOrganizations
		SELECT [OrganizationId] FROM [acc].[fnGetOrganizations] (@UserId, @OrganizationId);

		SELECT
			[R].[Id],
			[EO].[OrganizationId],
			[acc].[fnGetOrganizationName]([EO].[OrganizationId], @UserOrganizations) AS [OrganizationName],
			[R].[Name],
			[R].[DarknessBeginTime],
			[R].[DarknessEndTime],
			[R].[EndСompetitionTime],
			[R].[StartRaceTime],
			[R].[StartСompetitionTime],
			[R].[Weather],
			[R].[RaceTypeId],
			[S].[Year],
			[S].[SeasonTypeId],
			[P].[Name] AS [PointName],
			[A].[Latitude],
			[A].[Longitude],
			[R].[AdultsCount],
			[R].[AverageDistance],
			[R].[CockCount],
			[R].[DurationOfCompetition],
			[R].[HenCount],
			[R].[InFactAbidedPercent],
			[R].[MemberCount],
			[R].[MemberTwentyPercentAFact],
			[R].[PigeonCount],
			[R].[PigeonTwentyPercentAFact],
			[R].[SpeedOfFirst],
			[R].[SpeedOfLast],
			[R].[TimeOfFirst],
			[R].[TimeOfLast],
			[R].[TwentyPercent],
			[R].[YearlyCount],
			[R].[YoungCount]
		FROM [race].[Race] [R] WITH(NOLOCK)
			INNER JOIN [plt].[EntityOrganization] [EO] WITH(NOLOCK) ON [EO].[EntityInfoId] = [R].[EntityInfoId]
			INNER JOIN [race].[Season] [S] WITH(NOLOCK) ON [S].[Id] = [R].[SeasonId]
			INNER JOIN [race].[Point] [P] WITH(NOLOCK) ON [P].[Id] = [R].[PointId]
			INNER JOIN [acc].[Address] [A] WITH(NOLOCK) ON [A].[Id] = [P].[AddressId]
		WHERE [R].[Id] = @RaceId AND EXISTS (SELECT * FROM [race].[RaceResult] [RR] WITH(NOLOCK) WHERE [RR].[RaceId] = [R].[Id])

		SELECT
			[M].[Id],
			[M].[FirstName],
			[M].[LastName],
			[M].[MiddleName],
			[acc].[fnGetOrganizationName]([EO].[OrganizationId], @UserOrganizations) AS [OrganizationName],
			(
				SELECT COUNT([RP1].[PigeonId])
				FROM [race].[RacePigeon] [RP1] WITH(NOLOCK)
					INNER JOIN [acc].[Pigeon] [P1] WITH(NOLOCK) ON [P1].[Id] = [RP1].[PigeonId] AND [P1].[MemberId] = [M].[Id]
				WHERE [RP1].[RaceId] = @RaceId
			) AS [StatementCount],
			(
				SELECT COUNT([RR2].[PigeonId])
				FROM [race].[RaceResult] [RR2] WITH(NOLOCK)
					INNER JOIN [acc].[Pigeon] [P2] WITH(NOLOCK) ON [P2].[Id] = [RR2].[PigeonId] AND [P2].[MemberId] = [M].[Id]
				WHERE [RR2].[RaceId] = @RaceId AND [RR2].[Ac] = 1
			) AS [PrizeCount],
			(
				SELECT MAX([RRC3].[Mark])
				FROM [race].[RaceResult] [RR3] WITH(NOLOCK)
					INNER JOIN [acc].[Pigeon] [P3] WITH(NOLOCK) ON [P3].[Id] = [RR3].[PigeonId] AND [P3].[MemberId] = [M].[Id]
					INNER JOIN [race].[RaceResultCategory] [RRC3] WITH(NOLOCK) ON [RRC3].[RaceResultId] = [RR3].[Id] 
						AND [RRC3].[Id] = (SELECT MIN([RRC].[Id]) FROM [race].[RaceResultCategory] [RRC] WITH(NOLOCK) WHERE [RRC].[RaceResultId] = [RR3].[Id])
				WHERE [RR3].[RaceId] = @RaceId AND [RR3].[Ac] = 1
				--ORDER BY [RRC3].[Mark] DESC
			) AS [Mark],
			(
				SELECT MAX([RRC4].[Mark])
				FROM [race].[RaceResult] [RR4] WITH(NOLOCK)
					INNER JOIN [acc].[Pigeon] [P4] WITH(NOLOCK) ON [P4].[Id] = [RR4].[PigeonId] AND [P4].[MemberId] = [M].[Id]
					INNER JOIN [race].[RaceResultCategory] [RRC4] WITH(NOLOCK) ON [RRC4].[RaceResultId] = [RR4].[Id] 
						AND [RRC4].[Id] = (SELECT MIN([RRC].[Id]) FROM [race].[RaceResultCategory] [RRC] WITH(NOLOCK) WHERE [RRC].[RaceResultId] = [RR4].[Id])
					INNER JOIN [race].[Race] [R] WITH(NOLOCK) ON [R].[SeasonId] = @SeasonId AND [R].[StartRaceTime] <= @StartRaceTime
				WHERE [RR4].[Ac] = 1
				--ORDER BY [RRC4].[Mark] DESC
			) AS [MarkTotal]
		FROM [acc].[Member] [M] WITH(NOLOCK)
			INNER JOIN [plt].[EntityOrganization] [EO] WITH(NOLOCK) ON [EO].[EntityInfoId] = [M].[EntityInfoId]
		WHERE EXISTS(
			SELECT * 
			FROM [acc].[Pigeon] [P] WITH(NOLOCK) 
				INNER JOIN [race].[RacePigeon] [RP] WITH(NOLOCK) ON [RP].[PigeonId] = [P].[Id] AND [RP].[RaceId] = @RaceId
				INNER JOIN [race].[Statement] [S] WITH(NOLOCK) ON [S].[PigeonId] = [P].[Id] AND [S].[SeasonId] = @SeasonId
			WHERE [P].[MemberId] = [M].[Id]
		)
		ORDER BY [MarkTotal] DESC
	END;
END;
