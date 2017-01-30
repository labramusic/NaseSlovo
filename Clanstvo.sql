CREATE TABLE [dbo].[Clanstvo]
(
	[ClanstvoID] INT NOT NULL,
	[KorisnikID] INT NOT NULL, 
    [DatumPlat] DATE NOT NULL, 
    [DatumIstek] DATE NOT NULL, 
    CONSTRAINT [ClanstvoKorisnik] FOREIGN KEY ([KorisnikID]) REFERENCES [Korisnik]([KorisnikID]),
	CONSTRAINT [PKClanstvoKorisnik] PRIMARY KEY (ClanstvoID, KorisnikID)
)
