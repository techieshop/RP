CREATE PROCEDURE [acc].[spGetMembers]
	@UserId INT,
	@ContextOrganizationId INT
AS
BEGIN
	DECLARE @MemberEntityTypeAccess [udt].[EntityAccess];
	INSERT INTO @MemberEntityTypeAccess
	EXEC [acc].[spGetMemberEntityTypeAccess] @UserId, @ContextOrganizationId;

	;WITH [ItemsFilter] AS (
		SELECT
			[M].[Id],
			(COALESCE([M].[LastName] + ' ', '') + COALESCE([M].[FirstName] + ' ', '') + COALESCE([M].[MiddleName], '')) AS [Text]
		FROM [acc].[Member] [M] WITH(NOLOCK)
			INNER JOIN @MemberEntityTypeAccess [ETA] ON [ETA].[EntityInfoId] = [M].[EntityInfoId]
	)
	SELECT
		[IF].[Id] AS [Value],
		[IF].[Text]
	FROM [ItemsFilter] [IF]
	ORDER BY [IF].[Text];
END;