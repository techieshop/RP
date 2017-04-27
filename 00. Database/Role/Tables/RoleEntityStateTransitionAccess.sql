CREATE TABLE [acc].[RoleEntityStateTransitionAccess]
(
	[Id] INT NOT NULL IDENTITY,
	[RoleId] INT NOT NULL,
	[EntityTypeId] INT NOT NULL,
	[EntityStateTransitionId] INT NOT NULL,
	CONSTRAINT [PK_RoleEntityStateTransitionAccess] PRIMARY KEY CLUSTERED ([Id]),
	CONSTRAINT [FK_RoleEntityStateTransitionAccess_RoleEntityTypeAccess] FOREIGN KEY ([RoleId]) REFERENCES [acc].[Role] ([Id]),
	CONSTRAINT [FK_RoleEntityStateTransitionAccess_EntityType] FOREIGN KEY ([EntityTypeId]) REFERENCES [plt].[EntityType] ([Id]),
	CONSTRAINT [FK_RoleEntityStateTransitionAccess_EntityTransition] FOREIGN KEY ([EntityStateTransitionId]) REFERENCES [plt].[EntityStateTransition] ([Id])
)
GO

CREATE NONCLUSTERED INDEX [IX_RoleEntityStateTransitionAccess_RoleId]
ON [acc].[RoleEntityStateTransitionAccess] ([RoleId])
INCLUDE ([EntityTypeId],[EntityStateTransitionId])
GO