IF OBJECT_ID ('[RolesAuditTrigger]') IS NOT NULL
    DROP TRIGGER [RolesAuditTrigger];
GO

CREATE TRIGGER [RolesAuditTrigger] 
ON [Roles]
AFTER INSERT, UPDATE, DELETE
AS 
BEGIN
	SET NOCOUNT ON;

	DECLARE @action as char(1);
	DECLARE @Name NVARCHAR(30);
	DECLARE @OldValue NVARCHAR(30);
	DECLARE @NewValue NVARCHAR(30);

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
	BEGIN
		SELECT @Name = [Name], @NewValue = inserted.[Name], @OldValue = inserted.[Name]
		FROM inserted

		INSERT INTO [dbo].[RolesAudit] (Type, FieldName, OldValue, NewValue)
		VALUES (@action, @Name, @OldValue, @NewValue)
	END 
	ELSE 
	IF @action = 'U' 
	BEGIN 
		SELECT @Name = inserted.[Name], @OldValue = deleted.[Name], @NewValue = inserted.[Name] 
		FROM inserted, deleted
		WHERE inserted.Id = deleted.Id

		INSERT INTO [dbo].[RolesAudit] (Type, FieldName, OldValue, NewValue)
		VALUES (@action, @Name, @OldValue, @NewValue)
	END 
	ELSE 
	IF @action = 'D'
	BEGIN 
		SELECT @Name = [Name]
		FROM deleted

		INSERT INTO [dbo].[RolesAudit] (Type, FieldName, OldValue, NewValue)
		VALUES (@action, @Name, NULL, NULL)
	END
END
GO


--IF OBJECT_ID ('[UsersAuditTrigger]') IS NOT NULL
--    DROP TRIGGER [UsersAuditTrigger];
--GO

--CREATE TRIGGER [UsersAuditTrigger] 
--ON [Users]
--AFTER INSERT, UPDATE, DELETE
--AS 
--BEGIN
--	SET NOCOUNT ON;

--	DECLARE @action as char(1);
--	DECLARE @Name NVARCHAR(30);
--	DECLARE @OldValue NVARCHAR(30);
--	DECLARE @NewValue NVARCHAR(30);

--    SET @action = 'I';
--    IF EXISTS(SELECT * FROM DELETED)
--    BEGIN
--        SET @action = 
--            CASE
--                WHEN EXISTS(SELECT * FROM INSERTED) THEN 'U' 
--                ELSE 'D'     
--            END
--    END
--    ELSE 
--        IF NOT EXISTS(SELECT * FROM INSERTED) RETURN;

--	IF @action = 'I'
--	BEGIN
--		SELECT @Name = [Name], @NewValue = inserted.[Name], @OldValue = inserted.[Name]
--		FROM inserted

--		INSERT INTO [dbo].[UsersAudit] (Type, FieldName, OldValue, NewValue)
--		VALUES (@action, @Name, @OldValue, @NewValue)
--	END 
--	ELSE 
--	IF @action = 'U' 
--	BEGIN 
--		SELECT @Name = inserted.[Name], @OldValue = deleted.[Name], @NewValue = inserted.[Name] 
--		FROM inserted, deleted
--		WHERE inserted.Id = deleted.Id

--		INSERT INTO [dbo].[UsersAudit] (Type, FieldName, OldValue, NewValue)
--		VALUES (@action, @Name, @OldValue, @NewValue)
--	END 
--	ELSE 
--	IF @action = 'D'
--	BEGIN 
--		SELECT @Name = [Name]
--		FROM deleted

--		INSERT INTO [dbo].[UsersAudit] (Type, FieldName, OldValue, NewValue)
--		VALUES (@action, @Name, NULL, NULL)
--	END
--END
--GO
