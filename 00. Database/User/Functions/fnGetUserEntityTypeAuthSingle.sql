CREATE FUNCTION [acc].[fnGetUserEntityTypeAuthSingle] (
	@UserId INT,
	@ContextOrganizationId INT,
	@EntityTypeId INT,
	@EntityStateIds [udt].[KeyList] READONLY,
	@IsActiveOnly BIT = 0
) RETURNS @UserAccess TABLE (
	[OrganizationId] INT,
	[EntityTypeId] INT,
	[EntityStateId] INT,
	[AccessTypeId] INT
)
AS
BEGIN
	DECLARE @IsActiveOnlyEntityStateId INT = 0;

	DECLARE @EntityState [udt].[EntityState];
	IF @IsActiveOnly = 1
		INSERT INTO @EntityState VALUES (@EntityTypeId, @IsActiveOnlyEntityStateId)
	ELSE IF EXISTS(SELECT * FROM @EntityStateIds)
		INSERT INTO @EntityState SELECT @EntityTypeId, [Id] FROM @EntityStateIds
	ELSE
		INSERT INTO @EntityState VALUES (@EntityTypeId, NULL)

	INSERT INTO @UserAccess
	SELECT * FROM [acc].[fnGetUserEntityTypeAuth](@UserId, @ContextOrganizationId, @EntityState);

	RETURN;
END