USE [Currency]
GO

/****** Object:  Table [dbo].[ExchangeRateHistoryDetails]    Script Date: 6/7/2022 13:24:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ExchangeRateHistoryDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ExchangeRateHistoryId] [int] NOT NULL,
	[CurrencyCode] [char](3) NOT NULL,
	[ExchangeRate] [money] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ExchangeRateHistoryDetails]  WITH CHECK ADD FOREIGN KEY([ExchangeRateHistoryId])
REFERENCES [dbo].[ExchangeRateHistory] ([Id])
GO


