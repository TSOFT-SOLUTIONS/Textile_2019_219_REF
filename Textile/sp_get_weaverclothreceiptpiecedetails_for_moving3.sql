CREATE PROCEDURE [sp_get_weaverclothreceiptpiecedetails_for_moving3]
	@weaver_clothreceipt_code varchar(100) ,
	@lot_code varchar(100) 
AS
BEGIN
	SET NOCOUNT ON;

	select * from Weaver_ClothReceipt_Piece_Details where Weaver_ClothReceipt_Code = @weaver_clothreceipt_code and Lot_Code = @lot_code and Create_Status = 1 Order by Sl_No, Piece_No;

END
