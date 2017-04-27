CREATE PROCEDURE [acc].[spGetMemberDetails]
	@UserId INT,
	@ContextOrganizationId INT,
	@MemberId INT
AS
BEGIN
	DECLARE @MemberEntityTypeAccess [udt].[EntityAccess];
	INSERT INTO @MemberEntityTypeAccess
	EXEC [acc].[spGetMemberEntityTypeAccess] @UserId, @ContextOrganizationId;

	DECLARE @MemberEntityInfoId INT;
	DECLARE @AccessTypeId INT;
	SELECT 
		@MemberEntityInfoId = [M].[EntityInfoId],
		@AccessTypeId = [ETA].[AccessTypeId]
	FROM [acc].[Member] [M] WITH(NOLOCK)
		INNER JOIN @MemberEntityTypeAccess [ETA] ON [ETA].[EntityInfoId] = [M].[EntityInfoId]
	WHERE [M].[Id] = @MemberId;

	IF (@MemberEntityInfoId IS NOT NULL AND @MemberEntityInfoId > 0)
	BEGIN
		DECLARE @UserOrganizations [udt].[KeyList];
		INSERT INTO @UserOrganizations
		SELECT [OrganizationId] FROM [acc].[fnGetOrganizations] (@UserId, @ContextOrganizationId);

		SELECT
			[M].[Id],
			[M].[EntityInfoId],
			@AccessTypeId AS [AccessTypeId],
			[EO].[EntityTypeId],
			[EO].[EntityStateId],
			[EO].[OrganizationId],
			[acc].[fnGetOrganizationName]([EO].[OrganizationId], @UserOrganizations) AS [OrganizationName],
			[M].[FirstName],
			[M].[MiddleName],
			[M].[LastName],
			[M].[Email],
			[M].[Phone],
			[M].[Mobile],
			[M].[DateOfBirth],
			[M].[GenderId],
			(SELECT
				COUNT([P].[Id])
			FROM [acc].[Pigeon] [P] WITH(NOLOCK)
			WHERE [P].[MemberId] = @MemberId
			) AS [PigeonCount]
		FROM [acc].[Member] [M] WITH(NOLOCK)
			INNER JOIN [plt].[EntityOrganization] [EO] WITH(NOLOCK) ON [EO].[EntityInfoId] = [M].[EntityInfoId]
		WHERE [M].[Id] = @MemberId;

		SELECT
			[A].[Id],
			[A].[City],
			[A].[PostalCode],
			[A].[Street],
			[A].[Number],
			[A].[FormattedAddress],
			[A].[Latitude],
			[A].[Longitude]
		FROM [acc].[Member] [M] WITH(NOLOCK)
			INNER JOIN [acc].[Address] [A] WITH(NOLOCK) ON [A].[Id] = [M].[AddressId]
		WHERE [M].[Id] = @MemberId;

		SELECT
			[W].[Id],
			[W].[Url],
			[W].[Description]
		FROM [acc].[Member] [M] WITH(NOLOCK)
			INNER JOIN [acc].[Website] [W] WITH(NOLOCK) ON [W].[Id] = [M].[WebsiteId]
		WHERE [M].[Id] = @MemberId;

		SELECT
			[OMT].[OrganizationId],
			[OMT].[MemberId],
			[OMT].[MemberTypeId],
			[O].[Name] AS [OrganizationName],
			[M].[FirstName],
			[M].[LastName],
			[M].[MiddleName]
		FROM [acc].[Member] [M] WITH(NOLOCK)
			INNER JOIN [acc].[OrganizationMemberType] [OMT] WITH(NOLOCK) ON [OMT].[MemberId] = [M].[Id] 
			INNER JOIN [acc].[Organization] [O] WITH(NOLOCK) ON [O].[Id] = [OMT].[OrganizationId]
		WHERE [M].[Id] = @MemberId;
	END;
END;
