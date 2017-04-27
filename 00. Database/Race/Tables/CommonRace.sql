CREATE TABLE [race].[CommonRace]
(
	[Id] INT NOT NULL IDENTITY,
	[CommonRaceId] INT NOT NULL,
	[RaceId] INT NOT NULL,
	CONSTRAINT [PK_CommonRace] PRIMARY KEY CLUSTERED ([Id]),
	CONSTRAINT [FK_CommonRace_CommonRace] FOREIGN KEY ([CommonRaceId]) REFERENCES [race].[Race] ([Id]),
	CONSTRAINT [FK_CommonRace_Race] FOREIGN KEY ([RaceId]) REFERENCES [race].[Race] ([Id])
)
