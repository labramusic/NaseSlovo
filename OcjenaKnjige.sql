CREATE TABLE [dbo].[OcjenaKnjige]
(
	[KnjigaID] INT NOT NULL , 
    [KorisnikID] INT NOT NULL, 
    [Ocjena] INT NOT NULL, 
    PRIMARY KEY ([KnjigaID], [KorisnikID]), 
	CONSTRAINT [OcjenaKnjigeKnjiga] FOREIGN KEY ([KnjigaID]) REFERENCES [Knjiga]([KnjigaID]), 
    CONSTRAINT [OcjenaKnjigeKorisnik] FOREIGN KEY ([KorisnikID]) REFERENCES [Korisnik]([KorisnikID])


)
