CREATE TABLE [dbo].[OcjenaTeksta]
(
	[TekstID] INT NOT NULL , 
    [KorisnikID] INT NOT NULL, 
    [Ocjena] INT NOT NULL, 
    PRIMARY KEY ([TekstID], [KorisnikID]), 
    CONSTRAINT [OcjenaTekstaTekst] FOREIGN KEY ([TekstID]) REFERENCES [Tekst]([TekstID]) ON DELETE CASCADE, 
    CONSTRAINT [OcjenaTekstaKorisnik] FOREIGN KEY ([KorisnikID]) REFERENCES [Korisnik]([KorisnikID]) ON DELETE CASCADE

)
