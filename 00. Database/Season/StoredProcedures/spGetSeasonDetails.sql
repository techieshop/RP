CREATE PROCEDURE [race].[spGetSeasonDetails]
	@UserId INT,
	@ContextOrganizationId INT,
	@SeasonId INT
AS
BEGIN
	DECLARE @SeasonEntityTypeAccess [udt].[EntityAccess];
	INSERT INTO @SeasonEntityTypeAccess
	EXEC [acc].[spGetSeasonEntityTypeAccess] @UserId, @ContextOrganizationId;

	DECLARE @SeasonEntityInfoId INT;
	DECLARE @AccessTypeId INT;
	DECLARE @OrganizationId INT;

	SELECT 
		@SeasonEntityInfoId = [S].[EntityInfoId],
		@AccessTypeId = [ETA].[AccessTypeId],
		@OrganizationId = [ETA].[OrganizationId]
	FROM [race].[Season] [S] WITH(NOLOCK)
		INNER JOIN @SeasonEntityTypeAccess [ETA] ON [ETA].[EntityInfoId] = [S].[EntityInfoId]
	WHERE [S].[Id] = @SeasonId;

	IF (@SeasonEntityInfoId IS NOT NULL AND @SeasonEntityInfoId > 0)
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
			[S].[Id],
			[S].[EntityInfoId],
			@AccessTypeId AS [AccessTypeId],
			[EO].[EntityTypeId],
			[EO].[EntityStateId],
			[EO].[OrganizationId],
			[acc].[fnGetOrganizationName]([EO].[OrganizationId], @UserOrganizations) AS [OrganizationName],
			[S].[Year],
			[S].[SeasonTypeId],
			(SELECT
				COUNT(DISTINCT [M].[Id])
			FROM [acc].[Pigeon] [P] WITH(NOLOCK)
				INNER JOIN [race].[Statement] [St] WITH(NOLOCK) ON [St].[PigeonId] = [P].[Id] AND [St].[SeasonId] = [S].[Id]
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
				INNER JOIN [race].[Statement] [St] WITH(NOLOCK) ON [St].[PigeonId] = [P].[Id] AND [St].[SeasonId] = [S].[Id]
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
			) AS [PigeonCount]
		FROM [race].[Season] [S] WITH(NOLOCK)
			INNER JOIN [plt].[EntityOrganization] [EO] WITH(NOLOCK) ON [EO].[EntityInfoId] = [S].[EntityInfoId]
		WHERE [S].[Id] = @SeasonId;

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

		SELECT DISTINCT
			[P].[Id],
			[P].[Year],
			[P].[Code],
			[P].[Number],
			[P].[MemberId]
		FROM [acc].[Pigeon] [P] WITH(NOLOCK)
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
		FROM [race].[Statement] [S] WITH(NOLOCK)
			INNER JOIN [acc].[Pigeon] [P] WITH(NOLOCK) ON [P].[Id] = [S].[PigeonId]
			INNER JOIN [plt].[EntityOrganization] [PEO] WITH(NOLOCK) ON [PEO].[EntityInfoId] = [P].[EntityInfoId]
			INNER JOIN @PigeonEntityTypeAccess [PETA] ON
				[PETA].[EntityInfoId] = [PEO].[EntityInfoId] AND
				[PETA].[OrganizationId] = [PEO].[OrganizationId] AND
				[PETA].[EntityStateId] = [PEO].[EntityStateId] AND
				[PETA].[EntityTypeId] = [PEO].[EntityTypeId]
			INNER JOIN [acc].[Member] [M] WITH(NOLOCK) ON [M].[Id] = [P].[MemberId]
			INNER JOIN [plt].[EntityOrganization] [MEO] WITH(NOLOCK) ON [MEO].[EntityInfoId] = [M].[EntityInfoId]
			INNER JOIN @MemberEntityTypeAccess [META] ON
				[META].[EntityInfoId] = [MEO].[EntityInfoId] AND
				[META].[OrganizationId] = [MEO].[OrganizationId] AND
				[META].[EntityStateId] = [MEO].[EntityStateId] AND
				[META].[EntityTypeId] = [MEO].[EntityTypeId]
		WHERE [S].[SeasonId] = @SeasonId;
	END;
END;
