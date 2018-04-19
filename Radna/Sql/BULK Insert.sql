BULK INSERT PED.dbo.PurchaseHistory
FROM 'D:\Projects GitHub\PurchaseAnalysis_DATA\data\Tigers_Invoices.txt'
WITH
(
  FIELDTERMINATOR = '\t',
  ROWTERMINATOR = '\n',
  ROWS_PER_BATCH = 10000, 
  FIRSTROW = 2,
  TABLOCK
);

BULK INSERT PED.dbo.PLS_RecomendHist
FROM 'D:\Projects GitHub\PurchaseAnalysis_DATA\data\PLS_RecomendHist.txt'
WITH
(
  FIELDTERMINATOR = '\t',
  ROWTERMINATOR = '\n',
  ROWS_PER_BATCH = 10000, 
  FIRSTROW = 2,
  TABLOCK
);

BULK INSERT PED.dbo.PurchasePeriods
FROM 'D:\Projects GitHub\PurchaseAnalysis_DATA\data\PurchasePeriods.txt'
WITH
(
  FIELDTERMINATOR = '\t',
  ROWTERMINATOR = '\n',
  ROWS_PER_BATCH = 10000, 
  FIRSTROW = 2,
  TABLOCK
);