CREATE TRIGGER trg_AracKirala
ON Kiralamalar
AFTER INSERT
AS
BEGIN
    DECLARE @AracID INT
    SELECT @AracID = AracID FROM inserted
    
    UPDATE Araclar 
    SET Durum = 'Kirada' 
    WHERE AracID = @AracID
END;