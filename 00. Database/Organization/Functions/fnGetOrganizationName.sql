CREATE FUNCTION [acc].[fnGetOrganizationName] (
	@OrganizationId INT,
	@UserOrganizations [udt].[KeyList] READONLY
) RETURNS NVARCHAR(512)
AS
BEGIN
	DECLARE @UserOrganizationOrder INT;

	SELECT TOP (1)
		@UserOrganizationOrder = [OR].[Order]
	FROM [acc].[OrganizationRelation] [OR] WITH(NOLOCK)
		LEFT JOIN @UserOrganizations [UO] ON [UO].[Id] = [OR].[RelatedOrganizationId]
	WHERE [OR].[OrganizationId] = @OrganizationId AND [UO].[Id] IS NOT NULL
	ORDER BY [OR].[Order] DESC;

	IF @UserOrganizationOrder IS NULL
		SET @UserOrganizationOrder = 1

	DECLARE @Name NVARCHAR(512) = '';

	SELECT
		@Name = @Name + ' > ' + [RO].[Name]
	FROM [acc].[OrganizationRelation] [OR] WITH(NOLOCK)
		INNER JOIN [acc].[Organization] [O] WITH(NOLOCK) ON [O].[Id] = [OR].[OrganizationId]
		INNER JOIN [plt].[EntityInfo] [OEI] WITH(NOLOCK) ON [OEI].[Id] = [O].[EntityInfoId]
		INNER JOIN [acc].[Organization] [RO] WITH(NOLOCK) ON [RO].[Id] = [OR].[RelatedOrganizationId]
		INNER JOIN [plt].[EntityInfo] [REI] WITH(NOLOCK) ON [REI].[Id] = [RO].[EntityInfoId]
	WHERE 1 = 1
		AND [OR].[OrganizationId] = @OrganizationId
		AND [OR].[Order] <= @UserOrganizationOrder
	ORDER BY [OR].[Order] DESC;

	RETURN SUBSTRING(@Name, 4, LEN(@Name));
END