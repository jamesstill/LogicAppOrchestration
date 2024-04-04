CREATE PROCEDURE [dbo].[spGetWidgets]
AS
/*
===============================================================================
Get all widgets
===============================================================================
*/
SET NOCOUNT, XACT_ABORT ON

	SELECT
		[Id]
		, [Name]
		, [Shape]
		, [Color]
	FROM
		dbo.Widget
GO