CREATE TYPE [udt].[EntityAccess] AS TABLE (
	[EntityInfoId] INT NOT NULL,
	[OrganizationId] INT NOT NULL,
	[EntityTypeId] INT NOT NULL,
	[EntityStateId] INT NOT NULL,
	[AccessTypeId] INT NOT NULL
);