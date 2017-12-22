DECLARE @StartDT INT
SET @StartDT =  '20150816'

WHILE @StartDT < '20171005'
BEGIN
	INSERT INTO PED.dbo.CustItemDailyCons (ItemNo,CustNo,ProcessingDate,AvgConsumption)
	SELECT a.ItemNo,a.CustNo,@StartDT, CAST(InvQty AS FLOAT)/(PurchasePeriod)  as prosPotrosnja FROM
		PED.dbo.PurchasePeriods a INNER JOIN PED.dbo.PurchaseHistory b
		ON a.ItemNo = b.ItemNo AND a.CustNo= b.CustNo AND a.InvDatePrior = CONVERT(datetime, convert(varchar(10), b.InvDate))
		WHERE CONVERT(datetime, convert(varchar(10), @StartDT)) <= a.InvDateCurr AND 
		CONVERT(datetime, convert(varchar(10), @StartDT)) >= a.InvDatePrior
				AND a.ItemNo = '10-4784' AND a.CustNo = 'FIV108'
		set @StartDT=@StartDT+1
END