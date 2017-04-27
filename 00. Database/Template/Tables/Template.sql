CREATE TABLE [stl].[Template]
(
	[Id] INT NOT NULL,
	[Name] NVARCHAR(128) NOT NULL,
	[Description] NVARCHAR(512),
	[TitleCode] INT NOT NULL,
	[ContentCode] INT NOT NULL,
	[EntityInfoId] INT NOT NULL,
	CONSTRAINT [PK_Template] PRIMARY KEY CLUSTERED ([Id]),
	CONSTRAINT [FK_Template_EntityInfo] FOREIGN KEY ([EntityInfoId]) REFERENCES [plt].[EntityInfo] ([Id])
)