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

IF OBJECT_ID('[Users.GetUserByUsernameAndPassword]') IS NOT NULL
	DROP PROCEDURE [Users.GetUserByUsernameAndPassword]
GO

CREATE PROCEDURE [Users.GetUserByUsernameAndPassword]
	@username NVARCHAR(30),
	@password NVARCHAR(20)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT *
	FROM [Users] 
	INNER JOIN [Roles] ON [Roles].[Id] = [Users].[RoleId]
	INNER JOIN [Employees] ON [Roles].[Id] = [Users].[RoleId]
	INNER JOIN [EmployeesPositions] ON [EmployeesPositions].[Id] = [Employees].[EmployeesPositionId]
	WHERE [Username] = @username AND [Password] = @password
END
GO

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

IF OBJECT_ID('Clients.GetClients]') IS NOT NULL
	DROP PROCEDURE [Clients.GetClients]
GO

CREATE PROCEDURE [Clients.GetClients]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM [Clients]
END
GO