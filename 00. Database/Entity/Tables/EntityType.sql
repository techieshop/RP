CREATE TABLE [plt].[EntityType]
(
	[Id] INT NOT NULL,
	[DescriptionCode] INT,
	[Name] VARCHAR(64) NOT NULL,
	CONSTRAINT [PK_EntityType] PRIMARY KEY CLUSTERED ([Id])
)