CREATE TABLE [acc].[Organization]
(
	[Id] INT NOT NULL IDENTITY,
	[Name] NVARCHAR(256) NOT NULL,
	[Description] NVARCHAR(MAX),
	[CreateDate] DATE,
	[AddressId] INT,
	[OrganizationTypeId] INT,
	[WebsiteId] INT,
	[EntityInfoId] INT NOT NULL,
	CONSTRAINT [PK_Organization] PRIMARY KEY CLUSTERED ([Id]),
	CONSTRAINT [FK_Organization_Address] FOREIGN KEY ([AddressId]) REFERENCES [acc].[Address] ([Id]),
	CONSTRAINT [FK_Organization_Website] FOREIGN KEY ([WebsiteId]) REFERENCES [acc].[Website] ([Id]),
	CONSTRAINT [FK_Organization_EntityInfo] FOREIGN KEY ([EntityInfoId]) REFERENCES [plt].[EntityInfo] ([Id])
)
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_Organization_EntityInfoId] ON [acc].[Organization] ([EntityInfoId] ASC)
