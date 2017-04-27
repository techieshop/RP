CREATE PROCEDURE [acc].[spGetOrganizationDetails]
	@UserId INT,
	@ContextOrganizationId INT,
	@OrganizationId INT
AS
BEGIN
	DECLARE @OrganizationEntityTypeAccess [udt].[EntityAccess];
	INSERT INTO @OrganizationEntityTypeAccess
	EXEC [acc].[spGetOrganizationEntityTypeAccess] @UserId, @ContextOrganizationId;

	IF EXISTS(SELECT * FROM @OrganizationEntityTypeAccess [ETA] WHERE [ETA].[OrganizationId] = @OrganizationId)
	BEGIN
		DECLARE @AccessTypeId INT;
		SELECT
			@AccessTypeId = [ETA].[AccessTypeId]
		FROM @OrganizationEntityTypeAccess [ETA] WHERE [ETA].[OrganizationId] = @OrganizationId

		DECLARE @MemberEntityTypeAccess [udt].[EntityAccess];
		INSERT INTO @MemberEntityTypeAccess
		EXEC [acc].[spGetMemberEntityTypeAccess] @UserId, @ContextOrganizationId;

		DECLARE @PigeonEntityTypeAccess [udt].[EntityAccess];
		INSERT INTO @PigeonEntityTypeAccess
		EXEC [acc].[spGetPigeonEntityTypeAccess] @UserId, @ContextOrganizationId;

		SELECT
			[O].[Id],
			[O].[EntityInfoId],
			@AccessTypeId AS [AccessTypeId],
			[EO].[EntityTypeId],
			[EO].[EntityStateId],
			[EO].[OrganizationId],
			[O].[Name],
			[O].[Description],
			[O].[CreateDate],
			(SELECT
				COUNT(DISTINCT [M].[Id])
			FROM [acc].[Member] [M] WITH(NOLOCK)
				INNER JOIN [plt].[EntityOrganization] [EO] WITH(NOLOCK) ON [EO].[EntityInfoId] = [M].[EntityInfoId]
				INNER JOIN @MemberEntityTypeAccess [META] ON
					[META].[EntityInfoId] = [EO].[EntityInfoId] AND
					[META].[OrganizationId] = [EO].[OrganizationId] AND
					[META].[EntityStateId] = [EO].[EntityStateId] AND
					[META].[EntityTypeId] = [EO].[EntityTypeId]
			WHERE [EO].[OrganizationId] = [O].[Id]
			) AS [MemberCount]
		FROM [acc].[Organization] [O] WITH(NOLOCK)
			INNER JOIN [plt].[EntityOrganization] [EO] WITH(NOLOCK) ON [EO].[EntityInfoId] = [O].[EntityInfoId]
		WHERE [O].[Id] = @OrganizationId;

		SELECT
			[A].[Id],
			[A].[City],
			[A].[PostalCode],
			[A].[Street],
			[A].[Number],
			[A].[FormattedAddress],
			[A].[Latitude],
			[A].[Longitude]
		FROM [acc].[Organization] [O] WITH(NOLOCK)
			INNER JOIN [acc].[Address] [A] WITH(NOLOCK) ON [A].[Id] = [O].[AddressId]
		WHERE [O].[Id] = @OrganizationId;

		SELECT
			[W].[Id],
			[W].[Url],
			[W].[Description]
		FROM [acc].[Organization] [O] WITH(NOLOCK)
			INNER JOIN [acc].[Website] [W] WITH(NOLOCK) ON [W].[Id] = [O].[WebsiteId]
		WHERE [O].[Id] = @OrganizationId;

		DECLARE @MemberIds AS TABLE(
			[Id] INT NOT NULL
		);

		INSERT INTO @MemberIds
		SELECT
			[M].[Id]
		FROM [acc].[Member] [M] WITH(NOLOCK)
			INNER JOIN [plt].[EntityOrganization] [EO] WITH(NOLOCK) ON [EO].[EntityInfoId] = [M].[EntityInfoId]
			INNER JOIN @MemberEntityTypeAccess [META] ON
				[META].[EntityInfoId] = [EO].[EntityInfoId] AND
				[META].[OrganizationId] = [EO].[OrganizationId] AND
				[META].[EntityStateId] = [EO].[EntityStateId] AND
				[META].[EntityTypeId] = [EO].[EntityTypeId]
		WHERE [EO].[OrganizationId] = @OrganizationId;

		SELECT
			[M].[Id],
			[M].[FirstName],
			[M].[LastName],
			[M].[MiddleName]
		FROM [acc].[Member] [M] WITH(NOLOCK)
		WHERE [M].[Id] IN (SELECT * FROM @MemberIds);

		SELECT
			[OMT].[OrganizationId],
			[OMT].[MemberId],
			[OMT].[MemberTypeId],
			[O].[Name] AS [OrganizationName],
			[M].[FirstName],
			[M].[LastName],
			[M].[MiddleName]
		FROM [acc].[Member] [M] WITH(NOLOCK)
			INNER JOIN [acc].[OrganizationMemberType] [OMT] WITH(NOLOCK) ON [OMT].[MemberId] = [M].[Id] AND [OMT].[OrganizationId] = @OrganizationId
			INNER JOIN [acc].[Organization] [O] WITH(NOLOCK) ON [O].[Id] = [OMT].[OrganizationId]
		WHERE [M].[Id] IN (SELECT * FROM @MemberIds);
	END;
END;
