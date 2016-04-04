INSERT INTO [dbo].[Clients] (FirstName, SecondName, MiddleName, Occupation, Address, PhoneNumber)
VALUES (N'Василий', N'Угорнов', N'Иванович', N'Слесарь', N'г. Минск, ул. Пушкина, д. 21, кв. 27' , N'+375291235343')

INSERT INTO [dbo].[Clients] (FirstName, SecondName, MiddleName, Occupation, Address, PhoneNumber)
VALUES (N'Иван', N'Райнов', N'Петрович', N'Банкир', N'г. Минск, ул. Лавренова, д. 1, кв. 2' , N'+375293332266')

GO

INSERT INTO [dbo].[Employees] ([FirstName], [LastName], [MiddleName], [Address], [PhoneNumber], [EmploymentDate], [EmployeesPositionId])
VALUES (N'Максим', N'Веселко', N'Иванович', N'г. Минск, ул. Куйбышева, д. 22, кв. 37' , N'+375291235343', '2010-05-08', 4)

INSERT INTO [dbo].[Employees] ([FirstName], [LastName], [MiddleName], [Address], [PhoneNumber], [EmploymentDate], [EmployeesPositionId])
VALUES (N'Дмитрий', N'Кременетский', N'Александрович', N'г. Минск, ул. Костюшки, д. 54, кв. 127' , N'+375291231231', '2013-04-12', 1)

INSERT INTO [dbo].[Employees] ([FirstName], [LastName], [MiddleName], [Address], [PhoneNumber], [EmploymentDate], [EmployeesPositionId])
VALUES (N'Андрей', N'Павловский', N'Романович', N'г. Минск, ул. Костюшки, д. 59, кв. 127' , N'+375291231731', '2013-04-12', 1)

GO

INSERT INTO [dbo].[Users] ([Username], [Password], [Salt], [RoleId], [EmployeeId])
VALUES (N'MaxR123', N'Qwerty123', N'', 1, 1)

INSERT INTO [dbo].[Users] ([Username], [Password], [Salt], [RoleId], [EmployeeId])
VALUES (N'Krem', N'qaz123qaz', N'', 2, 2)

INSERT INTO [dbo].[Users] ([Username], [Password], [Salt], [RoleId], [EmployeeId])
VALUES (N'Lolovik', N'qaz123qajz', N'', 2, 3)

GO

--Deal Sql Script1

INSERT INTO [dbo].[Deals] ([Description], [EmployeeId], [ClientId])
VALUES (N'Заказ услуг у компании', 1, 1)

INSERT INTO [dbo].[DealService] ([DealId], [ServiceId])
VALUES (1, 1)

INSERT INTO [dbo].[DealService] ([DealId], [ServiceId])
VALUES (1, 3)

INSERT INTO [dbo].[DealService] ([DealId], [ServiceId])
VALUES (1, 4)

INSERT INTO [dbo].[DealService] ([DealId], [ServiceId])
VALUES (1, 5)

INSERT INTO [dbo].[Bill] ([Id], [BasePrice], [TotalPrice], [IsPaid], [DateTime])
VALUES (1, 10000, 10000, 0, '12-01-01 12:32')

GO

--Deal Sql Script2

INSERT INTO [dbo].[Deals] ([Description], [EmployeeId], [ClientId])
VALUES (N'Заказ услуг у компании', 2, 2)

INSERT INTO [dbo].[DealService] ([DealId], [ServiceId])
VALUES (2, 1), (2, 2), (2, 3)

INSERT INTO [dbo].[Bill] ([Id], [BasePrice], [TotalPrice], [IsPaid], [DateTime])
VALUES (2, 10000, 10000, 1, '12-01-16 12:32')

--Deal Sql Script3

INSERT INTO [dbo].[Deals] ([Description], [EmployeeId], [ClientId])
VALUES (N'Заказ услуг у компании2', 2, 1)

INSERT INTO [dbo].[DealService] ([DealId], [ServiceId])
VALUES (3, 2), (3, 4), (3, 5)

INSERT INTO [dbo].[Bill] ([Id], [BasePrice], [TotalPrice], [IsPaid], [DateTime])
VALUES (3, 100000, 100000, 1, '12-11-16 10:30')





--EXEC [Clients.AddClient] N'1', N'1', N'1', N'1', N'1' , N'1'
--EXEC [Employees.AddEmployee] N'1', N'1', N'1', N'1' , N'1', '2013-04-12', 1
--EXEC [Users.AddUser] N'1', N'1', N'1', 1, 1

--EXEC [Deals.AddDeal] N'qwe1', 1, 1, 0, '12-11-16 10:30', '1,2,3,5';