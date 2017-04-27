CREATE PROCEDURE [acc].[spGetUserAuth]
	@UserId INT
AS
BEGIN
	IF @UserId IS NOT NULL
	BEGIN
		DECLARE @UserRole [udt].[UserRole];
		INSERT INTO @UserRole
		SELECT * FROM [acc].[fnGetUserRole](@UserId, 1);

		IF EXISTS(SELECT * FROM @UserRole)
		BEGIN

			-- UserData

			SELECT
				[U].[Id],
				[EI].[Guid],
				[U].[HasPhoto],
				[U].[FirstName],
				[U].[Email],
				[U].[LanguageId],
				[EO].[OrganizationId]
			FROM [acc].[User] [U] WITH(NOLOCK)
				INNER JOIN [plt].[EntityInfo] [EI] WITH(NOLOCK) ON [EI].[Id] = [U].[EntityInfoId]
				INNER JOIN [plt].[EntityOrganization] [EO] WITH(NOLOCK) ON [EO].[EntityInfoId] = [EI].[Id]
			WHERE [U].[Id] = @UserId;

			-- EntityTypeAccess

			SELECT
				[RETA].[EntityTypeId],
				MAX([RETA].[AccessTypeId]) AS [AccessTypeId]
			FROM @UserRole [UR]
				INNER JOIN [acc].[RoleEntityTypeAccess] [RETA] WITH(NOLOCK) ON [RETA].[RoleId] = [UR].[RoleId]
			GROUP BY [UR].[OrganizationId], [RETA].[EntityTypeId];

			-- EntityStateAccess

			SELECT
				[UR].[OrganizationId],
				[RESA].[EntityTypeId],
				[RESA].[EntityStateId],
				MAX([RESA].[AccessTypeId]) AS [AccessTypeId]
			FROM @UserRole [UR]
				INNER JOIN [acc].[RoleEntityStateAccess] [RESA] WITH(NOLOCK) ON [RESA].[RoleId] = [UR].[RoleId]
			GROUP BY [UR].[OrganizationId], [RESA].[EntityTypeId], [RESA].[EntityStateId];

			-- EntityStateTransitionAccess

			SELECT
				[UR].[OrganizationId],
				[RESTA].[EntityTypeId],
				[RESTA].[EntityStateTransitionId]
			FROM @UserRole [UR]
				INNER JOIN [acc].[RoleEntityStateTransitionAccess] [RESTA] WITH(NOLOCK) ON [RESTA].[RoleId] = [UR].[RoleId]
			GROUP BY [UR].[OrganizationId], [RESTA].[EntityTypeId], [RESTA].[EntityStateTransitionId];
		END
	END
END
