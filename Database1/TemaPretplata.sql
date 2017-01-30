CREATE TABLE [dbo].[TemaPretplata]
(
	[KorisnikID] INT NOT NULL,
	[TemaID] INT NOT NULL,
	PRIMARY KEY ([KorisnikID], [TemaID]), 
	CONSTRAINT [PretplataTemaKorisnik] FOREIGN KEY ([TemaID]) REFERENCES [Tema]([TemaID]) ON DELETE CASCADE,
	CONSTRAINT [PretplataKorisnikTema] FOREIGN KEY ([KorisnikID]) REFERENCES [Korisnik]([KorisnikID]) ON DELETE CASCADE
)
