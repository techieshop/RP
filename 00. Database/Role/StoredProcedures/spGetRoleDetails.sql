CREATE PROCEDURE [acc].[spGetRoleDetails]
	@UserId INT,
	@ContextOrganizationId INT,
	@RoleId INT
AS
BEGIN
	DECLARE @RoleEntityTypeAccess [udt].[EntityAccess];
	INSERT INTO @RoleEntityTypeAccess
	EXEC [acc].[spGetRoleEntityTypeAccess] @UserId, @ContextOrganizationId;

	DECLARE @RoleEntityInfoId INT;
	DECLARE @AccessTypeId INT;
	SELECT
		@RoleEntityInfoId = [R].[EntityInfoId],
		@AccessTypeId = [ETA].[AccessTypeId]
	FROM [acc].[Role] [R] WITH(NOLOCK)
		INNER JOIN @RoleEntityTypeAccess [ETA] ON [ETA].[EntityInfoId] = [R].[EntityInfoId]
	WHERE [R].[Id] = @RoleId;

	IF (@RoleEntityInfoId IS NOT NULL AND @RoleEntityInfoId > 0)
	BEGIN
		SELECT
			[R].[Id],
			[R].[EntityInfoId],
			@AccessTypeId AS [AccessTypeId],
			[EO].[EntityTypeId],
			[EO].[EntityStateId],
			[EO].[OrganizationId],
			[R].[Name],
			[R].[Description],
			[EO].[EntityStateId],
			[EO].[OrganizationId]
		FROM [acc].[Role] [R] WITH(NOLOCK)
			INNER JOIN [plt].[EntityOrganization] [EO] WITH(NOLOCK) ON [EO].[EntityInfoId] = [R].[EntityInfoId]
		WHERE [R].[Id] = @RoleId;
	END;
END;
