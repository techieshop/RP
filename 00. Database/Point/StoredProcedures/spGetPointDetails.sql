CREATE PROCEDURE [race].[spGetPointDetails]
	@UserId INT,
	@ContextOrganizationId INT,
	@PointId INT
AS
BEGIN
	DECLARE @PointEntityTypeAccess [udt].[EntityAccess];
	INSERT INTO @PointEntityTypeAccess
	EXEC [acc].[spGetPointEntityTypeAccess] @UserId, @ContextOrganizationId;

	DECLARE @PointEntityInfoId INT;
	DECLARE @AccessTypeId INT;
	SELECT 
		@PointEntityInfoId = [P].[EntityInfoId],
		@AccessTypeId = [ETA].[AccessTypeId]
	FROM [race].[Point] [P] WITH(NOLOCK)
		INNER JOIN @PointEntityTypeAccess [ETA] ON [ETA].[EntityInfoId] = [P].[EntityInfoId]
	WHERE [P].[Id] = @PointId;

	IF (@PointEntityInfoId IS NOT NULL AND @PointEntityInfoId > 0)
	BEGIN
		DECLARE @UserOrganizations [udt].[KeyList];
		INSERT INTO @UserOrganizations
		SELECT [OrganizationId] FROM [acc].[fnGetOrganizations] (@UserId, @ContextOrganizationId);

		SELECT
			[P].[Id],
			[P].[EntityInfoId],
			@AccessTypeId AS [AccessTypeId],
			[EO].[EntityTypeId],
			[EO].[EntityStateId],
			[EO].[OrganizationId],
			[acc].[fnGetOrganizationName]([EO].[OrganizationId], @UserOrganizations) AS [OrganizationName],
			[P].[Name]
		FROM [race].[Point] [P] WITH(NOLOCK)
			INNER JOIN [plt].[EntityOrganization] [EO] WITH(NOLOCK) ON [EO].[EntityInfoId] = [P].[EntityInfoId]
		WHERE [P].[Id] = @PointId;

		SELECT
			[A].[Id],
			[A].[City],
			[A].[PostalCode],
			[A].[Street],
			[A].[Number],
			[A].[FormattedAddress],
			[A].[Latitude],
			[A].[Longitude]
		FROM [race].[Point] [P] WITH(NOLOCK)
			INNER JOIN [acc].[Address] [A] WITH(NOLOCK) ON [A].[Id] = [P].[AddressId]
		WHERE [P].[Id] = @PointId;
	END;
END;
