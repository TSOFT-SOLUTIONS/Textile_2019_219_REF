CREATE PROCEDURE sp_save_Weaver_ClothReceipt_Piece_Details_update_and_insert
	@Weaver_Piece_Checking_Code varchar(100),  
	@Company_IdNo int,  
	@Weaver_Piece_Checking_No varchar(100),  
	@Weaver_Piece_Checking_Date datetime,  
	@Ledger_Idno int,  
	@StockOff_IdNo int,  
	@Weaver_ClothReceipt_Code varchar(100),  
	@Weaver_ClothReceipt_No varchar(100),  
	@for_orderby numeric,  
	@Weaver_ClothReceipt_Date datetime,  
	@Lot_Code varchar(100), 
	@Lot_No varchar(100), 
	@Cloth_IdNo int, 
	@Folding_Checking numeric, 
	@Folding numeric, 
	@Sl_No int, 
	@Piece_No varchar(100), 
	@Main_PieceNo varchar(100), 
	@PieceNo_OrderBy numeric, 
	@ReceiptMeters_Checking numeric, 
	@Receipt_Meters numeric, 
	@Loom_No varchar(100), 
	@Is_LastPiece varchar(100), 
	@Pick numeric, 
	@Width numeric, 
	@Type1_Meters numeric, 
	@Type2_Meters numeric, 
	@Type3_Meters numeric, 
	@Type4_Meters numeric, 
	@Type5_Meters numeric, 
	@Total_Checking_Meters numeric,  
	@Weight numeric, 
	@Weight_Meter numeric, 
	@Beam_Knotting_Code varchar(100), 
	@Beam_Knotting_No varchar(100), 
	@Loom_IdNo int, 
	@Width_Type varchar(100), 
	@Crimp_Percentage numeric, 
	@Set_Code1 varchar(100), 
	@Set_No1 varchar(100), 
	@Beam_No1 varchar(100), 
	@Balance_Meters1 numeric, 
	@Set_Code2 varchar(100), 
	@Set_No2 varchar(100), 
	@Beam_No2 varchar(100), 
	@Balance_Meters2 numeric,  
	@BeamConsumption_Meters numeric

AS

BEGIN

	SET NOCOUNT ON;

	IF EXISTS(SELECT Weaver_ClothReceipt_Code, Lot_Code, Piece_No, Receipt_Meters, Type1_Meters, Type2_Meters, Type3_Meters, Type4_Meters, Type5_Meters, Total_Checking_Meters FROM [Weaver_ClothReceipt_Piece_Details] WHERE Weaver_ClothReceipt_Code = @Weaver_ClothReceipt_Code and Lot_Code = @Lot_Code and Piece_No = @Piece_No)

		BEGIN
		
			UPDATE Weaver_ClothReceipt_Piece_Details
			SET [Weaver_Piece_Checking_Code] = @Weaver_Piece_Checking_Code,
			[Weaver_Piece_Checking_No] = @Weaver_Piece_Checking_No,
			[Weaver_Piece_Checking_Date] = @Weaver_Piece_Checking_Date,
			[Ledger_Idno] = @Ledger_Idno,
			[StockOff_IdNo] = @StockOff_IdNo,
			[Cloth_IdNo] = @Cloth_IdNo,
			[Folding_Checking] = @Folding_Checking,
			[Folding] = @Folding,
			[Sl_No] = @Sl_No,
			[Main_PieceNo] = @Main_PieceNo,
			[PieceNo_OrderBy] = @PieceNo_OrderBy,
			[ReceiptMeters_Checking] = @ReceiptMeters_Checking,
			[Receipt_Meters] = @Receipt_Meters,
			[Loom_No] = @Loom_No,
			[Is_LastPiece] = @Is_LastPiece,
			[Pick] = @Pick,
			[Width] = @Width,
			[Type1_Meters] = @Type1_Meters,
			[Type2_Meters] = @Type2_Meters,
			[Type3_Meters] = @Type3_Meters,
			[Type4_Meters] = @Type4_Meters,
			[Type5_Meters] = @Type5_Meters,
			[Total_Checking_Meters] = @Total_Checking_Meters,
			[Weight] = @Weight,
			[Weight_Meter] = @Weight_Meter,
			[Beam_Knotting_Code] = @Beam_Knotting_Code,
			[Beam_Knotting_No] = @Beam_Knotting_No,
			[Loom_IdNo] = @Loom_IdNo,
			[Width_Type] = @Width_Type,
			[Crimp_Percentage] = @Crimp_Percentage,
			[Set_Code1] = @Set_Code1,
			[Set_No1] = @Set_No1,
			[Beam_No1] = @Beam_No1,
			[Balance_Meters1] = @Balance_Meters1,
			[Set_Code2] = @Set_Code2,
			[Set_No2] = @Set_No2,
			[Beam_No2] = @Beam_No2,
			[Balance_Meters2] = @Balance_Meters2,
			[BeamConsumption_Meters] = @BeamConsumption_Meters
			WHERE [Weaver_ClothReceipt_Code] = @Weaver_ClothReceipt_Code and 
			[Lot_Code] = @Lot_Code and 
			[Piece_No] = @Piece_No ;

		END

	ELSE

		BEGIN

			INSERT INTO Weaver_ClothReceipt_Piece_Details ( [Weaver_Piece_Checking_Code], [Company_IdNo], [Weaver_Piece_Checking_No], [Weaver_Piece_Checking_Date], [Ledger_Idno], [StockOff_IdNo], [Weaver_ClothReceipt_Code], [Weaver_ClothReceipt_No], [for_orderby], [Weaver_ClothReceipt_Date], [Lot_Code],  Lot_No,  Cloth_IdNo,  Folding_Checking,  Folding,  Sl_No,  Piece_No,  Main_PieceNo,  PieceNo_OrderBy,  ReceiptMeters_Checking,  Receipt_Meters,  Loom_No,  Is_LastPiece,  Pick,  Width,  Type1_Meters,  Type2_Meters,  Type3_Meters,  Type4_Meters,  Type5_Meters,  Total_Checking_Meters, [Weight],  Weight_Meter,  Beam_Knotting_Code,  Beam_Knotting_No,  Loom_IdNo,  Width_Type,  Crimp_Percentage,  Set_Code1,  Set_No1,  Beam_No1,  Balance_Meters1,  Set_Code2,  Set_No2,  Beam_No2,  Balance_Meters2,  BeamConsumption_Meters )
			VALUES										  (  @Weaver_Piece_Checking_Code,  @Company_IdNo,  @Weaver_Piece_Checking_No,  @Weaver_Piece_Checking_Date,  @Ledger_Idno,  @StockOff_IdNo,  @Weaver_ClothReceipt_Code,  @Weaver_ClothReceipt_No,  @for_orderby,  @Weaver_ClothReceipt_Date,  @Lot_Code, @Lot_No, @Cloth_IdNo, @Folding_Checking, @Folding, @Sl_No, @Piece_No, @Main_PieceNo, @PieceNo_OrderBy, @ReceiptMeters_Checking, @Receipt_Meters, @Loom_No, @Is_LastPiece, @Pick, @Width, @Type1_Meters, @Type2_Meters, @Type3_Meters, @Type4_Meters, @Type5_Meters, @Total_Checking_Meters,  @Weight, @Weight_Meter, @Beam_Knotting_Code, @Beam_Knotting_No, @Loom_IdNo, @Width_Type, @Crimp_Percentage, @Set_Code1, @Set_No1, @Beam_No1, @Balance_Meters1, @Set_Code2, @Set_No2, @Beam_No2, @Balance_Meters2, @BeamConsumption_Meters );
		
		END

END

	
GO


