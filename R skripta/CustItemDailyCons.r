custItem<-changed_ds[["CI_narudzbine"]];
numOfRecords<-length(custItem);
result<-numeric(length = length(custItem));
current<-1;
period<-1;
qty<-0;
while(current<=numOfRecords)
{
  if(!is.na(custItem[current]))
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