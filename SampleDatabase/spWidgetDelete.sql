CREATE PROCEDURE [dbo].[spWidgetDelete]
	@Payload		NVARCHAR(MAX),
	@PayloadAction	NVARCHAR(100),
	@ReturnJSON		NVARCHAR(MAX) OUTPUT
AS
/*
===============================================================================
Concrete stored procedure for widget delete
===============================================================================
*/
BEGIN
SET NOCOUNT, XACT_ABORT ON

	DECLARE
		@Id					BIGINT
		, @Success			BIT

	SELECT
		@Id					= Id
	FROM
		OPENJSON (@Payload)
	WITH
	(
		Id				BIGINT '$.Id'
	);

	IF @PayloadAction = 'Deleted'
	BEGIN
		
		DELETE dbo.Widget WHERE Id = @Id
		SET @Success = 1

	END

	SET @ReturnJSON = (
		SELECT
			@Id AS Id
			, @Success AS Success
		FOR JSON PATH
	);

END
