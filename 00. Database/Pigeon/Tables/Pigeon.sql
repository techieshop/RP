CREATE TABLE [acc].[Pigeon]
(
	[Id] INT NOT NULL IDENTITY,
	[Name] NVARCHAR(32),
	[Year] INT NOT NULL,
	[Code] NVARCHAR(16),
	[Number] NVARCHAR(16) NOT NULL,
	[SexId] INT,
	[MemberId] INT NOT NULL,
	[EntityInfoId] INT NOT NULL,
	CONSTRAINT [PK_Pigeon] PRIMARY KEY CLUSTERED ([Id]),
	CONSTRAINT [FK_Pigeon_Sex] FOREIGN KEY ([SexId]) REFERENCES [dom].[DomainValue] ([Id]),
	CONSTRAINT [FK_Pigeon_Member] FOREIGN KEY ([MemberId]) REFERENCES [acc].[Member] ([Id]),
	CONSTRAINT [FK_Pigeon_EntityInfo] FOREIGN KEY ([EntityInfoId]) REFERENCES [plt].[EntityInfo] ([Id]),
)
