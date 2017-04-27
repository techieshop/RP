CREATE TABLE [race].[RacePigeon]
(
	[Id] INT NOT NULL IDENTITY,
	[RaceId] INT NOT NULL,
	[PigeonId] INT NOT NULL,
	CONSTRAINT [PK_RacePigeon] PRIMARY KEY CLUSTERED ([Id]),
	CONSTRAINT [FK_RacePigeon_Race] FOREIGN KEY ([RaceId]) REFERENCES [race].[Race] ([Id]),
	CONSTRAINT [FK_RacePigeon_Pigeon] FOREIGN KEY ([PigeonId]) REFERENCES [acc].[Pigeon] ([Id])
)
