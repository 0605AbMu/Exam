DECLARE @DataBaseName AS VARCHAR(20)='EVENT_MANAGEMENT';
IF EXISTS(SELECT 1
          FROM sys.databases
          WHERE name = @DataBaseName)
    BEGIN
        USE master;
        ALTER DATABASE EVENT_MANAGEMENT
            SET SINGLE_USER
            WITH ROLLBACK IMMEDIATE
        DROP DATABASE EVENT_MANAGEMENT;
    END
CREATE DATABASE EVENT_MANAGEMENT;
USE EVENT_MANAGEMENT;

DROP TABLE IF EXISTS [User]
GO;
DROP TABLE IF EXISTS [Room]
GO;
DROP TABLE IF EXISTS [Application]
GO;
DROP TABLE IF EXISTS [Order]
GO;
DROP TABLE IF EXISTS [UserEvent]
GO;
DROP TABLE IF EXISTS [Company]
GO;
DROP TABLE IF EXISTS [Role]
GO;

CREATE TABLE [User]
(
    [id]        int PRIMARY KEY IDENTITY (1, 1),
    [name]      varchar(30) NOT NULL,
    [roleId]    int         NOT NULL,
    [companyId] int,
    [username]  varchar(30) NOT NULL,
    [password]  varchar(30) NOT NULL,
    [createdAt] datetime,
    [updatedAt] datetime
)
GO

CREATE TABLE [Room]
(
    [id]        int PRIMARY KEY IDENTITY (1, 1),
    [name]      varchar(30) NOT NULL,
    [capacity]  int         NOT NULL,
    [createdAt] datetime,
    [updatedAt] datetime
)
GO

CREATE TABLE [Application]
(
    [id]        int PRIMARY KEY IDENTITY (1, 1),
    [userId]    int      NOT NULL,
    [roomId]    int      NOT NULL,
    [companyId] int      NOT NULL,
    [startAt]   datetime NOT NULL,
    [endAt]     datetime NOT NULL,
    [status]    int      NOT NULL,
    [createdAt] datetime,
    [updatedAt] datetime
)
GO

CREATE TABLE [Order]
(
    [userId]   int NOT NULL,
    [roomId]   int NOT NULL,
    [position] int NOT NULL
)
GO

CREATE TABLE [UserEvent]
(
    [userId]        int NOT NULL,
    [applicationId] int NOT NULL
)
GO

CREATE TABLE [Company]
(
    [id]      int PRIMARY KEY IDENTITY (1, 1),
    [name]    varchar(50) NOT NULL,
    [details] nvarchar(40)
)
GO

CREATE TABLE [Role]
(
    [id]       int PRIMARY KEY IDENTITY (1, 1),
    [roleName] varchar(40) NOT NULL,
    [status]   int         NOT NULL
)
GO

ALTER TABLE [Application]
    ADD FOREIGN KEY ([userId]) REFERENCES [User] ([id]) ON DELETE CASCADE
GO

ALTER TABLE [Application]
    ADD FOREIGN KEY ([companyId]) REFERENCES [Company] ([id]) ON DELETE CASCADE
GO

ALTER TABLE [User]
    ADD FOREIGN KEY ([roleId]) REFERENCES [Role] ([id])
GO

ALTER TABLE [User]
    ADD FOREIGN KEY ([companyId]) REFERENCES [Company] ([id])
GO

ALTER TABLE [UserEvent]
    ADD FOREIGN KEY ([userId]) REFERENCES [User] ([id]) ON DELETE CASCADE
GO

ALTER TABLE [UserEvent]
    ADD FOREIGN KEY ([applicationId]) REFERENCES [Application] ([id])
GO

ALTER TABLE [Application]
    ADD FOREIGN KEY ([roomId]) REFERENCES [Room] ([id]) ON DELETE CASCADE
GO

ALTER TABLE [Order]
    ADD FOREIGN KEY ([userId]) REFERENCES [User] ([id]) ON DELETE CASCADE
GO

ALTER TABLE [Order]
    ADD FOREIGN KEY ([roomId]) REFERENCES [Room] ([id]) ON DELETE CASCADE
GO


IF EXISTS(SELECT * FROM sys.views WHERE name = 'VIEW_GetAllUsers')
BEGIN
DROP VIEW VIEW_GetAllUsers;
END GO;

CREATE VIEW VIEW_GetAllUsers AS SELECT id, name, roleId as role, companyId, username, password, createdAt, updatedAt FROM [User];
GO;



INSERT INTO [Role] (roleName, status)
VALUES ('Superadmin', 1),
       ('Admin', 1),
       ('User', 1)

INSERT INTO [User] (name, roleId, companyId, username, password, createdAt, updatedAt)
VALUES ('Admin', 1, null, 'admin', 'admin', GETDATE(), GETDATE())

INSERT INTO Company (name, details) VALUES ('Unknown', '...');

INSERT INTO Room (name, capacity, createdAt, updatedAt)
VALUES
('Tencent', 20, GETDATE(), GETDATE());


INSERT INTO [User] (name, roleId, companyId, username, password, createdAt, updatedAt)
VALUES ('Abdumannon', 3, 1, '0605AbMu', '12345678', GETDATE(), GETDATE())


INSERT INTO [Application] (userId, roomId, companyId, startAt, endAt, status, createdAt, updatedAt)
VALUES
(1, 1, 1, GETDATE(), GETDATE(), 1, GETDATE(), GETDATE())