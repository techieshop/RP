SET IDENTITY_INSERT [plt].[EntityInfo] ON
INSERT INTO [plt].[EntityInfo] ([Id], [Guid], [CreatedDate], [EntityTypeId]) VALUES 
(1, NEWID(), GETDATE(), 8), -- Role - RP Admin Role
(2, NEWID(), GETDATE(), 8), -- Role > Manager

(6, NEWID(), GETDATE(), 1), -- Main User
(7, NEWID(), GETDATE(), 2) -- Main Organization
SET IDENTITY_INSERT [plt].[EntityInfo] OFF