
CREATE PROCEDURE [sp_get_Weaver_ClothReceipt_Piece_Details_by_CheckingCode_ClothReceiptCode_PieceNo]
	@Weaver_Piece_Checking_Code varchar(100), 
	@Weaver_ClothReceipt_Code varchar(100),
	@Piece_No varchar(100)
AS
BEGIN
	SET NOCOUNT ON;
	select TOP 1 * from Weaver_ClothReceipt_Piece_Details where Weaver_Piece_Checking_Code = @Weaver_Piece_Checking_Code and Weaver_ClothReceipt_Code = @Weaver_ClothReceipt_Code  and Piece_No = @Piece_No 
END

GO


