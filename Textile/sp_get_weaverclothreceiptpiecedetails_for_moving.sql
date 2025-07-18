CREATE PROCEDURE [sp_get_weaverclothreceiptpiecedetails_for_moving2]
	@weaver_clothreceipt_code varchar(100) 
AS
BEGIN
	SET NOCOUNT ON;

	Select a.*, b.Loom_Name from Weaver_ClothReceipt_Piece_Details a LEFT OUTER JOIN Loom_Head b ON a.Loom_IdNo = b.Loom_IdNo Where a.Weaver_ClothReceipt_Code = @weaver_clothreceipt_code Order by a.PieceNo_OrderBy, a.Sl_No, a.Piece_No;

END
