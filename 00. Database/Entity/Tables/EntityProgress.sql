CREATE TABLE [plt].[EntityProgress]
(
	[Id] INT NOT NULL IDENTITY,
	[EntityInfoId] INT NOT NULL,
	[DateTime] DATETIME NOT NULL,
	[OrganizationId] INT,
	[UserId] INT,
	[EntityStateBeforeId] INT,
	[EntityStateAfterId] INT NOT NULL,
	[Remarks] NVARCHAR(256),
	[IpAddress] VARCHAR(32),
	CONSTRAINT [PK_EntityProgress] PRIMARY KEY CLUSTERED ([Id]),
	CONSTRAINT [FK_EntityProgress_EntityInfo] FOREIGN KEY ([EntityInfoId]) REFERENCES [plt].[EntityInfo] ([Id]),
	CONSTRAINT [FK_EntityProgress_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [acc].[Organization] ([Id]),
	CONSTRAINT [FK_EntityProgress_User] FOREIGN KEY ([UserId]) REFERENCES [acc].[User] ([Id]),
	CONSTRAINT [FK_EntityProgress_EntityState_Before] FOREIGN KEY ([EntityStateBeforeId]) REFERENCES [plt].[EntityState] ([Id]),
	CONSTRAINT [FK_EntityProgress_EntityState_After] FOREIGN KEY ([EntityStateAfterId]) REFERENCES [plt].[EntityState] ([Id])
)
GO

CREATE NONCLUSTERED INDEX [IX_EntityProgress_EntityInfoId] ON [plt].[EntityProgress] ([EntityInfoId] ASC)