CREATE TABLE [dbo].[Knjiga]
(
	[KnjigaID] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Autori] NVARCHAR(50) NOT NULL, 
    [Naslov] NVARCHAR(50) NOT NULL, 
    [Signatura] NVARCHAR(50) NOT NULL, 
    [Izdavac] NVARCHAR(50) NOT NULL, 
    [Godina] SMALLINT NOT NULL, 
    [VrstaID] INT NOT NULL, 
    [SrednjaOcjena] FLOAT NOT NULL, 
    CONSTRAINT [JedinstvenaSignatura] UNIQUE(Signatura),
	CONSTRAINT [KnjigaVrsta] FOREIGN KEY ([VrstaID]) REFERENCES [Vrsta]([VrstaID]),
)
