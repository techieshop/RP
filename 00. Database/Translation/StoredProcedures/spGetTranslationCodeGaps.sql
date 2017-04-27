CREATE PROCEDURE [stl].[spGetTranslationCodesGaps]
	@MinGapSize INT = NULL
AS
	IF (@MinGapSize IS NULL)
	BEGIN
		SET @MinGapSize = 0;
	END;

	DECLARE @MaxId INT;
	DECLARE @IdFrom INT;
	DECLARE @IdTo INT;
	DECLARE @val AS TABLE ([Id] INT);
	DECLARE @res AS TABLE ([IdFrom] INT, [IdTo] INT, [Count] INT);

	SELECT @MaxId = MAX([Id])
	FROM [stl].[TranslationCode];

	SET @IdTo = 1;

	WHILE (@IdTo <= @MaxId)
	BEGIN
		INSERT INTO @val VALUES(@IdTo)
		SET @IdTo = @IdTo + 1;
	END

	SET @IdTo = 1;
	SET @IdFrom = 1;

	WHILE (@IdFrom <= @MaxId) 
	BEGIN
		SELECT @IdFrom = MIN(val.[Id])
		FROM @val [val]
			LEFT JOIN [stl].[TranslationCode] tc ON [tc].[Id] = [val].[Id]
		WHERE [val].[Id] > @IdTo AND [tc].[Id] IS NULL;

		IF (@IdFrom > @MaxId OR @IdFrom IS NULL) BEGIN
			BREAK;
		END;

		SELECT @IdTo = MIN(val.[Id])
		FROM @val [val]
			LEFT JOIN [stl].[TranslationCode] tc ON [tc].[Id] = [val].[Id]
		WHERE [val].[Id] > @IdFrom AND [tc].[Id] IS NOT NULL;

		INSERT INTO @res VALUES (@IdFrom, @IdTo - 1, @IdTo - @IdFrom);
	END;

	SELECT
		CAST([IdFrom] AS VARCHAR(MAX)) + CASE WHEN [Count] > 1 THEN ' - ' + CAST([IdTo] AS VARCHAR(MAX)) ELSE '' END AS 'Range',
		[Count] AS 'Gap size',
		[IdFrom] AS 'Start Id',
		[IdTo] AS 'End Id'
	FROM @res
	WHERE [Count] >= @MinGapSize
	Order By [IdTo] DESC;
GO
