﻿CREATE TABLE [dbo].[Vrsta]
(
	[VrstaID] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[Naziv] NVARCHAR(50) NOT NULL UNIQUE
)
