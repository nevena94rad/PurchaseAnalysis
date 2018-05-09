USE [PED]
GO
/****** Object:  Table [dbo].[Parameters]    Script Date: 5/9/2018 11:40:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Parameters](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ProcessingStart] [datetime] NOT NULL,
	[ProcessingEnd] [datetime] NULL,
	[ProcessingStatus] [nvarchar](50) NULL,
	[ProcessingError] [nvarchar](250) NULL,
	[ProcessingParameters] [nvarchar](1500) NOT NULL,
	[ProcessingPreparer] [nvarchar](150) NOT NULL,
	[ProcessingCalculator] [nvarchar](150) NOT NULL,
	[UseGpi] [bit] NOT NULL,
 CONSTRAINT [PK_Parameters] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
