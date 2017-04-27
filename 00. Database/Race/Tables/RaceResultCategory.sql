CREATE TABLE [race].[RaceResultCategory]
(
	[Id] INT NOT NULL IDENTITY,
	[RaceResultId] INT NOT NULL,
	[CategoryId] INT NOT NULL,
	[Coefficient] FLOAT NOT NULL,
	[Mark] FLOAT NOT NULL,
	[IsOlymp] BIT NOT NULL,
	CONSTRAINT [PK_RaceResultCategory] PRIMARY KEY CLUSTERED ([Id]),
	CONSTRAINT [FK_RaceResult_RaceResult] FOREIGN KEY ([RaceResultId]) REFERENCES [race].[RaceResult] ([Id]),
	CONSTRAINT [FK_RaceResult_Category] FOREIGN KEY ([CategoryId]) REFERENCES [dom].[DomainValue] ([Id])
)
