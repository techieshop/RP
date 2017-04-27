CREATE VIEW [acc].[vOrganization]
WITH SCHEMABINDING
AS
	SELECT
		[OR].[OrganizationId] AS [Id],
		SUM(CASE WHEN [ES].[IsActive] = 0 THEN 1 ELSE 0 END) AS [InactiveRelationCount],
		SUM(CASE WHEN [ES].[IsActive] = 0 AND [OR].[Order] != 1 THEN 1 ELSE 0 END) AS [InactiveParentRelationCount],
		COUNT_BIG(*) AS [RelationCount]
	FROM [acc].[OrganizationRelation] [OR]
		INNER JOIN [acc].[Organization] [O] ON [O].[Id] = [OR].[RelatedOrganizationId]
		INNER JOIN [plt].[EntityOrganization] [EO] ON [EO].[EntityInfoId] = [O].[EntityInfoId]
		INNER JOIN [plt].[EntityState] [ES] ON [ES].[Id] = [EO].[EntityStateId]
	GROUP BY [OR].[OrganizationId];
GO

CREATE UNIQUE CLUSTERED INDEX [IX_vOrganization] ON [acc].[vOrganization] ([Id])
GO

CREATE NONCLUSTERED INDEX [IX_vOrganization_InactiveRelationCount] ON [acc].[vOrganization] ([InactiveRelationCount]) INCLUDE ([Id])
GO

CREATE NONCLUSTERED INDEX [IX_vOrganization_InactiveParentRelationCount] ON [acc].[vOrganization] ([InactiveParentRelationCount]) INCLUDE ([Id])
GO
