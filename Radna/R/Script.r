
elog$date <- as.Date(elog$date, "%Y%m%d");
elog <- dc.MergeTransactionsOnSameDate(elog);

split.data <- dc.SplitUpElogForRepeatTrans(elog);
clean.elog <- split.data$repeat.trans.elog;
freq.cbt <- dc.CreateSpendCBT(clean.elog);

tot.cbt <- dc.CreateSpendCBT(elog)
cal.cbt <- dc.MergeCustomers(tot.cbt, freq.cbt)

birth.periods <- split.data$cust.data$birth.per
last.dates <- split.data$cust.data$last.date

