insert into ARIMA.dbo.Average_Consumption(Item, CustNo, Consumption )
select * from(
select a.Item, a.CustNo,(((CAST (sum(a.InvQty) as
float))
-
(select avg(r.InvQty) from ARIMA.dbo.Tigers_Invoices r
where r.InvDate in
(
select max(InvDate) from
ARIMA.dbo.Tigers_Invoices j
where j.CustNo=r.CustNo and j.Item=r.Item
group by j.Item, j.CustNo
)
and r.CustNo=a.CustNo and r.Item =a.Item
group by r.Item, r.CustNo
)
)/Nullif(DateDiff(day,
CONVERT(datetime, convert(varchar(10),MIN(a.InvDate))),
CONVERT(datetime, convert(varchar(10),MAX(a.InvDate)))),0.0)) as srednja
from ARIMA.dbo.Tigers_Invoices a
GROUP BY a.Item, a.CustNo
) b
where b.srednja is not null
order by b.srednja desc