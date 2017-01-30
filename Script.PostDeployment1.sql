MERGE INTO Korisnik AS Target 
USING (VALUES 
        (1, 'Ivan', 'Ivanić', '1/1/2001', 'ivan@fer.hr', 'M')
) 
AS Source (KorisnikID, Ime, Prezime, DatumRod, Email, Spol)
ON Target.KorisnikID = Source.KorisnikID 
WHEN NOT MATCHED BY TARGET THEN
INSERT (Ime, Prezime, DatumRod, Email, Spol)
VALUES (Ime, Prezime, DatumRod, Email, Spol);