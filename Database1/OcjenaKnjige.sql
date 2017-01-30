CREATE TABLE [dbo].[OcjenaKnjige]
(
	[KnjigaID] INT NOT NULL , 
    [KorisnikID] INT NOT NULL, 
    [Ocjena] INT NOT NULL, 
    PRIMARY KEY ([KnjigaID], [KorisnikID]), 
	CONSTRAINT [OcjenaKnjigeKnjiga] FOREIGN KEY ([KnjigaID]) REFERENCES [Knjiga]([KnjigaID]) ON DELETE CASCADE, 
    CONSTRAINT [OcjenaKnjigeKorisnik] FOREIGN KEY ([KorisnikID]) REFERENCES [Korisnik]([KorisnikID]) ON DELETE CASCADE


)
