CREATE PROCEDURE [race].[spGetResultDetails]
	@UserId INT,
	@ContextOrganizationId INT,
	@RaceId INT
AS
BEGIN
	DECLARE @RaceEntityTypeAccess [udt].[EntityAccess];
	INSERT INTO @RaceEntityTypeAccess
	EXEC [acc].[spGetRaceEntityTypeAccess] @UserId, @ContextOrganizationId;

	DECLARE @RaceEntityInfoId INT;

	SELECT
		@RaceEntityInfoId = [R].[EntityInfoId]
	FROM [race].[Race] [R] WITH(NOLOCK)
		INNER JOIN @RaceEntityTypeAccess [ETA] ON [ETA].[EntityInfoId] = [R].[EntityInfoId]
	WHERE [R].[Id] = @RaceId;

	IF (@RaceEntityInfoId IS NOT NULL AND @RaceEntityInfoId > 0)
	BEGIN
		DECLARE @UserOrganizations [udt].[KeyList];
		INSERT INTO @UserOrganizations
		SELECT [OrganizationId] FROM [acc].[fnGetOrganizations] (@UserId, @ContextOrganizationId);

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
	END;
END;
