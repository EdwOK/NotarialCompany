﻿INSERT INTO [dbo].[Roles] (Name)
VALUES ('Admin')

INSERT INTO [dbo].[Roles] (Name)
VALUES ('User')

GO

INSERT INTO [dbo].[Services] (Name, Description, Cost)
VALUES (N'Удостоверение сделок (договоров)', N'У нотариуса вы можете оформить и удостоверить абсолютно любой вид сделки или договора (например, договор купли-продажи, мены, дарения, ренты, брачный договор и т.д.).', 7000)

INSERT INTO [dbo].[Services](Name, Description, Cost)
VALUES (N'Составление завещаний и выдача свидетельств о праве на наследство', N'Наследование – это переход имущественных прав и обязанностей умершего человека, другим лицам, в порядке определенном завещанием этого человека или законом. Существует два вида наследования: Наследование по закону. Наследование по завещанию.', 2000)

INSERT INTO [dbo].[Services] (Name, Description, Cost)
VALUES (N'Удостоверение доверенностей', N'Нотариус может «заверить» абсолютно любой вид доверенности (например, доверенность на право пользования, распоряжения имуществом, получение документов, генеральную доверенность на автомобиль и т.д.).', 5000)

INSERT INTO [dbo].[Services] (Name, Description, Cost)
VALUES (N'Выдача свидетельств', N'Обратившись к нотариусу, вы можете получить свидетельство:Об удостоверении фактов, нахождения гражданина в живых и в определенном месте.Тождественности гражданина с лицом, изображенным на фотографии.Об удостоверении времени предъявления документов.', 1300)

INSERT INTO [dbo].[Services] (Name, Description, Cost)
VALUES (N'Засвидетельствование подлинности подписи, верности перевода, копий документов и выписок из них', N'В основном к нотариусу обращаются за оказанием данной услуги юридические лица (например, когда хотят засвидетельствовать подлинность подписи на заявлениях в налоговые органы, банковских карточках, и на других документах).', 1000)

INSERT INTO [dbo].[Services] (Name, Description, Cost)
VALUES (N'Принятие в депозит денежных сумм и ценных бумаг', N'Нотариус может взять на себя ответственность и принять от должника денежную сумму или ценные бумаги, для их дальнейшей передачи кредитору. Также существуют другие случаи, когда необходимо принять в депозит денежную сумму или ценные бумаги (например, при банкротстве или ликвидации организации).', 900)

INSERT [dbo].[Services] (Name, Description, Cost)
VALUES (N'Совершение исполнительских надписей и обеспечение доказательств', N'Нотариус совершает исполнительные надписи на документах в случаях, когда необходимо взыскать денежную сумму или истребовать имущество от должника. Помимо испольнительных надписей нотариус оказывает услуги по обеспечению необходимых доказательств.', 1200)

INSERT INTO [dbo].[Services] (Name, Description, Cost)
VALUES (N'Принятие на хранение документов, предъявление чеков к платежу и удостоверение их неоплаты', N'Нотариусу можно сдать на хранение различного рода документы. Как правило, это делается по описи, но можно обойтись и без нее, в том случае если все документы оформлены надлежащим образом. Так же нотариус принимает для предъявления к платежу чеки (ценные бумаги в которых содержится распоряжение чекодателя своему банку о выплате денежной суммы чекодержателю).', 3000)

GO

INSERT INTO [dbo].[EmployeesPositions] (Position, Salary, Commission)
VALUES (N'Стажер', 20000, 5)

INSERT INTO [dbo].[EmployeesPositions] (Position, Salary, Commission)
VALUES (N'Нотариус 3 разряда', 40000, 10)

INSERT INTO [dbo].[EmployeesPositions] (Position, Salary, Commission)
VALUES (N'Нотариус 2 разряда', 60000, 20)

INSERT INTO [dbo].[EmployeesPositions] (Position, Salary, Commission)
VALUES (N'Нотариус 1 разряда', 80000, 30)

GO

INSERT INTO [dbo].[Employees] ([FirstName], [LastName], [MiddleName], [Address], [PhoneNumber], [EmploymentDate], [EmployeesPositionId])
VALUES (N'Default', N'', N'', N'' , N'', '2010-05-08', 1)

INSERT INTO [dbo].[Users] ([Username], [Password], [Salt], [RoleId], [EmployeeId])
VALUES (N'not', N'bH/LJ4ToQu+2U/upEYz8d7E+ySLODkVCUBwrSK6rb4o=', N'+NXdbYwfuP9NEtcYIxu/tt+IhJ7Xje8QoGLXV8dwdKA=', 1, 1)