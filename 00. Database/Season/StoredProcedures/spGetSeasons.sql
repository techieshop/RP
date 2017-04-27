CREATE PROCEDURE [race].[spGetSeasons]
	@UserId INT,
	@ContextOrganizationId INT,
	@OrganizationId INT = NULL
AS
BEGIN
	DECLARE @SeasonEntityTypeAccess [udt].[EntityAccess];
	INSERT INTO @SeasonEntityTypeAccess
	EXEC [acc].[spGetSeasonEntityTypeAccess] @UserId, @ContextOrganizationId, DEFAULT, 1;

	;WITH [ItemsFilter] AS (
		SELECT
			[S].[Id],
			([T].[Value] + '-'+ CAST ( [S].[Year] AS NVARCHAR(4))) AS [Text],
			ROW_NUMBER() OVER (ORDER BY [S].[Year] DESC, [S].[SeasonTypeId] DESC ) AS [RowNumber]
		FROM [race].[Season] [S] WITH(NOLOCK)
			INNER JOIN @SeasonEntityTypeAccess [ETA] ON [ETA].[EntityInfoId] = [S].[EntityInfoId] AND (@OrganizationId IS NOT NULL AND [ETA].[OrganizationId] = @OrganizationId)
			INNER JOIN [dom].[DomainValue] [DV] WITH(NOLOCK) ON [DV].[Id] = [S].[SeasonTypeId]
			INNER JOIN [stl].[Translation] [T] WITH(NOLOCK) ON [T].[TranslationCodeId] = [DV].[NameCode] 
			INNER JOIN [acc].[User] [U] WITH(NOLOCK) ON [U].[Id] = @UserId AND [U].[LanguageId] = [T].[LanguageId]
	)
	SELECT
		[IF].[Id] AS [Value],
		[IF].[Text]
	FROM [ItemsFilter] [IF]
	ORDER BY [IF].[RowNumber];
END
