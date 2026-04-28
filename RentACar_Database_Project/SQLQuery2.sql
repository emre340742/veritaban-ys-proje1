USE RentACarDB;
GO

-- 1. Araçlar Tablosuna 5 Yeni Kayıt Ekleme
INSERT INTO Araclar (Plaka, Marka, Model, Yil, GunlukUcret) VALUES 
('34ABC123', 'Toyota', 'Corolla', 2022, 1200.00),
('06ANK006', 'Volkswagen', 'Passat', 2021, 1500.00),
('35IZM350', 'Renault', 'Megane', 2020, 1000.00),
('16BUR161', 'BMW', '320i', 2023, 2500.00),
('07ANT070', 'Fiat', 'Egea', 2022, 900.00);

-- 2. Müşteriler (5 Kayıt)
INSERT INTO Musteriler (TCKimlik, Ad, Soyad, Telefon, EhliyetNo) VALUES 
('11111111111', 'Ahmet', 'Yılmaz', '05551112233', 'EHL001'),
('22222222222', 'Mehmet', 'Öztürk', '05321112233', 'EHL002'),
('33333333333', 'Ayşe', 'Kaya', '05442223344', 'EHL003'),
('44444444444', 'Can', 'Demir', '05053334455', 'EHL004'),
('55555555555', 'Zeynep', 'Çelik', '05304445566', 'EHL005');

-- 3. Kiralamalar (5 Kayıt)
INSERT INTO Kiralamalar (AracID, MusteriID, AlisTarihi, TeslimTarihi, ToplamTutar) VALUES 
(1, 1, '2023-11-01', '2023-11-05', 4800.00),
(2, 2, '2023-11-05', '2023-11-08', 4500.00),
(3, 3, '2023-11-10', '2023-11-15', 5000.00),
(4, 4, '2023-11-12', '2023-11-14', 5000.00),
(5, 5, '2023-11-15', '2023-11-20', 4500.00);