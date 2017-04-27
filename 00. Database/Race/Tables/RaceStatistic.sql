CREATE TABLE [race].[RaceStatistic]
(
	[Id] INT NOT NULL IDENTITY,
	[RaceId] INT NOT NULL,
	[MemberId] INT NOT NULL,
	[StatedPigeonCount] INT NOT NULL,
	[PrizePigeonCount] INT NOT NULL,
	[Mark] FLOAT NOT NULL,
	[Success] FLOAT NOT NULL,
	CONSTRAINT [PK_RaceStatistic] PRIMARY KEY CLUSTERED ([Id]),
	CONSTRAINT [FK_RaceStatistic_Race] FOREIGN KEY ([RaceId]) REFERENCES [race].[Race] ([Id]),
	CONSTRAINT [FK_RaceStatistic_Member] FOREIGN KEY ([MemberId]) REFERENCES [acc].[Member] ([Id])
)
