CREATE TABLE [dbo].[Setlists]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [UserId] INT NOT NULL, 
    [Name] NVARCHAR(255) NOT NULL, 
    [CreatedAt] DATETIME2 NOT NULL, 
    CONSTRAINT [FK_Setlists_To_Users] FOREIGN KEY ([UserId]) REFERENCES [Users]([Id])
)
