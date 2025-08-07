CREATE PROCEDURE SP_get_ProductionMeters_of_Beam
	@temptablename varchar(100), 
	@setcode varchar(100), 
	@beamno varchar(100)
AS

BEGIN

	SET NOCOUNT ON;

	DECLARE @SQL_QUERY NVARCHAR(max);    
	DECLARE @PARAMS NVARCHAR (1000);

	SET @SQL_QUERY = N'Truncate table ' + @temptablename;
	EXECUTE sp_executesql @SQL_QUERY, @PARAMS;

	SET @SQL_QUERY = N'Insert into ' + @temptablename + ' ( Name1, Meters1 ) Select a.Width_Type,  (CASE WHEN (a.Type1_Meters+a.Type2_Meters+a.Type3_Meters+a.Type4_Meters+a.Type5_Meters) <> 0 THEN (a.Type1_Meters+a.Type2_Meters+a.Type3_Meters+a.Type4_Meters+a.Type5_Meters) ELSE a.Receipt_Meters END) from Weaver_ClothReceipt_Piece_Details a Where a.Set_Code1 = @setcode and a.Beam_No1 = @beamno';
	SET @PARAMS = N'@setcode varchar(100), @beamno varchar(100)';
	EXECUTE sp_executesql @SQL_QUERY, @PARAMS, @setcode=@setcode, @beamno=@beamno;
	

	SET @SQL_QUERY = N'Insert into ' + @temptablename + ' ( Name1, Meters1 ) Select a.Width_Type, (CASE WHEN (a.Type1_Meters+a.Type2_Meters+a.Type3_Meters+a.Type4_Meters+a.Type5_Meters) <> 0 THEN (a.Type1_Meters+a.Type2_Meters+a.Type3_Meters+a.Type4_Meters+a.Type5_Meters) ELSE a.Receipt_Meters END) from Weaver_ClothReceipt_Piece_Details a Where a.Set_Code2 = @setcode and a.Beam_No2 = @beamno ';
	SET @PARAMS = N'@setcode varchar(100), @beamno varchar(100)';
	EXECUTE sp_executesql @SQL_QUERY, @PARAMS, @setcode=@setcode, @beamno=@beamno;

	SET @SQL_QUERY = N'Insert into ' + @temptablename + ' ( Name1, Meters1 ) Select a.Width_Type, (CASE WHEN (a.Type1_Checking_Meters+a.Type2_Checking_Meters+a.Type3_Checking_Meters+a.Type4_Checking_Meters+a.Type5_Checking_Meters) <> 0 THEN (a.Type1_Checking_Meters+a.Type2_Checking_Meters+a.Type3_Checking_Meters+a.Type4_Checking_Meters+a.Type5_Checking_Meters) ELSE a.Receipt_Meters END) from Weaver_Cloth_Receipt_Head a Where a.Set_Code1 = @setcode and a.Beam_No1 = @beamno';
	SET @PARAMS = N'@setcode varchar(100), @beamno varchar(100)';
	EXECUTE sp_executesql @SQL_QUERY, @PARAMS, @setcode=@setcode, @beamno=@beamno;

	SET @SQL_QUERY = N'Insert into ' + @temptablename + ' ( Name1, Meters1 ) Select a.Width_Type, (CASE WHEN (a.Type1_Checking_Meters+a.Type2_Checking_Meters+a.Type3_Checking_Meters+a.Type4_Checking_Meters+a.Type5_Checking_Meters) <> 0 THEN (a.Type1_Checking_Meters+a.Type2_Checking_Meters+a.Type3_Checking_Meters+a.Type4_Checking_Meters+a.Type5_Checking_Meters) ELSE a.Receipt_Meters END) from Weaver_Cloth_Receipt_Head a Where a.Set_Code2 = @setcode and a.Beam_No2 = @beamno';
	SET @PARAMS = N'@setcode varchar(100), @beamno varchar(100)';
	EXECUTE sp_executesql @SQL_QUERY, @PARAMS, @setcode=@setcode, @beamno=@beamno;


	SET @SQL_QUERY = N'Select Name1, sum(Meters1) as ProdMeters from ' + @temptablename + ' group by Name1 Having sum(Meters1) <> 0';
	SET @PARAMS = '';
	EXECUTE sp_executesql @SQL_QUERY, @PARAMS;
	
END

GO


