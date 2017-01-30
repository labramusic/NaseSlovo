CREATE TABLE [dbo].[Tekst]
(
	[TekstID] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Sadrzaj] NVARCHAR(MAX) NOT NULL, 
    [Naslov] NVARCHAR(50) NOT NULL, 
    [AutorID] INT NOT NULL,
	[TemaID] INT NOT NULL, 
	[VrstaID] INT NOT NULL,
    [DatumVrijeme] DATETIME NOT NULL, 
    [SrednjaOcjena] FLOAT NOT NULL, 
	[BiltenID] INT DEFAULT NULL,
    CONSTRAINT [TekstKorisnik] FOREIGN KEY ([AutorID]) REFERENCES [Korisnik]([KorisnikID]),
	CONSTRAINT [TekstTema] FOREIGN KEY ([TemaID]) REFERENCES [Tema]([TemaID]),
	CONSTRAINT [TekstVrsta] FOREIGN KEY ([VrstaID]) REFERENCES [Vrsta]([VrstaID]),
	CONSTRAINT [TekstBilten] FOREIGN KEY ([BiltenID]) REFERENCES [Bilten]([BiltenID]) 
	ON DELETE SET NULL ON UPDATE CASCADE
)
