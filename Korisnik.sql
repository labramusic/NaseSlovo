CREATE TABLE [dbo].[Korisnik]
(
	[KorisnikID] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Ime] NVARCHAR(50) NOT NULL, 
    [Prezime] NVARCHAR(50) NOT NULL, 
    [DatumRod] DATE NULL, 
    [Email] VARCHAR(50) NOT NULL, 
    [Spol] NVARCHAR(50) NULL
)
