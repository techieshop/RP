CREATE FUNCTION [acc].[fnGetUserRole] (
	@UserId INT,
	@ContextOrganizationId INT
)
RETURNS @UserRole TABLE (
	[OrganizationId] INT,
	[RoleId] INT
)
AS
BEGIN
	DECLARE @UserOrganization [udt].[KeyList];

	INSERT INTO @UserOrganization
	SELECT
		[EO].[OrganizationId]
	FROM [acc].[User] [U] WITH(NOLOCK)
		INNER JOIN [plt].[EntityOrganization] [EO] WITH(NOLOCK) ON [EO].[EntityInfoId] = [U].[EntityInfoId]
		INNER JOIN [plt].[EntityState] [ES] WITH(NOLOCK) ON [ES].[Id] = [EO].[EntityStateId]
		INNER JOIN [acc].[vOrganization] [vO] WITH(NOEXPAND) ON [vO].[Id] = [EO].[OrganizationId]
	WHERE [U].[Id] = @UserId AND [vO].[InactiveRelationCount] = 0 AND [ES].[IsActive] = 1;

	DECLARE @UserContextOrganization AS TABLE (
		[OrganizationId] INT,
		[ActualOrganizationId] INT
	);

	INSERT INTO @UserContextOrganization
	SELECT
		[UO].[Id],
		[UO].[Id]
	FROM @UserOrganization [UO]
		INNER JOIN [acc].[OrganizationRelation] [OR] WITH(NOLOCK) ON [OR].[OrganizationId] = [UO].[Id] AND [OR].[RelatedOrganizationId] = @ContextOrganizationId;

	-- Check if a User Organization is Parent for Context Organization
	IF NOT EXISTS(SELECT * FROM @UserContextOrganization)
	BEGIN
		INSERT INTO @UserContextOrganization
		SELECT
			[UO].[Id],
			@ContextOrganizationId
		FROM @UserOrganization [UO]
			INNER JOIN [acc].[OrganizationRelation] [OR] WITH(NOLOCK) ON [OR].[OrganizationId] = @ContextOrganizationId AND [OR].[RelatedOrganizationId] = [UO].[Id];
	END

	IF EXISTS(SELECT * FROM @UserContextOrganization)
	BEGIN
		INSERT INTO @UserRole
		SELECT
			[UCO].[ActualOrganizationId],
			[UR].[RoleId]
		FROM [acc].[UserRole] [UR] WITH(NOLOCK)
			INNER JOIN @UserContextOrganization [UCO] ON [UCO].[OrganizationId] = [UR].[OrganizationId]
		WHERE [UR].[UserId] = @UserId
			AND NOT EXISTS ( -- exclude roles that are explicitly disabled by any of the parents
				SELECT
					[R].[Id]
				FROM [acc].[Role] [R] WITH(NOLOCK)
					INNER JOIN [plt].[EntityOrganization] [EO] WITH(NOLOCK) ON [EO].[EntityInfoId] = [R].[EntityInfoId]
					INNER JOIN [acc].[OrganizationRelation] [OR] WITH(NOLOCK) ON [OR].[OrganizationId] = [UR].[OrganizationId] AND [OR].[RelatedOrganizationId] = [EO].[OrganizationId]
					INNER JOIN [plt].[EntityState] [ES] WITH(NOLOCK) ON [ES].[Id] = [EO].[EntityStateId]
				WHERE [R].[Id] = [UR].[RoleId] AND [ES].[IsActive] = 0
			);
	END

	RETURN;
END