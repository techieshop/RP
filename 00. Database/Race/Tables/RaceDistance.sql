CREATE TABLE [race].[RaceDistance]
(
	[Id] INT NOT NULL IDENTITY,
	[RaceId] INT NOT NULL,
	[MemberId] INT NOT NULL,
	[Distance] FLOAT NOT NULL,
	CONSTRAINT [PK_RaceDistance] PRIMARY KEY CLUSTERED ([Id]),
	CONSTRAINT [FK_RaceDistance_Race] FOREIGN KEY ([RaceId]) REFERENCES [race].[Race] ([Id]),
	CONSTRAINT [FK_RaceDistance_Member] FOREIGN KEY ([MemberId]) REFERENCES [acc].[Member] ([Id])
)
