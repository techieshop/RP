﻿CREATE TABLE [acc].[Registration]
(
	[Id] INT NOT NULL IDENTITY,
	[DateTimeRequest] DATETIME NOT NULL,
	[IsRead] BIT NOT NULL CONSTRAINT [DF_Registration_IsRead] DEFAULT 0,
	[OrganizationName] NVARCHAR(256) NOT NULL,
	[CandidateInfo] NVARCHAR(2048) NOT NULL,
	[HeadInfo] NVARCHAR(2048) NOT NULL,
	CONSTRAINT [PK_Registration] PRIMARY KEY CLUSTERED ([Id])
)
