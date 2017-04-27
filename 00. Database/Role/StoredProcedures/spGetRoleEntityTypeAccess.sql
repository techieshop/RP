CREATE PROCEDURE [acc].[spGetRoleEntityTypeAccess]
	@UserId INT,
	@ContextOrganizationId INT,
	@EntityStateIds [udt].[KeyList] READONLY,
	@IsActiveOnly BIT = 0
AS
BEGIN
	DECLARE @RoleEntityTypeId INT = 8;
	DECLARE @PlatformId INT = 1;

	DECLARE @UserEntityTypeAuth [udt].[UserEntityTypeAuth]
	INSERT INTO @UserEntityTypeAuth
	SELECT * FROM [acc].[fnGetUserEntityTypeAuthSingle](@UserId, @ContextOrganizationId, @RoleEntityTypeId, @EntityStateIds, @IsActiveOnly);

	DECLARE @OrganizationIds [udt].[KeyList];
	INSERT INTO @OrganizationIds
	SELECT DISTINCT [OrganizationId] FROM @UserEntityTypeAuth;

	SELECT DISTINCT
		[EO].[EntityInfoId],
		[EO].[OrganizationId],
		[EO].[EntityTypeId],
		[EO].[EntityStateId],
		[UEA].[AccessTypeId]
	FROM @OrganizationIds [OI]
		INNER JOIN [plt].[EntityOrganization] [EO] WITH(NOLOCK) ON [EO].[OrganizationId] = @PlatformId AND [EO].[EntityTypeId] = @RoleEntityTypeId
		INNER JOIN @UserEntityTypeAuth [UEA] ON [UEA].[EntityStateId] = [EO].[EntityStateId] AND [UEA].[OrganizationId] = [OI].[Id]
		INNER JOIN [acc].[Role] [R] WITH(NOLOCK) ON [R].[EntityInfoId] = [EO].[EntityInfoId]
		INNER JOIN [acc].[RoleEntityTypeAccess] [RETA] WITH(NOLOCK) ON [RETA].[RoleId] = [R].[Id]
		INNER JOIN [acc].[RoleEntityStateAccess] [RESA] WITH(NOLOCK) ON [RESA].[RoleId] = [R].[Id]
		INNER JOIN [acc].[RoleEntityStateTransitionAccess] [RESTA] WITH(NOLOCK) ON [RESTA].[RoleId] = [R].[Id]
	WHERE EXISTS(
		SELECT
			*
		FROM [acc].[UserRole] [UR] WITH(NOLOCK)
			INNER JOIN [acc].[RoleEntityTypeAccess] [tRETA] WITH(NOLOCK) ON
					[tRETA].[RoleId] = [UR].[RoleId]
				AND [tRETA].[EntityTypeId] = [RETA].[EntityTypeId]
				AND [tRETA].[AccessTypeId] <= [RETA].[AccessTypeId]
			INNER JOIN [acc].[RoleEntityStateAccess] [tRESA] WITH(NOLOCK) ON
					[tRESA].[RoleId] = [UR].[RoleId]
				AND [tRESA].[EntityTypeId] = [tRETA].[EntityTypeId]
				AND [tRESA].[EntityStateId] = [RESA].[EntityStateId]
				AND [tRESA].[AccessTypeId] <= [RESA].[AccessTypeId]
			INNER JOIN [acc].[RoleEntityStateTransitionAccess] [tRESTA] WITH(NOLOCK) ON
					[tRESTA].[RoleId] = [UR].[RoleId]
				AND [tRESTA].[EntityTypeId] = [tRETA].[EntityTypeId]
				AND [tRESTA].[EntityStateTransitionId] = [RESTA].[EntityStateTransitionId]
		WHERE [UR].[UserId] = @UserId
	)
END