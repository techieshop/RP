CREATE TABLE [plt].[EntityState]
(
	[Id] INT NOT NULL,
	[NameCode] INT NOT NULL,
	[DescriptionCode] INT,
	[Order] TINYINT NOT NULL,
	[IsStart] BIT NOT NULL,
	[IsFinish] BIT NOT NULL,
	[IsActive] BIT NOT NULL,
	[EntityTypeId] INT NOT NULL,
	CONSTRAINT [PK_EntityState] PRIMARY KEY CLUSTERED ([Id]),
	CONSTRAINT [FK_EntityState_EntityType] FOREIGN KEY ([EntityTypeId]) REFERENCES [plt].[EntityType] ([Id])
)
