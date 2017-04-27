CREATE PROCEDURE [acc].[spGetOrganizations]
	@UserId INT,
	@ContextOrganizationId INT,
	@OrganizationTypeIds [udt].[KeyList] READONLY
AS
BEGIN
	DECLARE @OrganizationEntityTypeAccess [udt].[EntityAccess];
	INSERT INTO @OrganizationEntityTypeAccess
	EXEC [acc].[spGetOrganizationEntityTypeAccess] @UserId, @ContextOrganizationId;

	DECLARE @UserOrganizations [udt].[KeyList];
	INSERT INTO @UserOrganizations
	SELECT [OrganizationId] FROM [acc].[fnGetOrganizations] (@UserId, @ContextOrganizationId);

	;WITH [ItemsFilter] AS (
		SELECT
			[O].[Id],
			[acc].[fnGetOrganizationName]([O].[Id], @UserOrganizations) AS [Text],
			ROW_NUMBER() OVER (ORDER BY [acc].[fnGetOrganizationPath]([O].[Id]) ASC) AS [RowNumber]
		FROM [acc].[Organization] [O] WITH(NOLOCK)
			INNER JOIN @OrganizationEntityTypeAccess [ETA] ON [ETA].[OrganizationId] = [O].[Id]
		WHERE (EXISTS(SELECT * FROM @OrganizationTypeIds) AND [O].[OrganizationTypeId] IN (SELECT * FROM @OrganizationTypeIds))
	)
	SELECT
		[IF].[Id] AS [Value],
		[IF].[Text]
	FROM [ItemsFilter] [IF]
	ORDER BY [IF].[RowNumber];
END