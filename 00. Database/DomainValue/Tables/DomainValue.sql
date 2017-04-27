CREATE TABLE [dom].[DomainValue]
(
	[Id] INT NOT NULL,
	[NameCode] INT NOT NULL,
	[DescriptionCode] INT,
	[Icon] NVARCHAR(256),
	[Code] NVARCHAR(2),
	[DomainValueTypeId] INT NOT NULL,
	CONSTRAINT [PK_DomainValue] PRIMARY KEY CLUSTERED ([Id]),
	CONSTRAINT [FK_DomainValue_DomainValueType] FOREIGN KEY ([DomainValueTypeId]) REFERENCES [dom].[DomainValueType] ([Id])
)
