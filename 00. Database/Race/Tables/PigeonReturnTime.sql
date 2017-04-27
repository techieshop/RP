CREATE TABLE [race].[PigeonReturnTime]
(
	[Id] INT NOT NULL IDENTITY,
	[RaceId] INT NOT NULL,
	[PigeonId] INT NOT NULL,
	[ReturnTime] DATETIME NOT NULL,
	CONSTRAINT [PK_PigeonReturnTime] PRIMARY KEY CLUSTERED ([Id]),
	CONSTRAINT [FK_PigeonReturnTime_Race] FOREIGN KEY ([RaceId]) REFERENCES [race].[Race] ([Id]),
	CONSTRAINT [FK_PigeonReturnTime_Pigeon] FOREIGN KEY ([PigeonId]) REFERENCES [acc].[Pigeon] ([Id])
)
