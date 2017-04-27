CREATE PROCEDURE [race].[spGetResultTime]
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

	SELECT 
		@RaceEntityInfoId = [R].[EntityInfoId],
		@OrganizationId = [ETA].[OrganizationId]
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
			[R].[Name],
			[R].[StartRaceTime],
			[S].[Year],
			[S].[SeasonTypeId]
		FROM [race].[Race] [R] WITH(NOLOCK)
			INNER JOIN [race].[Season] [S] WITH(NOLOCK) ON [S].[Id] = [R].[SeasonId]
		WHERE [R].[Id] = @RaceId AND EXISTS (SELECT * FROM [race].[RaceResult] [RR] WITH(NOLOCK) WHERE [RR].[RaceId] = [R].[Id])

		SELECT
			[RR].[Position],
			[P].[SexId],
			[P].[Id],
			[RR].[Distance],
			[PRT].[ReturnTime],
			[RR].[FlyTime],
			[RR].[Speed],
			[RRC].[Mark],
			[RRC].[Coefficient],
			[RR].[Ac],
			[M].[FirstName],
			[M].[LastName],
			[M].[MiddleName],
			[acc].[fnGetOrganizationName]([EO].[OrganizationId], @UserOrganizations) AS [OrganizationName]
		FROM [race].[RaceResult] [RR] WITH(NOLOCK)
			INNER JOIN [acc].[Pigeon] [P] WITH(NOLOCK) ON [P].[Id] = [RR].[PigeonId]
			INNER JOIN [acc].[Member] [M] WITH(NOLOCK) ON [M].[Id] = [P].[MemberId]
			INNER JOIN [plt].[EntityOrganization] [EO] WITH(NOLOCK) ON [EO].[EntityInfoId] = [M].[EntityInfoId]
			INNER JOIN [race].[PigeonReturnTime] [PRT] WITH(NOLOCK) ON [PRT].[PigeonId] = [P].[Id] AND [PRT].[RaceId] = [RR].[RaceId]
			LEFT JOIN [race].[RaceResultCategory] [RRC] WITH(NOLOCK) ON [RRC].[RaceResultId] = [RR].[Id] AND [RRC].[Id] = (SELECT MIN([RRC].[Id]) FROM [race].[RaceResultCategory] [RRC] WITH(NOLOCK) WHERE [RRC].[RaceResultId] = [RR].[Id])
		WHERE [RR].[RaceId] = @RaceId
		ORDER BY [RR].[Position]
	END;
END;
