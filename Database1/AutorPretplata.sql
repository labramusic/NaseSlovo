CREATE TABLE [dbo].[AutorPretplata]
(
	[KorisnikID] INT NOT NULL,
	[AutorID] INT NOT NULL,
	PRIMARY KEY ([KorisnikID], [AutorID]), 
	CONSTRAINT [PretplataAutorKorisnik] FOREIGN KEY ([AutorID]) REFERENCES [Korisnik]([KorisnikID]),
	CONSTRAINT [PretplataKorisnikAutor] FOREIGN KEY ([KorisnikID]) REFERENCES [Korisnik]([KorisnikID]) ON DELETE CASCADE
)
