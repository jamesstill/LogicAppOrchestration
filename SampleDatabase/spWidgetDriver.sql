CREATE PROCEDURE [dbo].[spWidgetDriver]
	@Payload		NVARCHAR(MAX),
	@PayloadAction	NVARCHAR(100),
	@ReturnJSON		NVARCHAR(MAX) OUTPUT
AS
/*
===============================================================================
Specific payload driver for widgets.
===============================================================================
*/
BEGIN
SET NOCOUNT, XACT_ABORT ON

	DECLARE @OutputJSON NVARCHAR(MAX)

	IF @PayloadAction = 'Deleted'
	BEGIN
		EXEC dbo.spWidgetDelete
			@Payload = @Payload
			, @PayloadAction = @PayloadAction
			, @ReturnJSON = @OutputJSON OUTPUT
	END
	ELSE
	BEGIN
		EXEC dbo.SpWidgetUpsert
			@Payload = @Payload
			, @PayloadAction = @PayloadAction
			, @ReturnJSON = @OutputJSON OUTPUT
	END

	SET @ReturnJSON = @OutputJSON

END