CREATE TABLE [acc].[UserRole]
(
	[Id] INT NOT NULL IDENTITY,
	[UserId] INT NOT NULL,
	[RoleId] INT NOT NULL,
	[OrganizationId] INT NOT NULL,
	CONSTRAINT [PK_UserRole] PRIMARY KEY CLUSTERED ([Id]),
	CONSTRAINT [FK_UserRole_User] FOREIGN KEY ([UserId]) REFERENCES [acc].[User] ([Id]),
	CONSTRAINT [FK_UserRole_Role] FOREIGN KEY ([RoleId]) REFERENCES [acc].[Role] ([Id]),
	CONSTRAINT [FK_UserRole_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [acc].[Organization] ([Id])
)
