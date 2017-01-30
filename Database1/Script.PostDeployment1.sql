MERGE INTO Korisnik AS Target 
USING (VALUES 
        (1, 'Ivan', 'Ivanić', '1/1/2001', 'ivan@fer.hr', 'M', 'ivanic')
) 
AS Source (KorisnikID, Ime, Prezime, DatumRod, Email, Spol, KorisnickoIme)
ON Target.KorisnikID = Source.KorisnikID 
WHEN NOT MATCHED BY TARGET THEN
INSERT (Ime, Prezime, DatumRod, Email, Spol, KorisnickoIme)
VALUES (Ime, Prezime, DatumRod, Email, Spol, KorisnickoIme);