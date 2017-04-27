CREATE TABLE [acc].[RoleEntityStateAccess]
(
	[Id] INT NOT NULL IDENTITY,
	[RoleId] INT NOT NULL,
	[EntityTypeId] INT NOT NULL,
	[EntityStateId] INT NOT NULL,
	[AccessTypeId] INT NOT NULL,
	CONSTRAINT [PK_RoleEntityStateAccess] PRIMARY KEY CLUSTERED ([Id]),
	CONSTRAINT [FK_RoleEntityStateAccess_Role] FOREIGN KEY ([RoleId]) REFERENCES [acc].[Role] ([Id]),
	CONSTRAINT [FK_RoleEntityStateAccess_EntityType] FOREIGN KEY ([EntityTypeId]) REFERENCES [plt].[EntityType] ([Id]),
	CONSTRAINT [FK_RoleEntityStateAccess_EntityState] FOREIGN KEY ([EntityStateId]) REFERENCES [plt].[EntityState] ([Id])
)
GO

CREATE NONCLUSTERED INDEX [IX_RoleEntityStateAccess_RoleId] ON [acc].[RoleEntityStateAccess] ([RoleId] ASC) INCLUDE ([EntityTypeId], [EntityStateId], [AccessTypeId])
GO
