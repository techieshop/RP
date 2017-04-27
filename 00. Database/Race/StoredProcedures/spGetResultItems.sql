CREATE PROCEDURE [race].[spGetResultItems]
	@UserId INT,
	@ContextOrganizationId INT
AS
BEGIN
	DECLARE @RaceEntityTypeAccess [udt].[EntityAccess];
	INSERT INTO @RaceEntityTypeAccess
	EXEC [acc].[spGetRaceEntityTypeAccess] @UserId, @ContextOrganizationId;

	DECLARE @UserOrganizations [udt].[KeyList];
	INSERT INTO @UserOrganizations
	SELECT [OrganizationId] FROM [acc].[fnGetOrganizations] (@UserId, @ContextOrganizationId);

	SELECT
		[R].[Id],
		[S].[Year],
		[acc].[fnGetOrganizationName]([EO].[OrganizationId], @UserOrganizations) AS [OrganizationName],
		[R].[SeasonId],
		[S].[SeasonTypeId],
		[R].[Name],
		[R].[RaceTypeId]
	FROM [race].[Race] [R] WITH(NOLOCK)
		INNER JOIN [plt].[EntityOrganization] [EO] WITH(NOLOCK) ON [EO].[EntityInfoId] = [R].[EntityInfoId]
		INNER JOIN @RaceEntityTypeAccess [ETA] ON
			[ETA].[EntityInfoId] = [EO].[EntityInfoId] AND
			[ETA].[OrganizationId] = [EO].[OrganizationId] AND
			[ETA].[EntityStateId] = [EO].[EntityStateId] AND
			[ETA].[EntityTypeId] = [EO].[EntityTypeId]
		INNER JOIN [race].[Season] [S] WITH(NOLOCK) ON [S].[Id] = [R].[SeasonId]
	WHERE EXISTS (SELECT * FROM [race].[RaceResult] [RR] WITH(NOLOCK) WHERE [RR].[RaceId] = [R].[Id])
END;