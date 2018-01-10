//ulaz u R skriptu:
//Consumption, Qty, BeginDate, Sum

library(lubridate)
library("forecast")

args = commandArgs(trailingOnly = TRUE)
Consumption <- args[1];
Qty <- args[2];
date1 <- args[3];
sum <- args[4];

year1 <- date1 %/% 10000
day1<-as.Date(toString(date1), "%Y%m%d")
day1<-yday(day1)

custItemCons <- ts(Qty, start=c(year1,day1),frequency = 365.25)         //vremenski za signal customer item Consumption 
vremenskiItem<-ts(Consumption,start=c(year1,day1),frequency = 365.25)   //vremenski za signal item Consumption

itemMean <- head(Consumption,sum)
itemMean <- mean(itemMean)                                              //srednja vrednost za signal item Consumption

L1 <- (vremenskiItem/itemMean) * mean(custItemCons)                     //normalizovan signal za item Consumption

fit<-auto.arima(custItemCons, stepwise=FALSE, approximation=FALSE, lambda=0)
prognoza<-forecast(fit,h=(length(Consumption)-sum))

ItemCust<-c(custItemCons,prognoza[["mean"]])

rezultat<-(L1+ItemCust)/2

print(sum(tail(rezultat,-sum)))