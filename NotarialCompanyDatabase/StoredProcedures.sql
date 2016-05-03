--IF OBJECT_ID('[Deals.AddDeal]') IS NOT NULL
--	DROP PROCEDURE [Deals.AddDeal]
--GO
--IF OBJECT_ID('[Clients.AddClient]') IS NOT NULL
--	DROP PROCEDURE [Clients.AddClient]
--GO
--IF OBJECT_ID('[Employees.AddEmployee]') IS NOT NULL
--	DROP PROCEDURE [Employees.AddEmployee]
--GO
--IF OBJECT_ID('[Users.AddUser]') IS NOT NULL
--	DROP PROCEDURE [Users.AddUser]
--GO

--CREATE PROCEDURE [Deals.AddDeal]
--	@description nvarchar,
--	@employeeId int,
--	@clientId int,
--	@isPaid bit,
--	@dateTime datetime,
--	@servicesIds varchar(max)
--AS
--	DECLARE @dealId int, @basePrice money, @totalPrice money;
--	SET NOCOUNT ON;

--	INSERT INTO [dbo].[Deals] ([Description], [EmployeeId], [ClientId])
--	VALUES (@description, @employeeId, @clientId)

--	SET @dealId = CAST(SCOPE_IDENTITY() AS int)

--	INSERT INTO [dbo].[DealService] ([DealId], [ServiceId])
--	SELECT @dealId, s.Id
--	FROM [dbo].[Split](@servicesIds) AS s

--	SELECT @basePrice = SUM(s.[Cost]) 
--	FROM [DealService] AS ds
--		INNER JOIN [Services] AS s ON s.[Id] = ds.[ServiceId]
--	WHERE ds.[DealId] = @dealId

--	SELECT @totalPrice = (ep.[Commission] * @basePrice) / 100 + @basePrice
--	FROM [Employees] AS e
--		INNER JOIN [EmployeesPositions] AS ep ON ep.[Id] = e.[EmployeesPositionId]
--	WHERE e.[Id] = @employeeId

--	INSERT INTO [dbo].[Bill] ([Id], [BasePrice], [TotalPrice], [IsPaid], [DateTime])
--	VALUES (@dealId, @basePrice, @totalPrice, @isPaid, @dateTime)
--RETURN 0
--GO


--CREATE PROCEDURE [Clients.AddClient]
--	@firstName NVARCHAR(30),
--	@secondName NVARCHAR(30),
--	@middleName NVARCHAR(30),
--	@occupation NVARCHAR(30),
--	@address NVARCHAR(150),
--	@phoneNumber NVARCHAR(15)
--AS
--	INSERT INTO [dbo].[Clients] (FirstName, SecondName, MiddleName, Occupation, Address, PhoneNumber)
--	VALUES (@firstName,	@secondName, @middleName, @occupation, @address, @phoneNumber)
--GO


--CREATE PROCEDURE [Employees.AddEmployee]
--    @firstName NVARCHAR(30),
--    @lastName NVARCHAR(30),
--    @middleName NVARCHAR(30),
--    @address NVARCHAR(250),
--    @phoneNumber NVARCHAR(15),
--    @employmentDate DATE,
--    @employeesPositionId INT
--AS
--	INSERT INTO [dbo].[Employees] ([FirstName], [LastName], [MiddleName], [Address], [PhoneNumber], [EmploymentDate], [EmployeesPositionId])
--	VALUES (@firstName,	@lastName, @middleName, @address, @phoneNumber, @employmentDate, @employeesPositionId)
--GO


--CREATE PROCEDURE [Users.AddUser]
--	@username NVARCHAR(30),
--    @password NVARCHAR(20),
--    @salt NVARCHAR(20),
--    @roleId INT,
--    @employeeId INT
--AS
--	INSERT INTO [dbo].[Users] ([Username], [Password], [Salt], [RoleId], [EmployeeId])
--	VALUES (@username,	@password, @salt, @roleId, @employeeId)
--GO
------------------------------------------------Users------------------------------------------------

IF OBJECT_ID('[Users.UsersGetUserByUsername]') IS NOT NULL
	DROP PROCEDURE [Users.UsersGetUserByUsername]
GO

CREATE PROCEDURE [Users.UsersGetUserByUsername]
	@username NVARCHAR(30)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM [Users] 
	INNER JOIN [Roles] ON [Roles].[Id] = [Users].[RoleId]
	INNER JOIN [Employees] ON [Employees].[Id] = [Users].[EmployeeId]
	INNER JOIN [EmployeesPositions] ON [EmployeesPositions].[Id] = [Employees].[EmployeesPositionId]
	WHERE [Username] = @username
END
GO

IF OBJECT_ID('[Users.GetUsers]') IS NOT NULL
	DROP PROCEDURE [Users.GetUsers]
GO

CREATE PROCEDURE [Users.GetUsers]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM [Users] 
	INNER JOIN [Roles] ON [Roles].[Id] = [Users].[RoleId]
	INNER JOIN [Employees] ON [Employees].[Id] = [Users].[EmployeeId]
	INNER JOIN [EmployeesPositions] ON [EmployeesPositions].[Id] = [Employees].[EmployeesPositionId]
END
GO

IF OBJECT_ID('[Users.CreateOrUpdateUser]') IS NOT NULL
	DROP PROCEDURE [Users.CreateOrUpdateUser]
GO

CREATE PROCEDURE [Users.CreateOrUpdateUser]
	@id INT,
    @username NVARCHAR(30),
    @password NVARCHAR(MAX),
    @salt NVARCHAR(MAX),
    @roleId INT,
    @employeeId INT
AS
	IF @id = 0
	BEGIN
		INSERT INTO [Users]
		VALUES (@username, @password, @salt, @roleId, @employeeId)
		RETURN
	END

	UPDATE [Users] 
	SET [Username] = @username, [Password] = @password, [Salt] = @salt, [RoleId] = @roleId, [EmployeeId] = @employeeId
	WHERE [Users].[Id] = @id
GO

IF OBJECT_ID('[Users.RemoveUser]') IS NOT NULL
	DROP PROCEDURE [Users.RemoveUser]
GO

CREATE PROCEDURE [Users.RemoveUser]
	@id INT
AS
	DELETE FROM [dbo].[Users]
	WHERE [Id] = @id
GO

------------------------------------------------Services------------------------------------------------

IF OBJECT_ID('[Services.GetServices]') IS NOT NULL
	DROP PROCEDURE [Services.GetServices]
GO

CREATE PROCEDURE [Services.GetServices]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM [Services]
END
GO

IF OBJECT_ID('[Services.CreateOrUpdateService]') IS NOT NULL
	DROP PROCEDURE [Services.CreateOrUpdateService]
GO


CREATE PROCEDURE [Services.CreateOrUpdateService]
	@id INT,
    @name NVARCHAR(100),
    @description NVARCHAR(500),
    @cost MONEY
AS
	IF @id = 0
	BEGIN
		INSERT INTO [Services]
		VALUES (@name, @description, @cost)
		RETURN
	END

	UPDATE [Services] 
	SET [Name] = @name, [Description] = @description, [Cost] = @cost
	WHERE [Services].[Id] = @id
GO


IF OBJECT_ID('[Services.RemoveService]') IS NOT NULL
	DROP PROCEDURE [Services.RemoveService]
GO

CREATE PROCEDURE [Services.RemoveService]
	@id INT
AS
	DELETE FROM [dbo].[Services]
	WHERE [Id] = @id
GO

------------------------------------------------Clients------------------------------------------------

IF OBJECT_ID('[Clients.CreateOrUpdateClient]') IS NOT NULL
	DROP PROCEDURE [Clients.CreateOrUpdateClient]
GO


CREATE PROCEDURE [Clients.CreateOrUpdateClient]
	@id INT,
	@firstName NVARCHAR(30),
	@secondName NVARCHAR(30),
	@middleName NVARCHAR(30),
	@occupation NVARCHAR(30),
	@address NVARCHAR(150),
	@phoneNumber NVARCHAR(15)
AS
	IF @id = 0
	BEGIN
		INSERT INTO [Clients]
		VALUES (@firstName,	@secondName, @middleName, @occupation, @address, @phoneNumber)
		RETURN
	END

	UPDATE [Clients] 
	SET [FirstName] = @firstName, [SecondName] = @secondName, [MiddleName] = @middleName, [Occupation] = @occupation, 
		[Address] = @address, [PhoneNumber] = @phoneNumber
	WHERE [Clients].[Id] = @id
GO

IF OBJECT_ID('[Clients.GetClients]') IS NOT NULL
	DROP PROCEDURE [Clients.GetClients]
GO

CREATE PROCEDURE [Clients.GetClients]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM [Clients]
END
GO

IF OBJECT_ID('[Clients.RemoveClient]') IS NOT NULL
	DROP PROCEDURE [Clients.RemoveClient]
GO

CREATE PROCEDURE [Clients.RemoveClient]
	@id INT
AS
	DELETE FROM [dbo].[Clients]
	WHERE [Id] = @id
GO

------------------------------------------------Roles------------------------------------------------

IF OBJECT_ID('[Roles.GetRoles]') IS NOT NULL
	DROP PROCEDURE [Roles.GetRoles]
GO

CREATE PROCEDURE [Roles.GetRoles]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM [Roles]
END
GO

------------------------------------------------Employees------------------------------------------------

IF OBJECT_ID('[Employees.CreateOrUpdateEmployee]') IS NOT NULL
	DROP PROCEDURE [Employees.CreateOrUpdateEmployee]
GO


CREATE PROCEDURE [Employees.CreateOrUpdateEmployee]
	@id INT,
	@firstName NVARCHAR(30),
	@lastName NVARCHAR(30),
	@middleName NVARCHAR(30),
	@address NVARCHAR(250),
	@phoneNumber NVARCHAR(15),
	@employmentDate DATE,
	@employeesPositionId INT
AS
	IF @id = 0
	BEGIN
		INSERT INTO [Employees]
		VALUES (@firstName,	@lastName, @middleName, @address, @phoneNumber, @employmentDate, @employeesPositionId)
		RETURN
	END

	UPDATE [Employees] 
	SET [FirstName] = @firstName, [LastName] = @lastName, [MiddleName] = @middleName, [Address] = @address, 
		[PhoneNumber] = @phoneNumber, [EmploymentDate] = @employmentDate, [EmployeesPositionId] = @employeesPositionId
	WHERE [Employees].[Id] = @id
GO

IF OBJECT_ID('[Employees.GetEmployees]') IS NOT NULL
	DROP PROCEDURE [Employees.GetEmployees]
GO

CREATE PROCEDURE [Employees.GetEmployees]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM [Employees]
	INNER JOIN [EmployeesPositions] ON [EmployeesPositions].[Id] = [Employees].[EmployeesPositionId]
END
GO

IF OBJECT_ID('[Employees.RemoveEmployee]') IS NOT NULL
	DROP PROCEDURE [Employees.RemoveEmployee]
GO

CREATE PROCEDURE [Employees.RemoveEmployee]
	@id INT
AS
	DELETE FROM [dbo].[Employees]
	WHERE [Id] = @id
GO

------------------------------------------------EmployeesPositions------------------------------------------------

IF OBJECT_ID('[EmployeesPositions.CreateOrUpdateEmployeesPosition]') IS NOT NULL
	DROP PROCEDURE [EmployeesPositions.CreateOrUpdateEmployeesPosition]
GO


CREATE PROCEDURE [EmployeesPositions.CreateOrUpdateEmployeesPosition]
	@id INT,
	@position NVARCHAR(30),
	@salary MONEY,
	@commission INT
AS
	IF @id = 0
	BEGIN
		INSERT INTO [EmployeesPositions]
		VALUES (@position,	@salary, @commission)
		RETURN
	END

	UPDATE [EmployeesPositions] 
	SET [Position] = @position, [Salary] = @salary, [Commission] = @commission
	WHERE [EmployeesPositions].[Id] = @id
GO

IF OBJECT_ID('[EmployeesPositions.GetEmployeesPosition]') IS NOT NULL
	DROP PROCEDURE [EmployeesPositions.GetEmployeesPosition]
GO

CREATE PROCEDURE [EmployeesPositions.GetEmployeesPosition]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM [EmployeesPositions]
END
GO

------------------------------------------------Deals------------------------------------------------

IF OBJECT_ID('[Deals.CreateOrUpdateDeal]') IS NOT NULL
	DROP PROCEDURE [Deals.CreateOrUpdateDeal]
GO

CREATE PROCEDURE [Deals.CreateOrUpdateDeal]
	@id int,
	@description nvarchar(100),
	@employeeId int,
	@clientId int,
	@isPaid bit,
	@basePrice money,
	@totalPrice money,
	@dateTime datetime,
	@servicesIds varchar(max)
AS
	IF @id = 0
	BEGIN
		INSERT INTO [dbo].[Deals] ([Description], [EmployeeId], [ClientId])
		VALUES (@description, @employeeId, @clientId)

		SET @id = CAST(SCOPE_IDENTITY() AS int)

		INSERT INTO [dbo].[DealService] ([DealId], [ServiceId])
		SELECT @id, s.Id
		FROM [dbo].[Split](@servicesIds) AS s

		INSERT INTO [dbo].[Bill] ([Id], [BasePrice], [TotalPrice], [IsPaid], [DateTime])
		VALUES (@id, @basePrice, @totalPrice, @isPaid, @dateTime)
		RETURN
	END

	UPDATE [Deals] 
	SET [Description] = @description, [EmployeeId] = @employeeId, [ClientId] = @clientId
	WHERE [Deals].[Id] = @id

	DELETE FROM [DealService]
	WHERE [DealService].[DealId] = @id
	
	INSERT INTO [dbo].[DealService] ([DealId], [ServiceId])
	SELECT @id, s.Id
	FROM [dbo].[Split](@servicesIds) AS s

	UPDATE [Bill] 
	SET [BasePrice] = @basePrice, [TotalPrice] = @totalPrice, [IsPaid] = @isPaid, [DateTime] = @dateTime
	WHERE [Bill].[Id] = @id
GO

IF OBJECT_ID('[Deals.GetDeals]') IS NOT NULL
	DROP PROCEDURE [Deals.GetDeals]
GO

CREATE PROCEDURE [Deals.GetDeals]
AS
	SET NOCOUNT ON;
	SELECT * FROM [Deals] 
	INNER JOIN [Bill] ON [Bill].[Id] = [Deals].[Id]
	INNER JOIN [Employees] ON [Employees].[Id] = [Deals].[EmployeeId]
	INNER JOIN [Clients] ON [Clients].[Id] = [Deals].[ClientId]
GO

IF OBJECT_ID('[Services.GetServicesByDealId]') IS NOT NULL
	DROP PROCEDURE [Services.GetServicesByDealId]
GO

CREATE PROCEDURE [Services.GetServicesByDealId]
	@id INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT [ServiceId] FROM [DealService]
	WHERE [DealService].[DealId] = @id
END
GO

IF OBJECT_ID('[EmployeesPositions.RemoveEmployeesPosition]') IS NOT NULL
	DROP PROCEDURE [EmployeesPositions.RemoveEmployeesPosition]
GO

CREATE PROCEDURE [EmployeesPositions.RemoveEmployeesPosition]
	@id INT
AS
	DELETE FROM [dbo].[EmployeesPositions]
	WHERE [Id] = @id
GO