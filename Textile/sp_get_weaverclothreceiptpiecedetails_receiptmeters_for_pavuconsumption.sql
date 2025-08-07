CREATE PROCEDURE [sp_get_weaverclothreceiptpiecedetails_receiptmeters_for_pavuconsumption]
	@weaver_clothreceipt_code varchar(100), 
	@lot_code varchar(100) 
AS
BEGIN
	SET NOCOUNT ON;
	select a.cloth_idno, a.crimp_percentage, a.width_type, b.noof_input_beams, sum(a.receipt_meters) as rcptmtrs from weaver_clothreceipt_piece_details a inner join loom_head b on b.loom_idno <> 0 and a.loom_idno = b.loom_idno where a.weaver_clothreceipt_code = @weaver_clothreceipt_code and a.lot_code = @lot_code group by a.cloth_idno, a.crimp_percentage, a.width_type, b.noof_input_beams having sum(a.receipt_meters) <> 0;
END
