CREATE TABLE [race].[Season]
(
	[Id] INT NOT NULL IDENTITY,
	[SeasonTypeId] INT NOT NULL,
	[Year] INT NOT NULL,
	[EntityInfoId] INT NOT NULL,
	CONSTRAINT [PK_Season] PRIMARY KEY CLUSTERED ([Id]),
	CONSTRAINT [FK_Season_SeasonTypeId] FOREIGN KEY ([SeasonTypeId]) REFERENCES [dom].[DomainValue] ([Id]),
	CONSTRAINT [FK_Season_EntityInfo] FOREIGN KEY ([EntityInfoId]) REFERENCES [plt].[EntityInfo] ([Id])
)
