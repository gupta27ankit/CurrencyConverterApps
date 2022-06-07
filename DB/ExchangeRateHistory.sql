USE [Currency]
GO

/****** Object:  Table [dbo].[ExchangeRateHistory]    Script Date: 6/7/2022 13:24:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ExchangeRateHistory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ExchangeRateDate] [datetime] NOT NULL,
	[IsDownloadSuccess] [bit] NOT NULL,
	[DownloadTimeStamp] [datetime] NOT NULL,
	[DownloadedByUser] [varchar](50) NOT NULL,
	[BaseCurrency] [char](3) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


