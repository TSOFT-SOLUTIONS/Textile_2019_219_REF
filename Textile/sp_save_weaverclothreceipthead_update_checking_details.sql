CREATE PROCEDURE [sp_save_weaverclothreceipthead_update_checking_details]
	@weaver_clothreceipt_code varchar(100), 
	@weaver_piece_checking_code varchar(100),
	@weaver_piece_checking_increment int,
	@weaver_piece_checking_date datetime, 
	@folding_checking decimal, 
	@folding decimal, 
	@receiptmeters_checking decimal, 
	@receipt_meters decimal, 
	@consumedyarn_checking decimal, 
	@consumed_yarn decimal, 
	@consumedpavu_checking decimal, 
	@consumed_pavu decimal, 
	@beamconsumption_checking decimal, 
	@beamconsumption_meters decimal,
	@type1_checking_meters decimal, 
	@type2_checking_meters decimal, 
	@type3_checking_meters decimal, 
	@type4_checking_meters decimal, 
	@type5_checking_meters decimal, 
	@total_checking_meters decimal 
AS
BEGIN
	SET NOCOUNT ON;

	Update Weaver_Cloth_Receipt_Head set  Weaver_Piece_Checking_Code = @weaver_piece_checking_code, Weaver_Piece_Checking_Increment = @weaver_piece_checking_increment, Weaver_Piece_Checking_Date = @weaver_piece_checking_date, Folding_Checking = @folding_checking, Folding = @folding, ReceiptMeters_Checking = @receiptmeters_checking, Receipt_Meters = @receipt_meters, ConsumedYarn_Checking = @consumedyarn_checking, Consumed_Yarn = @consumed_yarn, ConsumedPavu_Checking = @consumedpavu_checking, Consumed_Pavu = @consumed_pavu, BeamConsumption_Checking = @beamconsumption_checking, BeamConsumption_Meters = @beamconsumption_meters, Type1_Checking_Meters = @type1_checking_meters, Type2_Checking_Meters = @type2_checking_meters, Type3_Checking_Meters = @type3_checking_meters, Type4_Checking_Meters = @type4_checking_meters, Type5_Checking_Meters = @type5_checking_meters, Total_Checking_Meters = @total_checking_meters  Where Weaver_ClothReceipt_Code = @weaver_clothreceipt_code;
								
END
