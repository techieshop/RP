CREATE FUNCTION [acc].[fnGetUserEntityTypeAuth] (
	@UserId INT,
	@ContextOrganizationId INT,
	@EntityState [udt].[EntityState] READONLY
)
RETURNS @UserAccess TABLE (
	[OrganizationId] INT,
	[EntityTypeId] INT,
	[EntityStateId] INT,
	[AccessTypeId] INT
)
AS
BEGIN
	DECLARE @UserRole [udt].[UserRole];
	INSERT INTO @UserRole SELECT * FROM [acc].[fnGetUserRole](@UserId, @ContextOrganizationId);

	IF NOT EXISTS(SELECT * FROM @UserRole) RETURN;

	DECLARE @IsActiveOnlyEntityStateId INT = 0;
	DECLARE @ActualEntityState [udt].[EntityState];

	-- specific EntityStates
	INSERT INTO @ActualEntityState
	SELECT * FROM @EntityState WHERE [EntityStateId] IS NOT NULL AND [EntityStateId] <> 0

	-- all EntityStates
	INSERT INTO @ActualEntityState
	SELECT
		[ES].[EntityTypeId],
		[ESTable].[Id]
	FROM @EntityState [ES]
		INNER JOIN [plt].[EntityState] [ESTable] WITH(NOLOCK) ON [ESTable].[EntityTypeId] = [ES].[EntityTypeId]
	WHERE [ES].[EntityStateId] IS NULL;

	-- active EntityStates
	INSERT INTO @ActualEntityState
	SELECT
		[ES].[EntityTypeId],
		[ESTable].[Id]
	FROM @EntityState [ES]
		INNER JOIN [plt].[EntityState] [ESTable] WITH(NOLOCK) ON [ESTable].[EntityTypeId] = [ES].[EntityTypeId]
	WHERE [ES].[EntityStateId] = @IsActiveOnlyEntityStateId AND [ESTable].[IsActive] = 1;

	INSERT INTO @UserAccess
	SELECT
		[UR].[OrganizationId],
		[RESA].[EntityTypeId],
		[RESA].[EntityStateId],
		MAX([RESA].[AccessTypeId]) AS [AccessTypeId]
	FROM @UserRole [UR]
		INNER JOIN [acc].[RoleEntityStateAccess] [RESA] WITH(NOLOCK) ON [RESA].[RoleId] = [UR].[RoleId]
		INNER JOIN @ActualEntityState [AES] ON [AES].[EntityTypeId] = [RESA].[EntityTypeId] AND [AES].[EntityStateId] = [RESA].[EntityStateId]
	GROUP BY [UR].[OrganizationId], [RESA].[EntityTypeId], [RESA].[EntityStateId];

	RETURN;
END