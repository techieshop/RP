CREATE TABLE [plt].[EntityOrganization]
(
	[Id] INT NOT NULL IDENTITY,
	[EntityInfoId] INT NOT NULL,
	[EntityTypeId] INT NOT NULL,
	[OrganizationId] INT NOT NULL,
	[EntityStateId] INT NOT NULL,
	CONSTRAINT [PK_EntityOrganization] PRIMARY KEY CLUSTERED ([Id]),
	CONSTRAINT [FK_EntityOrganization_EntityInfo] FOREIGN KEY ([EntityInfoId]) REFERENCES [plt].[EntityInfo] ([Id]),
	CONSTRAINT [FK_EntityOrganization_EntityType] FOREIGN KEY ([EntityTypeId]) REFERENCES [plt].[EntityType] ([Id]),
	CONSTRAINT [FK_EntityOrganization_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [acc].[Organization] ([Id]),
	CONSTRAINT [FK_EntityOrganization_EntityState] FOREIGN KEY ([EntityStateId]) REFERENCES [plt].[EntityState] ([Id])
)
GO

CREATE NONCLUSTERED INDEX [IX_EntityOrganization_EntityInfo] ON [plt].[EntityOrganization] ([EntityInfoId]) INCLUDE ([EntityStateId], [OrganizationId], [EntityTypeId])
GO

CREATE NONCLUSTERED INDEX [IX_EntityOrganization_EntityType] ON [plt].[EntityOrganization] ([EntityTypeId]) INCLUDE ([EntityInfoId], [OrganizationId], [EntityStateId])