USE PED;
GO
CREATE PROCEDURE Forecast (@param1 varchar(50), @param2 varchar(50), @param3 int, @param4 int, @param5 int)
AS
EXECUTE sp_execute_external_script
      @language = N'R'
    , @script = N'		
						CustItemDailyCons <- function(custItem)
						{
							numOfRecords<-length(custItem);
							result<-numeric(length = length(custItem));
							current<-1;
							period<-1;
							qty<-0;
							while(current<=numOfRecords)
							{
								if(!is.na(custItem[current]) && current != numOfRecords)
								{
									period <- 1
									qty <- custItem[current]
									innerCurr <- current + 1
									while(is.na(custItem[innerCurr]) && innerCurr<=numOfRecords)
									{  
										period<-period+1
										innerCurr<-innerCurr+1
									}
									result[current]<-qty/period
									current<-current+1
								}
								else
								{
									result[current]<-qty/period
									current<-current+1
								}
							}
							return(result)
						}


						library(lubridate)
						library("forecast")
						year1 <- date1 %/% 10000
						day1<-as.Date(toString(date1), "%Y%m%d")
						day1<-yday(day1)

						dates<-InputDataSet[["Date"]]
						sum <- 0
						current <- 1
						while(dates[current] < date2)
						{
							sum <- sum + 1
							current<-current+1
						}

						custItemQty <- InputDataSet[["Qty"]]
						custItemConsumption <- CustItemDailyCons(head(custItemQty,sum))
						custItemCons <- ts(custItemConsumption, start=c(year1,day1),frequency = 365.25)

						vremenskiItem<-ts(InputDataSet[["Consumption"]],start=c(year1,day1),frequency = 365.25)

						itemMean <- head(InputDataSet[["Consumption"]],sum)
						itemMean <- mean(itemMean)

						L1 <- (vremenskiItem/itemMean) * mean(custItemCons)

						fit<-auto.arima(custItemCons, stepwise=FALSE, approximation=FALSE, lambda=0)
						prognoza<-forecast(fit,h=(length(dates)-sum))

						ItemCust<-c(custItemCons,prognoza[["mean"]])

						rezultat<-(L1+ItemCust)/2
							
						OutputDataSet<-data.frame(sum(tail(rezultat,-sum)))
	
	'
	, @input_data_1 = N'	select Consumption, Qty, Date from
							((SELECT Consumption, Date FROM PED.dbo.ItemConsumption
							WHERE Date<@date3 AND Date>@date1 
							and ItemNo=@item) a
							left join
							(SELECT Sum(InvQty) as Qty, InvDate FROM PED.dbo.PurchaseHistory 
							where ItemNo=@item and CustNo=@cust
							group by InvDate) b
							On a.Date = b.InvDate)
							order by Date '
							
	, @params = N' @item varchar(50), @cust varchar(50), @date1 int, @date2 int, @date3 int'
	, @item = @param1
	, @cust = @param2
	, @date1 = @param3
	, @date2 = @param4
	, @date3 = @param5						
    WITH RESULT SETS (([Potrosnja] float NOT NULL));