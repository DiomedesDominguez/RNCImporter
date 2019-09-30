/*    ==Scripting Parameters==

    Source Server Version : SQL Server 2016 (13.0.4001)
    Source Database Engine Edition : Microsoft SQL Server Enterprise Edition
    Source Database Engine Type : Standalone SQL Server

    Target Server Version : SQL Server 2016
    Target Database Engine Edition : Microsoft SQL Server Enterprise Edition
    Target Database Engine Type : Standalone SQL Server
*/

USE [dbDGII]
GO

/****** Object:  Table [dbo].[mRNCs]    Script Date: 8/30/2017 4:39:46 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[mRNCs](
	[RNC] [varchar](13) NULL,
	[RazonSocial] [varchar](70) NULL,
	[NombreComercial] [varchar](70) NULL,
	[Categoria] [varchar](40) NULL,
	[CalleAvenida] [varchar](70) NULL,
	[Numero] [varchar](10) NULL,
	[Sector] [varchar](70) NULL,
	[Telefono] [varchar](10) NULL,
	[Registrado] [varchar](10) NULL,
	[Estado] [varchar](20) NULL,
	[RegimenPagos] [varchar](10) NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[mRNCs] ADD  CONSTRAINT [DF_mRNCs_RazonSocial]  DEFAULT ('') FOR [RazonSocial]
GO

ALTER TABLE [dbo].[mRNCs] ADD  CONSTRAINT [DF_mRNCs_NombreComercial]  DEFAULT ('') FOR [NombreComercial]
GO

ALTER TABLE [dbo].[mRNCs] ADD  CONSTRAINT [DF_mRNCs_Categoria]  DEFAULT ('') FOR [Categoria]
GO

ALTER TABLE [dbo].[mRNCs] ADD  CONSTRAINT [DF_mRNCs_CalleAvenida]  DEFAULT ('') FOR [CalleAvenida]
GO

ALTER TABLE [dbo].[mRNCs] ADD  CONSTRAINT [DF_mRNCs_Numero]  DEFAULT ('') FOR [Numero]
GO

ALTER TABLE [dbo].[mRNCs] ADD  CONSTRAINT [DF_mRNCs_Telefono]  DEFAULT ('') FOR [Telefono]
GO

ALTER TABLE [dbo].[mRNCs] ADD  CONSTRAINT [DF_mRNCs_Estado]  DEFAULT ('') FOR [Estado]
GO


