CREATE TABLE [acc].[Role]
(
	[Id] INT NOT NULL,
	[Name] NVARCHAR(256) NOT NULL,
	[Description] NVARCHAR(MAX),
	[EntityInfoId] INT NOT NULL,
	CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED ([Id]),
	CONSTRAINT [FK_Role_EntityInfo] FOREIGN KEY ([EntityInfoId]) REFERENCES [plt].[EntityInfo] ([Id])
)
