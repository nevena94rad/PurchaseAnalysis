DECLARE @StartDT INT
SET @StartDT =  '20150500'

WHILE @StartDT < '20171200'
BEGIN
	INSERT INTO PED.dbo.ItemConsumption (ItemNo, Date, Consumption)
	SELECT a.ItemNo,@StartDT,(cast (SUM(InvQty) as float)/SUM(PurchasePeriod)) as prosPotrosnja FROM
		Ped.dbo.PurchasePeriods a INNER JOIN PED.dbo.PurchaseHistory b
		ON a.ItemNo = b.ItemNo AND a.CustNo= b.CustNo AND a.InvDatePrior = CONVERT(datetime, convert(varchar(10), b.InvDate))
		WHERE CONVERT(datetime, convert(varchar(10), @StartDT)) <= a.InvDateCurr AND CONVERT(datetime, convert(varchar(10), @StartDT)) >= a.InvDatePrior
		GROUP BY a.ItemNo
		set @StartDT=@StartDT+1
END