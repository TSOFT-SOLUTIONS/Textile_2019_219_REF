CREATE PROCEDURE [sp_save_yarn_pavu_cloth_stockposting_update]
	@reference_code varchar(100), 
	@reference_date datetime, 
	@weaverwagescode varchar(100), 
	@yarn_consumedweight decimal, 
	@pavu_consumedmeter decimal, 
	@folding decimal, 
	@unchecked_meters decimal, 
	@cloth_meterstype1 decimal, 
	@cloth_meterstype2 decimal, 
	@cloth_meterstype3 decimal, 
	@cloth_meterstype4 decimal, 
	@cloth_meterstype5 decimal
AS
BEGIN
	SET NOCOUNT ON;

	update stock_cloth_processing_details set reference_date = @reference_date,  folding = @folding, unchecked_meters = @unchecked_meters, meters_type1 = @cloth_meterstype1, meters_type2 = @cloth_meterstype2, meters_type3 = @cloth_meterstype3, meters_type4 = @cloth_meterstype4, meters_type5 = @cloth_meterstype5 where reference_code = @reference_code;

	IF @weaverwagescode IS NOT NULL  
	BEGIN  
		
		IF @weaverwagescode <> ''
		BEGIN  
		
			Update Stock_Yarn_Processing_Details set [Weight] = @yarn_consumedweight Where Reference_Code = @reference_code;

			Update Stock_Pavu_Processing_Details set [Meters] = @pavu_consumedmeter Where Reference_Code = @reference_code;
  
		END
  
	END

	
END
