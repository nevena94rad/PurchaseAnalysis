BULK INSERT PED.dbo.PurchaseHistory
FROM 'C:\Users\neven\Desktop\PurchaseAnalysis\podaci\Tigers_Invoices.txt'
WITH
(
  FIELDTERMINATOR = '\t',
  ROWTERMINATOR = '\n',
  ROWS_PER_BATCH = 10000, 
  FIRSTROW = 2,
  TABLOCK
)