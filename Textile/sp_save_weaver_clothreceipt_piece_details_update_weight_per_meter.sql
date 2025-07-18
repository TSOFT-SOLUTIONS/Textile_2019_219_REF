CREATE PROCEDURE [sp_save_weaver_clothreceipt_piece_details_update_weight_per_meter]
	@weaver_piece_checking_code varchar(100), 
	@weaver_clothreceipt_code varchar(100), 
	@lot_code varchar(100), 
	@main_pieceno varchar(100),
	@weight_meter decimal
AS
BEGIN
	SET NOCOUNT ON;
   
	Update Weaver_ClothReceipt_Piece_Details set Weight_Meter = @weight_meter where weaver_piece_checking_code = @weaver_piece_checking_code and weaver_clothreceipt_code = @weaver_clothreceipt_code and lot_code = @lot_code and main_pieceno = @main_pieceno;

END
