-- Users Selects

SELECT * FROM [dbo].[Users]

SELECT [Users].[UserName], [Roles].[Name] AS Role
FROM [Users]
INNER JOIN [Roles] ON [Users].[RoleId] = [Roles].[Id]

SELECT [Users].[UserName], [Employees].[LastName], [Employees].[FirstName], [Employees].[PhoneNumber]
FROM [Users]
INNER JOIN [Employees] ON [Users].[EmployeeId] = [Employees].[Id]

SELECT [Users].[UserName]
FROM [Users]
INNER JOIN [Roles] ON [Users].[RoleId] = [Roles].[Id]
WHERE [Roles].[Name] = 'Admin'

SELECT [Users].[UserName]
FROM [Users]
INNER JOIN [Roles] ON [Users].[RoleId] = [Roles].[Id]
WHERE [Roles].[Name] = 'User'


-- Services Selects

SELECT * FROM [dbo].[Services]

SELECT * 
FROM [dbo].[Services]
WHERE [Cost] > 3000

SELECT [Name], [Cost]
FROM [Services]
ORDER BY [Cost] DESC;

SELECT *
FROM [Services]
LEFT JOIN [DealService] ON [DealService].[ServiceId] = [Services].[Id]

SELECT [Services].[Name], COUNT([ServiceId]) AS Count
FROM [Services]
LEFT JOIN [DealService] ON [DealService].[ServiceId] = [Services].[Id]
GROUP BY [Name]
ORDER BY Count DESC;


-- Roles Selects

SELECT * FROM [Roles]

SELECT [Roles].[Name], COUNT(*) AS RolesCount 
FROM [Roles]
INNER JOIN [Users] ON [Users].[RoleId] = [Roles].[Id]
GROUP BY [Name]


-- EmployeesPositions Selects

SELECT * FROM [EmployeesPositions]

SELECT [Position], [Salary], [Commission]
FROM [EmployeesPositions]
ORDER BY [Salary] DESC, [Commission] DESC

SELECT [Position], COUNT([Employees].[Id]) AS PositionCount
FROM [EmployeesPositions]
LEFT JOIN [Employees] ON [Employees].[EmployeesPositionId] = [EmployeesPositions].[Id]
GROUP BY [Position]

SELECT AVG([Salary]) AS AverageSalary
FROM [EmployeesPositions]

SELECT [Position], COUNT(*) AS DealsCountByPosition
FROM [EmployeesPositions]
INNER JOIN [Employees] ON [Employees].[EmployeesPositionId] = [EmployeesPositions].[Id]
INNER JOIN [Deals] ON [Deals].[EmployeeId] = [Employees].[Id]
GROUP BY [Position]


-- Employees Selects

SELECT * FROM [Employees]

SELECT [LastName], [FirstName], [Position]
FROM [Employees]
INNER JOIN [EmployeesPositions] ON [EmployeesPositions].[Id] = [Employees].[EmployeesPositionId]

SELECT [LastName], [FirstName], [Position], [Username], [Name] As RoleName
FROM [Employees]
INNER JOIN [Users] ON [Users].[EmployeeId] = [Employees].[Id]
INNER JOIN [EmployeesPositions] ON [EmployeesPositions].[Id] = [Employees].[EmployeesPositionId]
INNER JOIN [Roles] ON [Roles].[Id] = [Users].[RoleId]

SELECT e.[Id], e.[FirstName], e.[LastName], e.[MiddleName], COUNT([Deals].[Id]) As DealsCount
FROM [Employees] As e
	LEFT JOIN [Deals] ON [Deals].[EmployeeId] = e.[Id]
GROUP BY e.[Id], e.[FirstName], e.[LastName], e.[MiddleName]

SELECT e.[Id], e.[FirstName], e.[LastName], e.[MiddleName], COALESCE(SUM([Bill].[TotalPrice]), 0) As ProfitByEmployee
FROM [Employees] As e
	LEFT JOIN [Deals] ON [Deals].[EmployeeId] = e.[Id]
	LEFT JOIN [Bill] ON [Bill].[Id] = [Deals].[Id]
GROUP BY e.[Id], e.[FirstName], e.[LastName], e.[MiddleName]
ORDER BY ProfitByEmployee DESC

SELECT  e.[Id], e.[FirstName], e.[LastName], e.[MiddleName], COUNT([DealService].[DealId]) As NumberOfServicesProvidedByEmployee
FROM [Employees] As e
	LEFT JOIN [Deals] ON [Deals].[EmployeeId] = e.[Id]
	LEFT JOIN [DealService] ON [DealService].[DealId] = [Deals].[Id]
GROUP BY e.[Id], e.[FirstName], e.[LastName], e.[MiddleName]
ORDER BY NumberOfServicesProvidedByEmployee DESC


-- Deals Selects

SELECT * FROM [Deals]

SELECT COUNT(*) AS DealsCount FROM [Deals]

SELECT c.[SecondName], c.[FirstName], c.[MiddleName], c.[Occupation], c.[Address], c.[PhoneNumber]
FROM [Deals]
	INNER JOIN [Bill] ON [Bill].[Id] = [Deals].[Id]
	INNER JOIN [Clients] AS c ON c.[Id] = [Deals].[ClientId]
WHERE [Bill].[IsPaid] = 0

SELECT e.[LastName] AS EmployeeSecondName, e.[FirstName] AS EmployeeFirstName, b.[TotalPrice], c.[SecondName] AS ClientSecondName, c.[FirstName] AS ClientFirstName
FROM [Deals] AS d
	INNER JOIN [Bill] AS b ON b.[Id] = d.[Id]
	INNER JOIN [Clients] AS c ON c.[Id] = d.[ClientId]
	INNER JOIN [Employees] AS e ON e.[Id] = d.[ClientId]
WHERE b.[IsPaid] = 1 AND b.[TotalPrice] = (SELECT MAX([Bill].[TotalPrice]) FROM [Bill])


-- Clients Selects

SELECT * FROM [Clients]

SELECT *
FROM [Clients] AS c
	INNER JOIN [Deals] AS d ON d.[ClientId] = c.[Id]

SELECT c.[Id], c.[FirstName], c.[SecondName], COUNT(d.[Id]) AS ServicesCount
FROM [Clients] AS c
	INNER JOIN [Deals] AS d ON d.[ClientId] = c.[Id]
GROUP BY c.[Id], c.[FirstName], c.[SecondName]

SELECT c.[Id], c.[FirstName], c.[SecondName], SUM(b.[TotalPrice]) AS MoneySpent
FROM [Clients] AS c
	INNER JOIN [Deals] AS d ON d.[ClientId] = c.[Id]
	INNER JOIN [Bill] AS b ON b.[Id] = d.[Id]
WHERE b.[IsPaid] = 1
GROUP BY c.[Id], c.[FirstName], c.[SecondName]