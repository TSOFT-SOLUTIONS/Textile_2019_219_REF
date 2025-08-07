CREATE PROCEDURE [SP_get_DiffMeters_of_CrossCheck_PcsChecking_and_PackingSlip_by_LotNo_and_PcsNo_and_ClothIdNo]  @temptablename varchar(100)  AS  
BEGIN   	    
	SET NOCOUNT ON;        
	DECLARE @SQL_QUERY NVARCHAR(max);  
	DECLARE @PARAMS NVARCHAR (1000);   
	
	SET @SQL_QUERY = N'Select Name1 as Lot_Code, Name2 as Piece_No, Int1 as Cloth_IdNo, Int2 as ClothType_IdNo, sum(Meters1) as ProdMtrs from  ' + @temptablename + ' Group by Name1, Name2, Int1, Int2 having sum(Meters1) <> 0';   
	SET @PARAMS = '';   
	EXECUTE sp_executesql @SQL_QUERY, @PARAMS;      

END 