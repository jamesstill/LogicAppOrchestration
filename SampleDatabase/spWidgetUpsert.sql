CREATE PROCEDURE [dbo].[spWidgetUpsert]
	@Payload		NVARCHAR(MAX),
	@PayloadAction	NVARCHAR(100),
	@ReturnJSON		NVARCHAR(MAX) OUTPUT
AS
/*
===============================================================================
Concrete stored procedure for widget insert or update
===============================================================================
*/
BEGIN
SET NOCOUNT, XACT_ABORT ON

	DECLARE
		@Id				BIGINT
		, @Name			NVARCHAR(50)
		, @Shape		NVARCHAR(50)
		, @Color		NVARCHAR(50)
		, @Success		BIT

	SELECT
		@Id			= [Id]
		, @Name		= [Name]
		, @Shape	= [Shape]
		, @Color	= [Color]
	FROM
		OPENJSON (@Payload)
	WITH
	(
		Id			BIGINT			'$.Id'
		, [Name]	NVARCHAR(50)	'$.Name'
		, [Shape]	NVARCHAR(50)	'$.Shape'
		, [Color]	NVARCHAR(50)	'$.Color'
	);



	IF (@PayloadAction = 'Added')
	BEGIN	
		SET IDENTITY_INSERT dbo.Widget ON;

		INSERT INTO dbo.Widget ([Id], [Name], [Shape], [Color]) VALUES (@Id, @Name, @Shape, @Color);
		SET @Success = 1

		SET IDENTITY_INSERT dbo.Widget OFF;
	END
	ELSE
	BEGIN
		UPDATE dbo.Widget SET [Name] = @Name, [Shape] = @Shape, [Color] = @Color WHERE [Id] = @Id;
		SET @Success = 1
	END

	SET @ReturnJSON = (
		SELECT
			@Id AS Id
			, @Success AS Success
		FOR JSON PATH
	);

END