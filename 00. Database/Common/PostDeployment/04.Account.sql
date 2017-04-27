SET IDENTITY_INSERT [acc].[Address] ON
INSERT INTO [acc].[Address] ([Id], [CountryId], [City], [PostalCode], [Street], [Number], [Latitude], [Longitude]) VALUES 
(1, 216, N'Чернівці', N'58000', NULL, NULL, 0, 0), -- User
(2, 216, N'Чернівці', N'58000', NULL, NULL, 0, 0) -- Organization
SET IDENTITY_INSERT [acc].[Address] OFF

SET IDENTITY_INSERT [acc].[Website] ON
INSERT INTO [acc].[Website] ([Id], [Url], [Description]) VALUES 
(1, N'www.rp.in.ua', N'Спортивні голуби України - Результати і загальна інформація')
SET IDENTITY_INSERT [acc].[Website] OFF

SET IDENTITY_INSERT [acc].[User] ON
INSERT INTO [acc].[User] ([Id], [Email], [Password], [FirstName], [MiddleName], [LastName], [DateOfBirth], [Phone], [Mobile], [Salutation], [AddressId], [GenderId], [LanguageId], [EntityInfoId]) VALUES
(1, N'andrij.sv1993@gmail.com', N'yg9soZcT0Q61DKexOadbyA==', N'Андрій', N'Володимирович', N'Слободян', '01-09-1993', NULL, '+380973068923', N'Головний по всьому', 1, 1, 8, 6)
SET IDENTITY_INSERT [acc].[User] OFF

SET IDENTITY_INSERT [acc].[Organization] ON
INSERT INTO [acc].[Organization] ([Id], [Name], [CreateDate], [AddressId], [WebsiteId], [EntityInfoId], [OrganizationTypeId]) VALUES 
(1, N'RP', '01-03-2015', 2, 1, 7, 0)
SET IDENTITY_INSERT [acc].[Organization] OFF

INSERT INTO [acc].[OrganizationRelation] ([OrganizationId], [RelatedOrganizationId], [Order], [Level]) VALUES
(1,1,1,1)