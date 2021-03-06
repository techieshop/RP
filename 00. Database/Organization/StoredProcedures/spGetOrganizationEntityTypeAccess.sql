﻿CREATE PROCEDURE [acc].[spGetOrganizationEntityTypeAccess]
	@UserId INT,
	@ContextOrganizationId INT,
	@EntityStateIds [udt].[KeyList] READONLY,
	@IsActiveOnly BIT = 0
AS
BEGIN
	DECLARE @OrganizationEntityTypeId INT = 2;

	DECLARE @UserEntityTypeAuth [udt].[UserEntityTypeAuth]
	INSERT INTO @UserEntityTypeAuth
	SELECT * FROM [acc].[fnGetUserEntityTypeAuthSingle](@UserId, @ContextOrganizationId, @OrganizationEntityTypeId, @EntityStateIds, @IsActiveOnly);

	DECLARE @OrganizationIds [udt].[KeyList];
	INSERT INTO @OrganizationIds
	SELECT DISTINCT [OrganizationId] FROM @UserEntityTypeAuth;

	SELECT
		[EO].[EntityInfoId],
		[EO].[OrganizationId],
		[EO].[EntityTypeId],
		[EO].[EntityStateId],
		[UEA].[AccessTypeId]
	FROM @OrganizationIds [OI]
		INNER JOIN [acc].[OrganizationRelation] [OR] WITH(NOLOCK) ON [OR].[RelatedOrganizationId] = [OI].[Id]
		INNER JOIN [acc].[vOrganization] [vO] WITH(NOEXPAND) ON [vO].[Id] = [OR].[OrganizationId]
		INNER JOIN [plt].[EntityOrganization] [EO] WITH(NOLOCK) ON [EO].[OrganizationId] = [OR].[OrganizationId] AND [EO].[EntityTypeId] = @OrganizationEntityTypeId
		INNER JOIN @UserEntityTypeAuth [UEA] ON [UEA].[EntityStateId] = [EO].[EntityStateId] AND [UEA].[OrganizationId] = [OI].[Id]
	OPTION (FORCE ORDER);
END