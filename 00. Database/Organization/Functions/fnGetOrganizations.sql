CREATE FUNCTION [acc].[fnGetOrganizations] (
	@UserId INT,
	@ContextOrganizationId INT
)
RETURNS @UserOrganization TABLE (
	[OrganizationId] INT
)
AS
BEGIN

	INSERT INTO @UserOrganization
	SELECT DISTINCT [OrganizationId] FROM [acc].[fnGetUserRole] (@UserId, @ContextOrganizationId);

	RETURN;
END