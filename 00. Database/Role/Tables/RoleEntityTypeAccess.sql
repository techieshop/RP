CREATE TABLE [acc].[RoleEntityTypeAccess]
(
	[Id] INT NOT NULL IDENTITY,
	[RoleId] INT NOT NULL,
	[EntityTypeId] INT NOT NULL,
	[AccessTypeId] INT NOT NULL,
	CONSTRAINT [PK_RoleEntityTypeAccess] PRIMARY KEY CLUSTERED ([Id]),
	CONSTRAINT [FK_RoleEntityTypeAccess_Role] FOREIGN KEY ([RoleId]) REFERENCES [acc].[Role] ([Id]),
	CONSTRAINT [FK_RoleEntityTypeAccess_EntityType] FOREIGN KEY ([EntityTypeId]) REFERENCES [plt].[EntityType] ([Id])
)
GO

CREATE NONCLUSTERED INDEX [IX_RoleEntityTypeAccess_RoleId] ON [acc].[RoleEntityTypeAccess] ([RoleId] ASC) INCLUDE ([EntityTypeId], [AccessTypeId])
GO