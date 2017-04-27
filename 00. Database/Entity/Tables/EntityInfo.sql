CREATE TABLE [plt].[EntityInfo]
(
	[Id] INT NOT NULL IDENTITY,
	[Guid] UNIQUEIDENTIFIER NOT NULL,
	[CreatedDate] DATETIME NOT NULL,
	[EntityTypeId] INT NOT NULL,
	CONSTRAINT [PK_EntityInfo] PRIMARY KEY CLUSTERED ([Id]),
	CONSTRAINT [FK_EntityInfo_EntityType] FOREIGN KEY ([EntityTypeId]) REFERENCES [plt].[EntityType] ([Id])
)
GO

CREATE NONCLUSTERED INDEX [IX_EntityInfo_EntityTypeId] ON [plt].[EntityInfo] ([EntityTypeId] ASC)
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_EntityInfo_Guid] ON [plt].[EntityInfo] ([Guid] ASC) WHERE [Guid] IS NOT NULL