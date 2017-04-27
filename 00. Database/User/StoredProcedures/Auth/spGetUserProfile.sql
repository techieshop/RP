CREATE PROCEDURE [acc].[spGetUserProfile]
	@UserId INT
AS
BEGIN
	SELECT
		[U].[Id],
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
	WHERE [U].[Id] = @UserId;

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
	WHERE [U].[Id] = @UserId;
END;
