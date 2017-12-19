delete from ARIMA.dbo.Average_Consumption
where Item+CustNo in
(select r.Item+r.CustNo from ARIMA.dbo.Tigers_Invoices r
group by r.Item+r.CustNo
having MAX(r.InvDate)<20170000
)