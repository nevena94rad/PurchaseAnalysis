result = tryCatch({ library(forecast) }, warning = function(w) {}, error = function(e) { install.packages('forecast', repos='https://fourdots.com/mirror/CRAN/'); }, finally = {})
library(forecast)
args <- commandArgs(trailingOnly = TRUE)
Qty <- as.numeric(read.table(args[1]))
ConsumptionLength <- as.numeric(args[2])
year1 <- as.numeric(args[3]);
day1 <- as.numeric(args[4]);
sum <- as.numeric(args[5]);

#print(1)
custItemCons <- ts(Qty, start = c(year1, day1), frequency = 365.25)

fit <- auto.arima(custItemCons, stepwise = FALSE, approximation = FALSE, lambda = 0)
model <- as.character(fit)
prognoza <- forecast(fit, h = (ConsumptionLength - sum + 7))

ItemCust <- c(custItemCons, head(prognoza[["mean"]], -7))

print(sum(tail(ItemCust, - sum)) / 2 + sum(tail(prognoza[["mean"]], 7)));

print(model)
