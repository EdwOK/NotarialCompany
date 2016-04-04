UPDATE [dbo].[Clients]
SET [FirstName] = 'Alexander', [SecondName] = 'Sashkov', [MiddleName] = 'Sashev', [Occupation] ='Director', [Address]='Pushkina street', [PhoneNumber] = '+375281234534'
WHERE Id = 1;

UPDATE [dbo].[Services]
SET [Description] = 'Kill Kenedy', [Cost] = 1000000
WHERE Id = 1;

UPDATE [dbo].[EmployeesPositions]
SET [Positition] = 'Intern', [Salary] = 100, [Commission] = 1, [Description] = 'Kenedy'
WHERE Id = 1;

UPDATE [dbo].[Employees]
SET [FirstName] = 'Vasia', [LastName] = 'Pupkin', [MiddleName] = 'Jora', [Address] = 'Kenedy', [PhoneNumber] = '+375297878987',[EmploymentDate] = '', [EmployeesPositionId] = 1
WHERE Id = 1;

UPDATE [dbo].[Users]
SET [Username] = 'Vasia', [Password] = 'qwerrtq', [Salt] = '', [RoleId] = 2, [EmployeeId] = 1
WHERE Id = 1;

UPDATE [dbo].[Deals]
SET [Description] = 'Deal', [EmployeeId] = 1
WHERE Id = 1;