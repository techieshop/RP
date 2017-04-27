CREATE PROCEDURE [race].[spGetRaceResultReturnTimes]
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
			[acc].[fnGetOrganizationName]([EO].[OrganizationId], @UserOrganizations) AS [OrganizationName],
			[R].[Name],
			[R].[RaceTypeId],
			[R].[PointId],
			[S].[Year],
			[S].[SeasonTypeId],
			[P].[Name] AS [PointName]
		FROM [race].[Race] [R] WITH(NOLOCK)
			INNER JOIN [plt].[EntityOrganization] [EO] WITH(NOLOCK) ON [EO].[EntityInfoId] = [R].[EntityInfoId]
			INNER JOIN [race].[Season] [S] WITH(NOLOCK) ON [S].[Id] = [R].[SeasonId]
			INNER JOIN [race].[Point] [P] WITH(NOLOCK) ON [P].[Id] = [R].[PointId]
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
			INNER JOIN [race].[RacePigeon] [RP] WITH(NOLOCK) ON [RP].[RaceId] = @RaceId AND [RP].[PigeonId] = [P].[Id]
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
			INNER JOIN [race].[RacePigeon] [RP] WITH(NOLOCK) ON [RP].[RaceId] = @RaceId AND [RP].[PigeonId] = [P].[Id]
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
			[P].[Id],
			[PRT].[ReturnTime]
		FROM [race].[PigeonReturnTime] [PRT] WITH(NOLOCK)
			INNER JOIN [acc].[Pigeon] [P] WITH(NOLOCK) ON [P].[Id] = [PRT].[PigeonId]
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
		WHERE [PRT].[RaceId] = @RaceId;
	END;
END;
