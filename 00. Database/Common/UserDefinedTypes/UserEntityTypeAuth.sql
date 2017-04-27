CREATE TYPE [udt].[UserEntityTypeAuth] AS TABLE (
	[OrganizationId] INT NOT NULL,
	[EntityTypeId] INT NOT NULL,
	[EntityStateId] INT NOT NULL,
	[AccessTypeId] INT NOT NULL
);