CREATE PROCEDURE [acc].[spGetMemberItems]
	@UserId INT,
	@ContextOrganizationId INT,
	@EntityStateIds [udt].[KeyList] READONLY,
	@Search NVARCHAR(MAX),
	@Skip INT,
	@Take INT
AS
BEGIN
	DECLARE @MemberEntityTypeAccess [udt].[EntityAccess];
	INSERT INTO @MemberEntityTypeAccess
	EXEC [acc].[spGetMemberEntityTypeAccess] @UserId, @ContextOrganizationId, @EntityStateIds;

	DECLARE @SearchRegEx NVARCHAR(MAX) = '';
	DECLARE @JoinClause NVARCHAR(MAX) = '' ;
	DECLARE @WhereClause NVARCHAR(MAX) = '';
	DECLARE @OrderClause NVARCHAR(MAX) = '';

	EXEC [acc].[spInitMemberFilter]
		@SearchRegEx OUTPUT,
		@JoinClause OUTPUT,
		@WhereClause OUTPUT,
		@OrderClause OUTPUT,
		@EntityStateIds,
		@Search;

	DECLARE @Sql NVARCHAR(MAX) = N'
		DECLARE @OrganizationEntityTypeAccess [udt].[EntityAccess];
		INSERT INTO @OrganizationEntityTypeAccess
		EXEC [acc].[spGetOrganizationEntityTypeAccess] @UserId, @ContextOrganizationId;

		DECLARE @PigeonEntityTypeAccess [udt].[EntityAccess];
		INSERT INTO @PigeonEntityTypeAccess
		EXEC [acc].[spGetPigeonEntityTypeAccess] @UserId, @ContextOrganizationId;

		DECLARE @UserOrganizations [udt].[KeyList];
		INSERT INTO @UserOrganizations
		SELECT [OrganizationId] FROM [acc].[fnGetOrganizations] (@UserId, @ContextOrganizationId);

		DECLARE @Filter AS TABLE (
			[Id] INT NOT NULL,
			[OrganizationId] INT NOT NULL,
			[RowNumber] INT NOT NULL
		);

		INSERT INTO @Filter
		SELECT
			[M].[Id],
			[ETA].[OrganizationId],
			ROW_NUMBER() OVER (ORDER BY ' + @OrderClause + N') AS [RowNumber]
		FROM [acc].[Member] [M] WITH(NOLOCK)
			INNER JOIN @MemberEntityTypeAccess [ETA] ON [ETA].[EntityInfoId] = [M].[EntityInfoId]'
			+ @JoinClause
		+ @WhereClause + N';

		SELECT
			[M].[Id],
			[M].[EntityInfoId],
			[acc].[fnGetOrganizationName]([EO].[OrganizationId], @UserOrganizations) AS [OrganizationName],
			[EO].[OrganizationId],
			[EO].[EntityStateId],
			[M].[FirstName],
			[M].[MiddleName],
			[M].[LastName],
			[M].[Phone],
			[M].[Mobile],
			[A].[FormattedAddress],
			(SELECT
				COUNT(DISTINCT [P].[Id])
			FROM [acc].[Pigeon] [P] WITH(NOLOCK)
				INNER JOIN [plt].[EntityOrganization] [EO] WITH(NOLOCK) ON [EO].[EntityInfoId] = [P].[EntityInfoId]
				INNER JOIN @PigeonEntityTypeAccess [PETA] ON
					[PETA].[EntityInfoId] = [EO].[EntityInfoId] AND
					[PETA].[OrganizationId] = [EO].[OrganizationId] AND
					[PETA].[EntityStateId] = [EO].[EntityStateId] AND
					[PETA].[EntityTypeId] = [EO].[EntityTypeId]
			WHERE [P].[MemberId] = [M].[Id]
			) AS [PigeonCount]
		FROM [acc].[Member] [M] WITH(NOLOCK)
			INNER JOIN @Filter [F] ON [F].[Id] = [M].[Id]
			INNER JOIN [plt].[EntityOrganization] [EO] WITH(NOLOCK) ON [EO].[EntityInfoId] = [M].[EntityInfoId] AND [EO].[OrganizationId] = [F].[OrganizationId]
			LEFT JOIN [acc].[Address] [A] WITH(NOLOCK) ON [A].[Id] = [M].[AddressId]
		WHERE [F].[RowNumber] > @Skip AND [F].[RowNumber] <= @Skip + @Take
		ORDER BY [F].[RowNumber];

		SELECT
			COUNT(DISTINCT [F].[Id]) AS [Count]
		FROM @Filter [F];

		SELECT
			COUNT(DISTINCT [O].[Id]) AS [OrganizationCount]
		FROM [acc].[Organization] [O] WITH(NOLOCK)
			INNER JOIN [plt].[EntityOrganization] [EO] WITH(NOLOCK) ON [EO].[EntityInfoId] = [O].[EntityInfoId]
			INNER JOIN @OrganizationEntityTypeAccess [OETA] ON
				[OETA].[EntityInfoId] = [EO].[EntityInfoId] AND
				[OETA].[OrganizationId] = [EO].[OrganizationId] AND
				[OETA].[EntityStateId] = [EO].[EntityStateId] AND
				[OETA].[EntityTypeId] = [EO].[EntityTypeId];

		SELECT
			COUNT(DISTINCT [P].[Id]) AS [PigeonCount]
		FROM [acc].[Pigeon] [P] WITH(NOLOCK)
			INNER JOIN [plt].[EntityOrganization] [EO] WITH(NOLOCK) ON [EO].[EntityInfoId] = [P].[EntityInfoId]
			INNER JOIN @PigeonEntityTypeAccess [PETA] ON
				[PETA].[EntityInfoId] = [EO].[EntityInfoId] AND
				[PETA].[OrganizationId] = [EO].[OrganizationId] AND
				[PETA].[EntityStateId] = [EO].[EntityStateId] AND
				[PETA].[EntityTypeId] = [EO].[EntityTypeId];
	';

	DECLARE @Params NVARCHAR(MAX) = N'
		@UserId INT,
		@ContextOrganizationId INT,
		@MemberEntityTypeAccess [udt].[EntityAccess] READONLY,
		@EntityStateIds [udt].[KeyList] READONLY,
		@SearchRegEx NVARCHAR(MAX),
		@Skip INT,
		@Take INT';

	EXECUTE sp_executesql @Sql, @Params,
		@UserId,
		@ContextOrganizationId,
		@MemberEntityTypeAccess,
		@EntityStateIds,
		@SearchRegEx,
		@Skip,
		@Take;
END;