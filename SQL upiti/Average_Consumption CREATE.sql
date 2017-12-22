insert into PED.dbo.CustItemAvgCons(ItemNo, CustNo, Consumption )
select * from(
select a.ItemNo, a.CustNo,(((CAST (sum(a.InvQty) as
float))
-
(select avg(r.InvQty) from PED.dbo.PurchaseHistory r
where r.InvDate in
(
select max(InvDate) from
PED.dbo.PurchaseHistory j
where j.CustNo=r.CustNo and j.ItemNo=r.ItemNo
group by j.ItemNo, j.CustNo
)
and r.CustNo=a.CustNo and r.ItemNo =a.ItemNo
group by r.ItemNo, r.CustNo
)
)/Nullif(DateDiff(day,
CONVERT(datetime, convert(varchar(10),MIN(a.InvDate))),
CONVERT(datetime, convert(varchar(10),MAX(a.InvDate)))),0.0)) as srednja
from PED.dbo.PurchaseHistory a
GROUP BY a.ItemNo, a.CustNo
) b
where b.srednja is not null
order by b.srednja desc