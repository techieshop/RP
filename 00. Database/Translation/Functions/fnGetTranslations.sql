CREATE FUNCTION [stl].[fnGetTranslations] (
	@LanguageId INT,
	@TranslationCodes [udt].[KeyList] READONLY
)
RETURNS @Translations TABLE (
	[TranslationCodeId] INT NOT NULL,
	[Value] NVARCHAR(MAX) NOT NULL
)
AS
BEGIN

	DECLARE @DefaultSystemLanguageId INT = 8; -- 7 ENGLISH, 8 - UKRAINIAN
	DECLARE @HasTranslationCodes INT;

	IF EXISTS(SELECT * FROM @TranslationCodes)
		SET @HasTranslationCodes = 1;
	ELSE
		SET @HasTranslationCodes = 0;

	WITH [Translations] AS (
		SELECT
			[T].[TranslationCodeId],
			[T].[Value]
		FROM [stl].[Translation] [T] WITH(NOLOCK)
		WHERE (@HasTranslationCodes = 0 OR ([T].[TranslationCodeId] IN (SELECT [Id] FROM @TranslationCodes))) AND LanguageId = @LanguageId
	)
	INSERT INTO @Translations
	SELECT
		[T].[TranslationCodeId],
		[T].[Value]
	FROM [Translations] [T]

	RETURN;
END