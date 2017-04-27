CREATE FUNCTION [acc].[fnGetOrganizationPath] (
	@OrganizationId INT
) RETURNS NVARCHAR(512)
AS
BEGIN
	DECLARE @Name NVARCHAR(512) = '';

	SELECT
		@Name = @Name + (ISNULL([O].[Name], '')) + CASE WHEN [OR].[Order] > 1 THEN ' > ' ELSE '' END
	FROM [acc].[OrganizationRelation] [OR] WITH(NOLOCK)
		INNER JOIN [acc].[Organization] [O] WITH(NOLOCK) ON [O].[Id] = [OR].[RelatedOrganizationId]
	WHERE [OR].[OrganizationId] = @OrganizationId
	ORDER BY [OR].[Order] DESC;

	RETURN @Name
END