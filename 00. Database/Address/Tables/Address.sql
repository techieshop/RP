CREATE TABLE [acc].[Address]
(
	[Id] INT NOT NULL IDENTITY,
	[CountryId] INT NOT NULL,
	[City] NVARCHAR(128),
	[PostalCode] NVARCHAR(16),
	[Street] NVARCHAR(128),
	[Number] NVARCHAR(16),
	[Latitude] FLOAT,
	[Longitude] FLOAT,
	[FormattedAddress] NVARCHAR(MAX) NULL,
	CONSTRAINT [PK_Address] PRIMARY KEY CLUSTERED ([Id]),
	CONSTRAINT [FK_Address_Country] FOREIGN KEY ([CountryId]) REFERENCES [dom].[Country] ([Id])
)
