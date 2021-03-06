USE [PED]
GO
/****** Object:  Table [dbo].[CustomerModel]    Script Date: 06-Mar-18 14:21:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerModel](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CustNo] [nvarchar](50) NOT NULL,
	[Parameters_ID] [int] NOT NULL,
	[Model] [varchar](150) NOT NULL,
 CONSTRAINT [PK_CustomerModel] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Parameters]    Script Date: 06-Mar-18 14:21:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Parameters](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ProcessingDate] [int] NOT NULL,
	[CustRecency] [int] NOT NULL,
	[PercentageCutOff] [float] NOT NULL,
	[CountCutOff] [int] NOT NULL,
 CONSTRAINT [PK_Parameters] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PurchaseHistory]    Script Date: 06-Mar-18 14:21:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PurchaseHistory](
	[CustNo] [varchar](50) NOT NULL,
	[ItemNo] [varchar](50) NOT NULL,
	[InvDate] [int] NOT NULL,
	[InvQty] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PurchasePeriods]    Script Date: 06-Mar-18 14:21:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PurchasePeriods](
	[CustNo] [varchar](50) NOT NULL,
	[ItemNo] [varchar](50) NOT NULL,
	[InvDateCurr] [datetime] NOT NULL,
	[InvDatePrior] [datetime] NOT NULL,
	[PurchasePeriod] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PurchasePrediction]    Script Date: 06-Mar-18 14:21:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PurchasePrediction](
	[key] [int] IDENTITY(1,1) NOT NULL,
	[CustNo] [nvarchar](50) NOT NULL,
	[ItemNo] [nvarchar](50) NOT NULL,
	[PredictedValue] [float] NOT NULL,
	[Model] [int] NOT NULL,
 CONSTRAINT [PK_PurchasePrediction] PRIMARY KEY CLUSTERED 
(
	[key] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CustomerModel]  WITH CHECK ADD  CONSTRAINT [FK_CustomerModel_Parameters] FOREIGN KEY([Parameters_ID])
REFERENCES [dbo].[Parameters] ([ID])
GO
ALTER TABLE [dbo].[CustomerModel] CHECK CONSTRAINT [FK_CustomerModel_Parameters]
GO
ALTER TABLE [dbo].[PurchasePrediction]  WITH CHECK ADD  CONSTRAINT [FK_PurchasePrediction_CustomerModel] FOREIGN KEY([Model])
REFERENCES [dbo].[CustomerModel] ([ID])
GO
ALTER TABLE [dbo].[PurchasePrediction] CHECK CONSTRAINT [FK_PurchasePrediction_CustomerModel]
GO
