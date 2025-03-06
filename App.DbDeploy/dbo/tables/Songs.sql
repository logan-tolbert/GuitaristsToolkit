CREATE TABLE [dbo].[Songs]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Title] NVARCHAR(255) NOT NULL, 
    [Key] NVARCHAR(50) NULL, 
    [BPM] INT NULL, 
    [DurationMinutes] INT NULL, 
    [Notes] NVARCHAR(MAX) NULL
)
