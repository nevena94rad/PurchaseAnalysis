delete from PED.dbo.CustItemAvgCons
where ItemNo+CustNo in
(select r.ItemNo+r.CustNo from PED.dbo.PurchaseHistory r
group by r.ItemNo+r.CustNo
having MAX(r.InvDate)<20170000
)