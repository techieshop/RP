CREATE PROCEDURE [acc].[spGetPigeonDetails]
	@UserId INT,
	@ContextOrganizationId INT,
	@PigeonId INT
AS
BEGIN
	DECLARE @PigeonEntityTypeAccess [udt].[EntityAccess];
	INSERT INTO @PigeonEntityTypeAccess
	EXEC [acc].[spGetPigeonEntityTypeAccess] @UserId, @ContextOrganizationId;

	DECLARE @PigeonEntityInfoId INT;
	DECLARE @AccessTypeId INT;
	SELECT
		@PigeonEntityInfoId = [P].[EntityInfoId],
		@AccessTypeId = [ETA].[AccessTypeId]
	FROM [acc].[Pigeon] [P] WITH(NOLOCK)
		INNER JOIN @PigeonEntityTypeAccess [ETA] ON [ETA].[EntityInfoId] = [P].[EntityInfoId]
	WHERE [P].[Id] = @PigeonId;

	IF (@PigeonEntityInfoId IS NOT NULL AND @PigeonEntityInfoId > 0)
	BEGIN
		DECLARE @UserOrganizations [udt].[KeyList];
		INSERT INTO @UserOrganizations
		SELECT [OrganizationId] FROM [acc].[fnGetOrganizations] (@UserId, @ContextOrganizationId);

		DECLARE @AC BIT = 1;

		SELECT
			[P].[Id],
			[P].[EntityInfoId],
			@AccessTypeId AS [AccessTypeId],
			[EO].[EntityTypeId],
			[EO].[EntityStateId],
			[EO].[OrganizationId],
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
			INNER JOIN [plt].[EntityOrganization] [EO] WITH(NOLOCK) ON [EO].[EntityInfoId] = [P].[EntityInfoId]
			INNER JOIN [acc].[Member] [M] WITH(NOLOCK) ON [M].[Id] = [P].[MemberId]
		WHERE [P].[Id] = @PigeonId;
	END
END;
