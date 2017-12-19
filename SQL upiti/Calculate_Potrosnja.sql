DECLARE @StartDT INT
SET @StartDT =  '20150500'

WHILE @StartDT < '20171200'
BEGIN
	INSERT INTO ARIMA.dbo.Potrosnja (item, datum, potrosnja)
	SELECT a.item,@StartDT,(cast (SUM(PurchasePeriod) as float)/SUM(InvQty)) as prosPotrosnja FROM
		ARIMA.dbo.PurchasePeriods a INNER JOIN ARIMA.dbo.Tigers_Invoices b
		ON a.item = b.item AND a.custNo= b.custNo AND a.invdatetimeprior = CONVERT(datetime, convert(varchar(10), b.InvDate))
		WHERE CONVERT(datetime, convert(varchar(10), @StartDT)) <= a.invdatetimecurr AND CONVERT(datetime, convert(varchar(10), @StartDT)) >= a.invdatetimeprior
				AND a.item = '10-4784'
		GROUP BY a.item
		set @StartDT=@StartDT+1
END