install.packages("lubridate")
library("lubridate")

year1 <- date1 %/% 10000
day1<-as.Date(date1, "%Y%m%d")
yday(day1)

custItemQty <- InputDataSet[["Qty"]]
custItemConsumption <- CustItemDailyCons(custItemQty)
custItemCons <- ts(custItemConsumption, start=c(year1,day1),frequency = 365.25)

vremenskiItem<-ts(InputDataSet[["Consumption"]]),start=c(year1,day1,frequency = 365.25)

dates<-InputDataSet[["Date"]]
sum <- 0
current <- 1
while(dates[current] <= date2)
{
    sum <- sum + 1
}

itemMean <- head(InputDataSet[["Consumption"]],sum)
itemMean <- mean(itemMean)

L1 <- (vremenskiItem/itemMean) * mean(custItemCons)

library(forecast)
fit<-auto.arima(custItemCons, stepwise=FALSE, approximation=FALSE, lambda=0)
prognoza<-forecast(fit,h=length(dates)-sum)

ItemCust<-c(custItemCons,prognoza[["mean"]])

rezultat<-(L1+ItemCust)/2;

n<-sum(tail(rezultat,163))



