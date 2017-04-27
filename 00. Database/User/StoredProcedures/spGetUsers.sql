CREATE PROCEDURE [acc].[spGetUsers]
	@UserId INT,
	@ContextOrganizationId INT
AS
BEGIN
	DECLARE @UserEntityTypeAccess [udt].[EntityAccess];
	INSERT INTO @UserEntityTypeAccess
	EXEC [acc].[spGetUserEntityTypeAccess] @UserId, @ContextOrganizationId;

	;WITH [ItemsFilter] AS (
		SELECT
			[U].[Id],
			(COALESCE([U].[LastName] + ' ', '') + COALESCE([U].[FirstName] + ' ', '') + COALESCE([U].[MiddleName] + ' ', '') + '('+ [U].[Email] +')') AS [Text]
		FROM [acc].[User] [U] WITH(NOLOCK)
			INNER JOIN @UserEntityTypeAccess [ETA] ON [ETA].[EntityInfoId] = [U].[EntityInfoId]
	)
	SELECT
		[IF].[Id] AS [Value],
		[IF].[Text]
	FROM [ItemsFilter] [IF]
	ORDER BY [IF].[Text];
END;