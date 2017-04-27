CREATE TABLE [race].[Statement]
(
	[Id] INT NOT NULL IDENTITY,
	[PigeonId] INT NOT NULL,
	[SeasonId] INT NOT NULL,
	CONSTRAINT [PK_Statement] PRIMARY KEY CLUSTERED ([Id]),
	CONSTRAINT [FK_Statement_Pigeon] FOREIGN KEY ([PigeonId]) REFERENCES [acc].[Pigeon] ([Id]),
	CONSTRAINT [FK_Statement_Season] FOREIGN KEY ([SeasonId]) REFERENCES [race].[Season] ([Id])
)
