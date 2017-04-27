CREATE TABLE [acc].[User]
(
	[Id] INT NOT NULL IDENTITY,
	[Email] NVARCHAR(128) NOT NULL,
	[Password] NVARCHAR(64) NOT NULL,
	[LastName] NVARCHAR(32) NOT NULL,
	[FirstName] NVARCHAR(32) NOT NULL,
	[MiddleName] NVARCHAR(32),
	[DateOfBirth] DATE,
	[Phone] NVARCHAR(32),
	[Mobile] NVARCHAR(32),
	[Salutation] NVARCHAR(32),
	[HasPhoto] BIT NOT NULL CONSTRAINT [DF_User_HasPhoto] DEFAULT 0,
	[AddressId] INT,
	[GenderId] INT,
	[LanguageId] INT NOT NULL,
	[EntityInfoId] INT NOT NULL,
	CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([Id]),
	CONSTRAINT [FK_User_Address] FOREIGN KEY ([AddressId]) REFERENCES [acc].[Address] ([Id]),
	CONSTRAINT [FK_User_Gender] FOREIGN KEY ([GenderId]) REFERENCES [dom].[DomainValue] ([Id]),
	CONSTRAINT [FK_User_Language] FOREIGN KEY ([LanguageId]) REFERENCES [dom].[DomainValue] ([Id]),
	CONSTRAINT [FK_User_EntityInfo] FOREIGN KEY ([EntityInfoId]) REFERENCES [plt].[EntityInfo] ([Id])
)
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_User_EntityInfoId] ON [acc].[User] ([EntityInfoId] ASC)
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_User_Email] ON [acc].[User] ([Email] ASC) WHERE [Email] IS NOT NULL