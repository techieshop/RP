CREATE PROCEDURE [stl].[spGetTranslations]
	@LanguageId INT,
	@TranslationCodes [udt].[KeyList] READONLY
AS
BEGIN
	SELECT * FROM [stl].[fnGetTranslations] (@LanguageId, @TranslationCodes);
END