CREATE PROCEDURE [sp_get_weaverclothreceiptpiecedetails_unchecked_receiptmeters]
	@weaver_clothreceipt_code varchar(100), 
	@lot_code varchar(100) 
AS
BEGIN
	SET NOCOUNT ON;
	Select sum(Receipt_Meters) as RecMtrs from Weaver_ClothReceipt_Piece_Details where Weaver_ClothReceipt_Code = @weaver_clothreceipt_code and Lot_Code = @lot_code and (Type1_Meters+Type2_Meters+Type3_Meters+Type4_Meters+Type5_Meters) = 0;
END
