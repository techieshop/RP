CREATE PROCEDURE [acc].[spGetUserDetails]
	@UserId INT,
	@ContextOrganizationId INT,
	@UId INT
AS
BEGIN
	DECLARE @UserEntityTypeAccess [udt].[EntityAccess];
	INSERT INTO @UserEntityTypeAccess
	EXEC [acc].[spGetUserEntityTypeAccess] @UserId, @ContextOrganizationId;

	DECLARE @UserEntityInfoId INT;
	DECLARE @AccessTypeId INT;
	SELECT
		@UserEntityInfoId = [U].[EntityInfoId],
		@AccessTypeId = [ETA].[AccessTypeId]
	FROM [acc].[User] [U] WITH(NOLOCK)
		INNER JOIN @UserEntityTypeAccess [ETA] ON [ETA].[EntityInfoId] = [U].[EntityInfoId]
	WHERE [U].[Id] = @UId;

	IF (@UserEntityInfoId IS NOT NULL AND @UserEntityInfoId > 0)
	BEGIN
		SELECT
			[U].[Id],
			[U].[EntityInfoId],
			@AccessTypeId AS [AccessTypeId],
			[EO].[EntityTypeId],
			[EO].[EntityStateId],
			[EO].[OrganizationId],
			[U].[Email],
			[U].[Password],
			[U].[FirstName],
			[U].[MiddleName],
			[U].[LastName],
			[U].[DateOfBirth],
			[U].[GenderId],
			[U].[LanguageId],
			[U].[Phone],
			[U].[Mobile],
			[U].[Salutation]
		FROM [acc].[User] [U] WITH(NOLOCK)
			INNER JOIN [plt].[EntityOrganization] [EO] WITH(NOLOCK) ON [EO].[EntityInfoId] = [U].[EntityInfoId]
		WHERE [U].[Id] = @UId;
	
		SELECT
			[A].[Id],
			[A].[City],
			[A].[PostalCode],
			[A].[Street],
			[A].[Number],
			[A].[FormattedAddress],
			[A].[Latitude],
			[A].[Longitude]
		FROM [acc].[User] [U] WITH(NOLOCK)
			INNER JOIN [acc].[Address] [A] WITH(NOLOCK) ON [A].[Id] = [U].[AddressId]
		WHERE [U].[Id] = @UId;
	
		SELECT
			[UR].[Id],
			[UR].[OrganizationId],
			[UR].[RoleId],
			[UR].[UserId],
			[R].[Name]
		FROM [acc].[UserRole] [UR] WITH(NOLOCK)
			INNER JOIN [acc].[Role] [R] WITH(NOLOCK) ON [R].[Id] = [UR].[RoleId]
		WHERE [UR].[UserId] = @UId;
	END
END;
