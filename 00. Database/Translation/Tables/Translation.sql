CREATE TABLE [stl].[Translation]
(
	[Id] INT NOT NULL IDENTITY,
	[LanguageId] INT NOT NULL,
	[TranslationCodeId] INT NOT NULL,
	[Value] NVARCHAR(MAX) NOT NULL,
	CONSTRAINT [PK_Translation] PRIMARY KEY CLUSTERED ([Id]),
	CONSTRAINT [FK_Translation_Language] FOREIGN KEY ([LanguageId]) REFERENCES [dom].[DomainValue] ([Id]),
	CONSTRAINT [FK_Translation_TranslationCode] FOREIGN KEY ([TranslationCodeId]) REFERENCES [stl].[TranslationCode] ([Id])
)
