CREATE TABLE [dbo].[Komentar]
(
	[KomentarID] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
	[KorisnikID] INT NOT NULL, 
    [TekstID] INT NOT NULL, 
    [Sadrzaj] NVARCHAR(MAX) NOT NULL, 
    [DatumVrijeme] DATETIME NOT NULL, 
    CONSTRAINT [KomentarTekst] FOREIGN KEY ([TekstID]) REFERENCES [Tekst]([TekstID]) ON DELETE CASCADE, 
    CONSTRAINT [KomentarKorisnik] FOREIGN KEY ([KorisnikID]) REFERENCES [Korisnik]([KorisnikID]) ON DELETE CASCADE
)
