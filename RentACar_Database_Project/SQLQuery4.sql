CREATE VIEW View_AktifKiralamalar AS
SELECT 
    A.Plaka, A.Marka, A.Model, 
    M.Ad + ' ' + M.Soyad AS Musteri,
    K.AlisTarihi, K.TeslimTarihi, K.ToplamTutar
FROM Kiralamalar K
JOIN Araclar A ON K.AracID = A.AracID
JOIN Musteriler M ON K.MusteriID = M.MusteriID;
