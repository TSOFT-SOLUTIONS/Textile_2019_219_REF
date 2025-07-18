CREATE PROCEDURE [sp_get_weaverclothreceiptpiecedetails_totalmeter_beamconsmeter]
	@weaver_clothreceipt_code varchar(100), 
	@lot_code varchar(100) 
AS
BEGIN
	SET NOCOUNT ON;
   
	Select sum(Receipt_Meters) as RecMtrs, sum(Type1_Meters) as Type1Mtrs, sum(Type2_Meters) as Type2Mtrs, sum(Type3_Meters) as Type3Mtrs, sum(Type4_Meters) as Type4Mtrs, sum(Type5_Meters) as Type5Mtrs, sum(BeamConsumption_Meters) as BeamCons_Meters from Weaver_ClothReceipt_Piece_Details where Weaver_ClothReceipt_Code = @weaver_clothreceipt_code and Lot_Code = @lot_code;

END
