CREATE TABLE [race].[Point]
(
	[Id] INT NOT NULL IDENTITY,
	[Name] NVARCHAR(128) NOT NULL,
	[AddressId] INT NOT NULL,
	[EntityInfoId] INT NOT NULL,
	CONSTRAINT [PK_Point] PRIMARY KEY CLUSTERED ([Id]),
	CONSTRAINT [FK_Point_Address] FOREIGN KEY ([AddressId]) REFERENCES [acc].[Address] ([Id]),
	CONSTRAINT [FK_Point_EntityInfo] FOREIGN KEY ([EntityInfoId]) REFERENCES [plt].[EntityInfo] ([Id])
)
