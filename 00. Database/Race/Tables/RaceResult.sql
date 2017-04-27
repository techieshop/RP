CREATE TABLE [race].[RaceResult]
(
	[Id] INT NOT NULL IDENTITY,
	[RaceId] INT NOT NULL,
	[PigeonId] INT NOT NULL,
	[FlyTime] FLOAT NOT NULL,
	[Distance] FLOAT NOT NULL,
	[Speed] FLOAT  NOT NULL,
	[Position] INT NOT NULL,
	[Ac] BIT NOT NULL,
	CONSTRAINT [PK_RaceResult] PRIMARY KEY CLUSTERED ([Id]),
	CONSTRAINT [FK_RaceResult_Race] FOREIGN KEY ([RaceId]) REFERENCES [race].[Race] ([Id]),
	CONSTRAINT [FK_RaceResult_Pigeon] FOREIGN KEY ([PigeonId]) REFERENCES [acc].[Pigeon] ([Id])
)
