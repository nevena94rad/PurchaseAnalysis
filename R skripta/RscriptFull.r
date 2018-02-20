library("forecast")

args <- commandArgs(trailingOnly = TRUE)
Qty <- as.numeric(read.table(args[1]))
ConsumptionLength <- as.numeric(args[2])
year1 <- as.numeric(args[3]);
day1 <- as.numeric(args[4]);
sum <- as.numeric(args[5]);

#return (Qty[1])

custItemCons <- ts(Qty, start=c(year1,day1),frequency = 365.25)      
#vremenskiItem<-ts(Consumption,start=c(year1,day1),frequency = 365.25)  

#itemMean <- head(Consumption,sum)
#itemMean <- mean(itemMean)                                            

#L1 <- (vremenskiItem/itemMean) * mean(custItemCons)

fit<-auto.arima(custItemCons, stepwise=FALSE, approximation=FALSE, lambda=0)                #model za customerItemConsumption
model<-as.character(fit)                                                                   #iskoriscen model
prognoza<-forecast(fit,h=(ConsumptionLength-sum + 7))                                     #prognoziran customerItemConsumption

ItemCust<-c(custItemCons,head(prognoza[["mean"]], -7))

#rezultat<-(L1+ItemCust)/2

#print(sum(tail(rezultat,-sum))+sum(tail(prognoza[["mean"]],7)));
print(sum(tail(ItemCust,-sum))/2 + sum(tail(prognoza[["mean"]],7)));