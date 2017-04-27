CREATE TABLE [acc].[OrganizationMemberType]
(
	[Id] INT NOT NULL IDENTITY,
	[OrganizationId] INT NOT NULL,
	[MemberId] INT NOT NULL,
	[MemberTypeId] INT NOT NULL,
	CONSTRAINT [PK_OrganizationMemberType] PRIMARY KEY CLUSTERED ([Id]),
	CONSTRAINT [FK_OrganizationMemberType_Organization] FOREIGN KEY ([OrganizationId]) REFERENCES [acc].[Organization] ([Id]),
	CONSTRAINT [FK_OrganizationMemberType_Member] FOREIGN KEY ([MemberId]) REFERENCES [acc].[Member] ([Id]),
	CONSTRAINT [FK_OrganizationMemberType_MemberType] FOREIGN KEY ([MemberTypeId]) REFERENCES [dom].[DomainValue] ([Id])
)
