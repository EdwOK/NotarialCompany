IF OBJECT_ID('Split') is not null 
    DROP FUNCTION split
GO
 
CREATE FUNCTION dbo.Split(@String varchar(max))
RETURNS @SplittedValues TABLE
(
    Id varchar(50) PRIMARY KEY
)
AS
BEGIN
    DECLARE @SplitLength int, @Delimiter varchar(5)
    
    SET @Delimiter = ','
    
    WHILE LEN(@String) > 0
    BEGIN 
        SELECT @SplitLength = (CASE charindex(@Delimiter,@String) WHEN 0 THEN
            LEN(@String) ELSE charindex(@Delimiter,@String) -1 END)
 
        INSERT INTO @SplittedValues
        SELECT SUBSTRING(@String,1,@SplitLength) 
    
        SELECT @String = (CASE (LEN(@String) - @SplitLength) WHEN 0 THEN  ''
            ELSE RIGHT(@String, LEN(@String) - @SplitLength - 1) END)
    END 
RETURN  
END