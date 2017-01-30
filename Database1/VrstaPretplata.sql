CREATE TABLE [dbo].[VrstaPretplata]
(
	[KorisnikID] INT NOT NULL,
	[VrstaID] INT NOT NULL,
	PRIMARY KEY ([KorisnikID], [VrstaID]), 
	CONSTRAINT [PretplataVrstaKorisnik] FOREIGN KEY ([VrstaID]) REFERENCES [Vrsta]([VrstaID]) ON DELETE CASCADE,
	CONSTRAINT [PretplataKorisnikVrsta] FOREIGN KEY ([KorisnikID]) REFERENCES [Korisnik]([KorisnikID]) ON DELETE CASCADE
)
