CREATE PROCEDURE [dbo].[spPayloadDriver]
	@Payload		NVARCHAR(MAX),
	@PayloadType	NVARCHAR(100),
	@PayloadAction	NVARCHAR(100),
	@ExecutedBy		BIGINT,
	@ReturnJSON		NVARCHAR(MAX) OUTPUT
AS
/*
===============================================================================
Main payload driver for Logic App orchestration proof of concept. Note
that error handling and audit logging are removed for clarity. The driver
evaluates the payload type and decides which concrete driver instance it
needs to route the payload.
===============================================================================
*/
BEGIN
SET NOCOUNT, XACT_ABORT ON

DECLARE @OutputJSON NVARCHAR(MAX)

	IF @PayloadType = 'Widget'
	BEGIN
		EXEC dbo.spWidgetDriver 
			@Payload = @Payload
			, @PayloadAction = @PayloadAction
			, @ReturnJSON = @OutputJSON OUTPUT

		SET @ReturnJSON = @OutputJSON
	END

	IF @PayloadType = 'TODO'
	BEGIN
		PRINT 'A different payload type with a call to its concrete driver'
	END

END