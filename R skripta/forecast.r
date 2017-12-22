bigQuery<-"SELECT Consumption FROM PED.dbo.ItemConsumption
WHERE Date<20171005 AND Date>20150816
ORDER BY Date";

connStr <- "Driver=SQL Server;Server=DESKTOP-I4LD1QU;Database=PED;Trusted_Connection=Yes"
sqlShareDir <- paste("C:\\AllShare\\",Sys.getenv("USERNAME"),sep="")
sqlWait <- TRUE
sqlConsoleOutput <- FALSE

featureDataSource <- RxSqlServerData(sqlQuery = bigQuery,colClasses = c(), connectionString = connStr);
env <- new.env();
changed_ds <- rxDataStep(inData = featureDataSource,rowsPerRead=500, reportProgress = 3);

vremenskiItem<-ts(changed_ds[["Consumption"]],start = c(2015,229), frequency = 365.25)
plot(vremenskiItem)

bigQuery<-"SELECT Consumption as Cons2 FROM PED.dbo.ItemConsumption
WHERE Date<20170425 AND Date>20150816
ORDER BY Date";

featureDataSource <- RxSqlServerData(sqlQuery = bigQuery,colClasses = c(), connectionString = connStr);
env <- new.env();
changed_ds <- rxDataStep(inData = featureDataSource,rowsPerRead=500, reportProgress = 3);

L1<-vremenskiItem/mean(changed_ds[["Cons2"]])

bigQuery<-"SELECT SUM(AvgConsumption)/COUNT(*) as AvgConsumption FROM PED.dbo.CustItemDailyCons
WHERE ProcessingDate<20170425
GROUP BY ProcessingDate
ORDER BY ProcessingDate";

featureDataSource <- RxSqlServerData(sqlQuery = bigQuery,colClasses = c(), connectionString = connStr);
env <- new.env();
changed_ds <- rxDataStep(inData = featureDataSource,rowsPerRead=500, reportProgress = 3);

C1<-ts(changed_ds[["AvgConsumption"]],start = c(2015,229), frequency = 365.25)
L1<-L1*mean(C1)

library(forecast)
fit<-auto.arima(C1, stepwise=FALSE, approximation=FALSE, lambda=0)
prognoza<-forecast(fit,h=163)

ItemCust<-c(C1,prognoza[["mean"]])

rezultat<-(L1+ItemCust)/2;

n<-sum(tail(rezultat,163))