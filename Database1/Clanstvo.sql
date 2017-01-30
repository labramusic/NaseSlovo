CREATE TABLE [dbo].[Clanstvo]
(
	[ClanstvoID] INT NOT NULL,
	[KorisnikID] INT NOT NULL UNIQUE, 
    [DatumPlat] DATE NOT NULL, 
    [DatumIstek] DATE NOT NULL, 
    CONSTRAINT [ClanstvoKorisnik] FOREIGN KEY ([KorisnikID]) REFERENCES [Korisnik]([KorisnikID]) ON DELETE CASCADE,
	CONSTRAINT [PKClanstvoKorisnik] PRIMARY KEY (ClanstvoID, KorisnikID)
)
