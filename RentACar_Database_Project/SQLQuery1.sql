-- 1. Veri Tabanı Oluşturma
CREATE DATABASE RentACarDB;
GO
USE RentACarDB;
GO

-- 2. Araçlar Tablosu
CREATE TABLE Araclar (
    AracID INT PRIMARY KEY IDENTITY(1,1),
    Plaka VARCHAR(15) UNIQUE NOT NULL,
    Marka VARCHAR(50) NOT NULL,
    Model VARCHAR(50) NOT NULL,
    Yil INT,
    GunlukUcret DECIMAL(10,2) NOT NULL,
    Durum VARCHAR(20) DEFAULT 'Musait' -- Musait, Kirada, Bakimda
);

-- 3. Müşteriler Tablosu
CREATE TABLE Musteriler (
    MusteriID INT PRIMARY KEY IDENTITY(1,1),
    TCKimlik VARCHAR(11) UNIQUE NOT NULL,
    Ad VARCHAR(50) NOT NULL,
    Soyad VARCHAR(50) NOT NULL,
    Telefon VARCHAR(15),
    EhliyetNo VARCHAR(20) UNIQUE
);

-- 4. Kiralamalar Tablosu (İlişkisel Tablo)
CREATE TABLE Kiralamalar (
    KiraID INT PRIMARY KEY IDENTITY(1,1),
    AracID INT FOREIGN KEY REFERENCES Araclar(AracID),
    MusteriID INT FOREIGN KEY REFERENCES Musteriler(MusteriID),
    AlisTarihi DATE NOT NULL,
    TeslimTarihi DATE NOT NULL,
    ToplamTutar DECIMAL(10,2)
);
