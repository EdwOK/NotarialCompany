CREATE DATABASE NotarialCompanyDatabase;
GO
USE NotarialCompanyDatabase;

CREATE TABLE [dbo].[Clients] (
    [Id] [int] NOT NULL IDENTITY,
    [FirstName] [nvarchar](30) NOT NULL,
    [SecondName] [nvarchar](30) NOT NULL,
    [MiddleName] [nvarchar](30),
    [Occupation] [nvarchar](30) NOT NULL,
    [Address] [nvarchar](250),
    [PhoneNumber] [nvarchar](15) NOT NULL,
    CONSTRAINT [PK_dbo.Clients] PRIMARY KEY ([Id])
)

CREATE TABLE [dbo].[ClientsAudit] (
    [Id] [int] NOT NULL IDENTITY,
	[Type] [char](1) NOT NULL,
    [FieldName] [nvarchar](128) NOT NULL,
    [OldValue] [nvarchar](1000) NOT NULL,
    [NewValue] [nvarchar](1000) NOT NULL,
    [Date] [datetime] DEFAULT (GETDATE()),
    CONSTRAINT [PK_dbo.ClientsAudit] PRIMARY KEY ([Id])
)

CREATE TABLE [dbo].[Deals] (
    [Id] [int] NOT NULL IDENTITY,
    [Description] [nvarchar](100),
    [EmployeeId] [int] NOT NULL,
    [ClientId] [int] NOT NULL,
    CONSTRAINT [PK_dbo.Deals] PRIMARY KEY ([Id])
)

CREATE INDEX [IX_EmployeeId] ON [dbo].[Deals]([EmployeeId])
CREATE INDEX [IX_ClientId] ON [dbo].[Deals]([ClientId])

CREATE TABLE [dbo].[DealsAudit] (
    [Id] [int] NOT NULL IDENTITY,
	[Type] [char](1) NOT NULL,
    [FieldName] [nvarchar](128) NOT NULL,
    [OldValue] [nvarchar](1000) NOT NULL,
    [NewValue] [nvarchar](1000) NOT NULL,
    [Date] [datetime] DEFAULT (GETDATE()),
    CONSTRAINT [PK_dbo.DealsAudit] PRIMARY KEY ([Id])
)

CREATE TABLE [dbo].[Employees] (
    [Id] [int] NOT NULL IDENTITY,
    [FirstName] [nvarchar](30) NOT NULL,
    [LastName] [nvarchar](30) NOT NULL,
    [MiddleName] [nvarchar](30),
    [Address] [nvarchar](250),
    [PhoneNumber] [nvarchar](15) NOT NULL,
    [EmploymentDate] [date] NOT NULL,
    [EmployeesPositionId] [int] NOT NULL,
    CONSTRAINT [PK_dbo.Employees] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_EmployeesPositionId] ON [dbo].[Employees]([EmployeesPositionId])

CREATE TABLE [dbo].[EmployeesAudit] (
    [Id] [int] NOT NULL IDENTITY,
	[Type] [char](1) NOT NULL,
    [FieldName] [nvarchar](128) NOT NULL,
    [OldValue] [nvarchar](1000) NOT NULL,
    [NewValue] [nvarchar](1000) NOT NULL,
    [Date] [datetime] DEFAULT (GETDATE()),
    CONSTRAINT [PK_dbo.EmployeesAudit] PRIMARY KEY ([Id])
)

CREATE TABLE [dbo].[EmployeesPositions] (
    [Id] [int] NOT NULL IDENTITY,
    [Position] [nvarchar](30) NOT NULL,
    [Salary] [money] NOT NULL,
    [Commission] [int] NOT NULL
    CONSTRAINT [PK_dbo.EmployeesPositions] PRIMARY KEY ([Id])
)

CREATE TABLE [dbo].[EmployeesPositionsAudit] (
    [Id] [int] NOT NULL IDENTITY,
	[Type] [char](1) NOT NULL,
    [FieldName] [nvarchar](128) NOT NULL,
    [OldValue] [nvarchar](1000) NOT NULL,
    [NewValue] [nvarchar](1000) NOT NULL,
    [Date] [datetime] DEFAULT (GETDATE()),
    CONSTRAINT [PK_dbo.EmployeesPositionsAudit] PRIMARY KEY ([Id])
)

CREATE TABLE [dbo].[Services] (
    [Id] [int] NOT NULL IDENTITY,
    [Name] [nvarchar](100) NOT NULL,
    [Description] [nvarchar](500),
    [Cost] [money] NOT NULL,
    CONSTRAINT [PK_dbo.Services] PRIMARY KEY ([Id])
)

CREATE TABLE [dbo].[ServicesAudit] (
    [Id] [int] NOT NULL IDENTITY,
	[Type] [char](1) NOT NULL,
    [FieldName] [nvarchar](128) NOT NULL,
    [OldValue] [nvarchar](1000) NOT NULL,
    [NewValue] [nvarchar](1000) NOT NULL,
    [Date] [datetime] DEFAULT (GETDATE()),
    CONSTRAINT [PK_dbo.ServicesAudit] PRIMARY KEY ([Id])
)

CREATE TABLE [dbo].[Roles] (
    [Id] [int] NOT NULL IDENTITY,
    [Name] [nvarchar](30) NOT NULL,
    CONSTRAINT [PK_dbo.Roles] PRIMARY KEY ([Id])
)

CREATE TABLE [dbo].[RolesAudit] (
    [Id] [int] NOT NULL IDENTITY,
	[Type] [char](1) NOT NULL,
    [FieldName] [nvarchar](128) NOT NULL,
    [OldValue] [nvarchar](1000),
    [NewValue] [nvarchar](1000),
    [Date] [datetime] DEFAULT (GETDATE()),
    CONSTRAINT [PK_dbo.RolesAudit] PRIMARY KEY ([Id])
)

CREATE TABLE [dbo].[Users] (
    [Id] [int] NOT NULL IDENTITY,
    [Username] [nvarchar](30) NOT NULL,
    [Password] [nvarchar](50) NOT NULL,
    [Salt] [nvarchar](50) NOT NULL,
    [RoleId] [int] NOT NULL,
    [EmployeeId] [int] NOT NULL,
    CONSTRAINT [PK_dbo.Users] PRIMARY KEY ([Id])
)

CREATE INDEX [IX_RoleId] ON [dbo].[Users]([RoleId])
CREATE INDEX [IX_EmployeeId] ON [dbo].[Users]([EmployeeId])

CREATE TABLE [dbo].[UsersAudit] (
    [Id] [int] NOT NULL IDENTITY,
	[Type] [char](1) NOT NULL,
    [FieldName] [nvarchar](128) NOT NULL,
    [OldValue] [nvarchar](1000) NOT NULL,
    [NewValue] [nvarchar](1000) NOT NULL,
    [Date] [datetime] DEFAULT (GETDATE()),
    CONSTRAINT [PK_dbo.UsersAudit] PRIMARY KEY ([Id])
)

CREATE TABLE [dbo].[DealService] (
    [DealId] [int] NOT NULL,
    [ServiceId] [int] NOT NULL,
    CONSTRAINT [PK_dbo.DealService] PRIMARY KEY ([DealId], [ServiceId])
)

CREATE INDEX [IX_DealId] ON [dbo].[DealService]([DealId])
CREATE INDEX [IX_ServiceId] ON [dbo].[DealService]([ServiceId])

CREATE TABLE [dbo].[DealServiceAudit] (
    [Id] [int] NOT NULL IDENTITY,
	[Type] [char](1) NOT NULL,
    [FieldName] [nvarchar](128) NOT NULL,
    [OldValue] [nvarchar](1000) NOT NULL,
    [NewValue] [nvarchar](1000) NOT NULL,
    [Date] [datetime] DEFAULT (GETDATE()),
    CONSTRAINT [PK_dbo.DealServiceAudit] PRIMARY KEY ([Id])
)

CREATE TABLE [dbo].[Bill] (
    [Id] [int] NOT NULL,
    [BasePrice] [money] NOT NULL,
    [TotalPrice] [money] NOT NULL,
	[IsPaid] [bit] NOT NULL,
    [DateTime] [datetime] NOT NULL,
    CONSTRAINT [PK_dbo.Bill] PRIMARY KEY ([Id])
)

CREATE TABLE [dbo].[BillAudit] (
    [Id] [int] NOT NULL IDENTITY,
	[Type] [char](1) NOT NULL,
    [FieldName] [nvarchar](128) NOT NULL,
    [OldValue] [nvarchar](1000) NOT NULL,
    [NewValue] [nvarchar](1000) NOT NULL,
    [Date] [datetime] DEFAULT (GETDATE()),
    CONSTRAINT [PK_dbo.BillAudit] PRIMARY KEY ([Id])
)

ALTER TABLE [dbo].[Deals] ADD CONSTRAINT [FK_dbo.Deals_dbo.Clients_ClientId] FOREIGN KEY ([ClientId]) REFERENCES [dbo].[Clients] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[Deals] ADD CONSTRAINT [FK_dbo.Deals_dbo.Employees_EmployeeId] FOREIGN KEY ([EmployeeId]) REFERENCES [dbo].[Employees] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[Bill] ADD CONSTRAINT [FK_dbo.Bills_dbo.Deals_Id] FOREIGN KEY ([Id]) REFERENCES [dbo].[Deals] ([Id]) ON DELETE CASCADE

ALTER TABLE [dbo].[Employees] ADD CONSTRAINT [FK_dbo.Employees_dbo.EmployeesPositions_EmployeesPositionId] FOREIGN KEY ([EmployeesPositionId]) REFERENCES [dbo].[EmployeesPositions] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[Users] ADD CONSTRAINT [FK_dbo.Users_dbo.Employees_EmployeeId] FOREIGN KEY ([EmployeeId]) REFERENCES [dbo].[Employees] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[Users] ADD CONSTRAINT [FK_dbo.Users_dbo.Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Roles] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[DealService] ADD CONSTRAINT [FK_dbo.DealService_dbo.Deals_DealId] FOREIGN KEY ([DealId]) REFERENCES [dbo].[Deals] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[DealService] ADD CONSTRAINT [FK_dbo.DealService_dbo.Services_ServiceId] FOREIGN KEY ([ServiceId]) REFERENCES [dbo].[Services] ([Id]) ON DELETE CASCADE