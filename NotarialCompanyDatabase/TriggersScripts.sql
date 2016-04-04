IF OBJECT_ID ('[RolesTrigger]') IS NOT NULL
    DROP TRIGGER [RolesTrigger];
GO

CREATE TRIGGER [RolesTrigger] 
ON [Roles]
AFTER INSERT, UPDATE, DELETE
AS 
BEGIN
	SET NOCOUNT ON;

	DECLARE @action as char(1);
	DECLARE @outPath NVARCHAR(100) = 'D:\Logs\AuditNotarialCompany.txt';
	DECLARE @outText NVARCHAR(100);
	DECLARE @query NVARCHAR(1000);

    SET @action = 'I';
    IF EXISTS(SELECT * FROM DELETED)
    BEGIN
        SET @action = 
            CASE
                WHEN EXISTS(SELECT * FROM INSERTED) THEN 'U' 
                ELSE 'D'     
            END
    END
    ELSE 
        IF NOT EXISTS(SELECT * FROM INSERTED) RETURN;

	IF @action = 'I'
		SELECT @outText = 'inserted [id=' + CAST([Id] AS varchar) + ', Name= ' + [Name] + '] ' + CAST(GETDATE() AS varchar)
		FROM inserted
	IF @action = 'U'
		SELECT @outText = 'updated [id=' + CAST(inserted.[Id] AS varchar) + ', Name=' + deleted.[Name] + '(old) ' + inserted.[Name] + '(new)] ' + CAST(GETDATE() AS varchar)
		FROM inserted, deleted
		WHERE inserted.Id = deleted.Id
	IF @action = 'D'
		SELECT @outText = 'deleted [id=' + CAST([Id] AS varchar) + ', Name= ' + [Name] + '] ' + CAST(GETDATE() AS varchar)
		FROM deleted

	SET @query = 'master..xp_cmdshell ''echo ' + @outText + ' >> ' + @outPath + '' 
	EXEC (@query)
END
GO
