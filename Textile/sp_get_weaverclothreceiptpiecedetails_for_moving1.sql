CREATE PROCEDURE [sp_get_weaverclothreceiptpiecedetails_for_moving1]
	@weaver_piece_checking_code varchar(100) 
AS
BEGIN
	SET NOCOUNT ON;

	Select a.*, b.employee_name as checkername, c.employee_name as foldername from Weaver_ClothReceipt_Piece_Details a  LEFT OUTER JOIN Employee_Head b ON a.Checker_IdNo <> 0 and a.Checker_IdNo = b.Employee_IdNo LEFT OUTER JOIN Employee_Head c ON a.folder_idno <> 0 and a.folder_idno = c.Employee_IdNo Where a.Weaver_Piece_Checking_Code = @weaver_piece_checking_code Order by a.PieceNo_OrderBy, a.Piece_No , a.Sl_No;

END
