CREATE PROCEDURE [acc].[spGetRoles]
	@UserId INT,
	@ContextOrganizationId INT
AS
BEGIN
	DECLARE @RoleEntityTypeAccess [udt].[EntityAccess];
	INSERT INTO @RoleEntityTypeAccess
	EXEC [acc].[spGetRoleEntityTypeAccess] @UserId, @ContextOrganizationId;

	;WITH [ItemsFilter] AS (
		SELECT
			[R].[Id],
			[R].[Name] AS [Text]
		FROM [acc].[Role] [R] WITH(NOLOCK)
			INNER JOIN @RoleEntityTypeAccess [ETA] ON [ETA].[EntityInfoId] = [R].[EntityInfoId]
	)
	SELECT
		[IF].[Id] AS [Value],
		[IF].[Text]
	FROM [ItemsFilter] [IF]
	ORDER BY [IF].[Text];
END;