
try(elog$date <- as.Date(elog$date, "%Y%m%d"));
try(elog <- dc.MergeTransactionsOnSameDate(elog));

try(split.data <- dc.SplitUpElogForRepeatTrans(elog));
try(clean.elog <- split.data$repeat.trans.elog);
try(freq.cbt <- dc.CreateSpendCBT(clean.elog));

try(tot.cbt <- dc.CreateSpendCBT(elog))
try(cal.cbt <- dc.MergeCustomers(tot.cbt, freq.cbt))

try(birth.periods <- split.data$cust.data$birth.per)
try(last.dates <- split.data$cust.data$last.date)

