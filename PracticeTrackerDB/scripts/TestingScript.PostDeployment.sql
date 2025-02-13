/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

SET NOCOUNT ON;

BEGIN TRANSACTION;

SET IDENTITY_INSERT Users ON;
-- Insert test users
IF NOT EXISTS (SELECT 1 FROM Users WHERE Username = 'testuser1')
INSERT INTO Users (Id, Username, Password, Email, CreatedAt)
VALUES 
(1, 'testuser1', 'hashedpassword1', 'testuser1@example.com', GETDATE()),
(2, 'testuser2', 'hashedpassword2', 'testuser2@example.com', GETDATE());
SET IDENTITY_INSERT Users OFF;

-- Insert test songs
IF NOT EXISTS (SELECT 1 FROM Songs WHERE Title = 'Hotel California')
INSERT INTO Songs (Title, [Key], BPM, DurationMinutes, Notes)
VALUES 
('Hotel California', 'B minor', 74, 6, 'Classic rock'),
('Wonderwall', 'F# minor', 87, 4, '90s anthem'),
('Sweet Child O'' Mine', 'D', 125, 6, 'Guitar classic'),
('Smells Like Teen Spirit', 'F minor', 117, 5, 'Grunge hit'),
('Hey Jude', 'C', 72, 7, 'Timeless ballad');

-- Insert test setlists
IF NOT EXISTS (SELECT 1 FROM Setlists WHERE Name = 'Rock Classics')
INSERT INTO Setlists (UserId, Name, CreatedAt)
VALUES 
(1, 'Rock Classics', GETDATE()),
(2, '90s Favorites', GETDATE());

-- Insert test setlist songs
IF NOT EXISTS (SELECT 1 FROM SetlistSongs WHERE SetlistId = 1 AND SongId = 1)
INSERT INTO SetlistSongs (SetlistId, SongId, SongOrder, Notes)
VALUES 
(1, 1, 1, 'Opener'),
(1, 3, 2, 'Midway energy boost'),
(1, 5, 3, 'Crowd favorite'),
(2, 2, 1, 'Singalong hit'),
(2, 4, 2, 'High energy closer');

-- Insert test practice sessions
IF NOT EXISTS (SELECT 1 FROM PracticeSessions WHERE UserId = 1 AND FocusArea = 'Scales')
INSERT INTO PracticeSessions (UserId, CreatedAt, DurationMinutes, FocusArea, Notes)
VALUES 
(1, GETDATE(), 30, 'Scales', 'Practicing major scales'),
(2, GETDATE(), 45, 'Chords', 'Focusing on bar chords'),
(1, GETDATE(), 60, 'Soloing', 'Working on improvisation over backing tracks');

COMMIT TRANSACTION;