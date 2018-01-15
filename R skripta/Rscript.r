library("forecast")

args <- commandArgs()
Consumption <- as.numeric(unlist(strsplit(args[2],",")))
Qty <- as.numeric(unlist(strsplit(args[3],",")))
year1 <- as.numeric(args[4]);
day1 <- as.numeric(args[5]);
sum <- as.numeric(args[6]);

#return (Qty[1])

custItemCons <- ts(Qty, start=c(year1,day1),frequency = 365.25)      
vremenskiItem<-ts(Consumption,start=c(year1,day1),frequency = 365.25)  

itemMean <- head(Consumption,sum)
itemMean <- mean(itemMean)                                            

L1 <- (vremenskiItem/itemMean) * mean(custItemCons)

fit<-auto.arima(custItemCons, stepwise=FALSE, approximation=FALSE, lambda=0)                #model za customerItemConsumption

prognoza<-forecast(fit,h=(length(Consumption)-sum + 7))                                       #prognoziran customerItemConsumption

ItemCust<-c(custItemCons,head(prognoza[["mean"]], -7))

rezultat<-(L1+ItemCust)/2

return (sum(tail(rezultat,-sum))+sum(tail(prognoza[["mean"]],7)))
