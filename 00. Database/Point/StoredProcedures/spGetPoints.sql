CREATE PROCEDURE [race].[spGetPoints]
	@UserId INT,
	@ContextOrganizationId INT,
	@OrganizationId INT = NULL
AS
BEGIN
	DECLARE @PointEntityTypeAccess [udt].[EntityAccess];
	INSERT INTO @PointEntityTypeAccess
	EXEC [acc].[spGetPointEntityTypeAccess] @UserId, @ContextOrganizationId, DEFAULT, 1;

	;WITH [ItemsFilter] AS (
		SELECT
			[P].[Id],
			[P].[Name] AS [Text],
			ROW_NUMBER() OVER (ORDER BY [P].[Name] ASC) AS [RowNumber]
		FROM [race].[Point] [P] WITH(NOLOCK)
			INNER JOIN @PointEntityTypeAccess [ETA] ON [ETA].[EntityInfoId] = [P].[EntityInfoId] AND (@OrganizationId IS NOT NULL AND [ETA].[OrganizationId] = @OrganizationId)
	)
	SELECT
		[IF].[Id] AS [Value],
		[IF].[Text]
	FROM [ItemsFilter] [IF]
	ORDER BY [IF].[RowNumber];
END
