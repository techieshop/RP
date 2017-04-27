CREATE TABLE [plt].[EntityStateTransition]
(
	[Id] INT NOT NULL,
	[ActionBeforeCode] INT,
	[ActionAfterCode] INT NOT NULL,
	[Order] TINYINT NOT NULL,
	[EntityStateFromId] INT,
	[EntityStateToId] INT NOT NULL,
	CONSTRAINT [PK_EntityStateTransition] PRIMARY KEY CLUSTERED ([Id]),
	CONSTRAINT [FK_EntityStateTransition_EntityState_From] FOREIGN KEY ([EntityStateFromId]) REFERENCES [plt].[EntityState] ([Id]),
	CONSTRAINT [FK_EntityStateTransition_EntityState_To] FOREIGN KEY ([EntityStateToId]) REFERENCES [plt].[EntityState] ([Id])
)
