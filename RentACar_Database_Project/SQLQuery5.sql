-- Hangi müşteri hangi aracı kaç günlüğüne kiraladı?
SELECT 
    M.Ad + ' ' + M.Soyad AS [Müşteri Bilgisi],
    A.Marka + ' ' + A.Model AS [Kiralanan Araç],
    DATEDIFF(day, K.AlisTarihi, K.TeslimTarihi) AS [Gün Sayısı],
    K.ToplamTutar AS [Ödenen Ücret]
FROM Kiralamalar K
JOIN Musteriler M ON K.MusteriID = M.MusteriID
JOIN Araclar A ON K.AracID = A.AracID;