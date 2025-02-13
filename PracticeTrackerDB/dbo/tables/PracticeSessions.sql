CREATE TABLE [dbo].[PracticeSessions]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [UserId] INT NOT NULL, 
    [CreatedAt] DATETIME2 NOT NULL, 
    [DurationMinutes] INT NOT NULL, 
    [FocusArea] NVARCHAR(255) NOT NULL, 
    [Notes] NVARCHAR(MAX) NULL, 
    CONSTRAINT [FK_PracticeSessions_ToUsers] FOREIGN KEY ([UserId]) REFERENCES [Users]([Id])
)
