CREATE TABLE [acc].[Member]
(
	[Id] INT NOT NULL IDENTITY,
	[Email] NVARCHAR(128),
	[LastName] NVARCHAR(32) NOT NULL,
	[FirstName] NVARCHAR(32) NOT NULL,
	[MiddleName] NVARCHAR(32),
	[Phone] NVARCHAR(32),
	[Mobile] NVARCHAR(32),
	[DateOfBirth] DATETIME,
	[AddressId] INT,
	[GenderId] INT,
	[WebsiteId] INT,
	[EntityInfoId] INT NOT NULL,
	CONSTRAINT [PK_Member] PRIMARY KEY CLUSTERED ([Id]),
	CONSTRAINT [FK_Member_Address] FOREIGN KEY ([AddressId]) REFERENCES [acc].[Address] ([Id]),
	CONSTRAINT [FK_Member_Gender] FOREIGN KEY ([GenderId]) REFERENCES [dom].[DomainValue] ([Id]),
	CONSTRAINT [FK_Member_Website] FOREIGN KEY ([WebsiteId]) REFERENCES [acc].[Website] ([Id]),
	CONSTRAINT [FK_Member_EntityInfo] FOREIGN KEY ([EntityInfoId]) REFERENCES [plt].[EntityInfo] ([Id])
)
