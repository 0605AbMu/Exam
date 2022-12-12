SELECT * FROM VIEW_GetAllUsers WHERE username = '0605AbMu'

INSERT INTO [User] (name, roleId, companyId, username, password, createdAt, updatedAt)
VALUES ('Abdumannon', 3, 1, '0605AbMu', '12345678', GETDATE(), GETDATE())


INSERT INTO [Application] (userId, roomId, companyId, startAt, endAt, status, createdAt, updatedAt)
VALUES
(1, 1, 1, GETDATE(), GETDATE(), 1, GETDATE(), GETDATE())