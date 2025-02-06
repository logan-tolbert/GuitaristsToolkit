CREATE TABLE [dbo].[SetlistSongs]
(
	[SetlistId] INT NOT NULL, 
    [SongId] INT NOT NULL, 
    [SongOrder] INT NOT NULL, 
    [Notes] NVARCHAR(MAX) NOT NULL, 
    CONSTRAINT [FK_SetlistSongs_To_Setlists] FOREIGN KEY ([SetlistId]) REFERENCES [Setlists]([Id]), 
    CONSTRAINT [FK_SetlistSongs_To_Songs] FOREIGN KEY ([SongId]) REFERENCES [Songs]([Id]) 
)
