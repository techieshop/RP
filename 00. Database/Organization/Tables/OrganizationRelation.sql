CREATE TABLE [acc].[OrganizationRelation]
(
	[Id] INT NOT NULL IDENTITY,
	[OrganizationId] INT NOT NULL,
	[RelatedOrganizationId] INT NOT NULL,
	[Order] TINYINT NOT NULL,
	[Level] TINYINT NOT NULL,
	CONSTRAINT [PK_OrganizationRelation] PRIMARY KEY CLUSTERED ([Id]),
	CONSTRAINT [FK_OrganizationRelation_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [acc].[Organization] ([Id]),
	CONSTRAINT [FK_OrganizationRelation_RelatedOrganization] FOREIGN KEY ([RelatedOrganizationId]) REFERENCES [acc].[Organization] ([Id])
)
GO

CREATE NONCLUSTERED INDEX [IX_OrganizationRelation_1] ON [acc].[OrganizationRelation] ([OrganizationId]) INCLUDE ([RelatedOrganizationId], [Order], [Level])
GO

CREATE NONCLUSTERED INDEX [IX_OrganizationRelation_2] ON [acc].[OrganizationRelation] ([RelatedOrganizationId]) INCLUDE ([OrganizationId], [Order], [Level])
GO

CREATE NONCLUSTERED INDEX [IX_OrganizationRelation_3] ON [acc].[OrganizationRelation] ([OrganizationId], [RelatedOrganizationId]) INCLUDE ([Order], [Level])
GO

CREATE NONCLUSTERED INDEX [IX_OrganizationRelation_4] ON [acc].[OrganizationRelation] ([RelatedOrganizationId], [Level]) INCLUDE ([OrganizationId], [Order])
GO

CREATE NONCLUSTERED INDEX [IX_OrganizationRelation_5] ON [acc].[OrganizationRelation] ([RelatedOrganizationId], [Order]) INCLUDE ([OrganizationId], [Level])