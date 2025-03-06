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
-- Ensure the Users table exists before inserting
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Users')
BEGIN
    PRINT 'Users table does not exist. Skipping seed script.';
    RETURN;
END

-- Insert Admin User if not exists
IF NOT EXISTS (SELECT 1 FROM Users WHERE Username = 'admin')
BEGIN
    INSERT INTO Users (Id, Username, FirstName, LastName, Email, PasswordHash, CreatedAt)
    VALUES 
    (NEWID(), 'admin', 'Admin', 'User', 'admin@example.com', '$2a$10$hTUIyEEHcYLMsz2EC8DDxeEOjN0/Dw1JEStoTypZmYM3HOgOye3Nq', GETDATE());

    PRINT 'Admin user inserted successfully.';
END
ELSE
BEGIN
    PRINT 'Admin user already exists. Skipping insertion.';
END

-- Declare Admin UserId
DECLARE @AdminUserId UNIQUEIDENTIFIER
SELECT @AdminUserId = Id FROM Users WHERE Username = 'admin'

-- Insert Sample Songs
IF NOT EXISTS (SELECT 1 FROM Songs)
BEGIN
    INSERT INTO Songs (Title, [Key], BPM, DurationMinutes, Notes)
    VALUES
    ('Sweet Child O'' Mine', 'D Major', 125, 6, 'Intro picking pattern'),
    ('Crazy Train', 'F# Minor', 135, 5, 'Watch tempo variations'),
    ('Purple Haze', 'E Minor', 114, 4, 'Focus on triplets in solo');

    PRINT 'Sample songs inserted successfully.';
END
ELSE
BEGIN
    PRINT 'Songs already exist. Skipping insertion.';
END

-- Insert Sample Setlist for Admin
IF NOT EXISTS (SELECT 1 FROM Setlists WHERE UserId = @AdminUserId)
BEGIN
    INSERT INTO Setlists (UserId, Name, CreatedAt)
    VALUES 
    (@AdminUserId, 'Rock Essentials', GETDATE());

    PRINT 'Sample setlist inserted successfully.';
END
ELSE
BEGIN
    PRINT 'Setlist already exists. Skipping insertion.';
END

-- Declare SetlistId
DECLARE @SetlistId INT
SELECT @SetlistId = Id FROM Setlists WHERE UserId = @AdminUserId AND Name = 'Rock Essentials'

-- Insert Songs into Setlist
IF NOT EXISTS (SELECT 1 FROM SetlistSongs WHERE SetlistId = @SetlistId)
BEGIN
    INSERT INTO SetlistSongs (SetlistId, SongId, SongOrder, Notes)
    SELECT @SetlistId, Id, ROW_NUMBER() OVER (ORDER BY Id) AS SongOrder, 'No notes'
    FROM Songs
    WHERE Title IN ('Sweet Child O'' Mine', 'Crazy Train', 'Purple Haze');

    PRINT 'Songs added to setlist successfully.';
END
ELSE
BEGIN
    PRINT 'Setlist songs already exist. Skipping insertion.';
END

-- Insert Sample Practice Sessions
IF NOT EXISTS (SELECT 1 FROM PracticeSessions WHERE UserId = @AdminUserId)
BEGIN
    INSERT INTO PracticeSessions (UserId, CreatedAt, DurationMinutes, FocusArea)
    VALUES
    (@AdminUserId, DATEADD(DAY, -1, GETDATE()), 45, 'Scales'),
    (@AdminUserId, DATEADD(DAY, -2, GETDATE()), 60, 'Chord Transitions'),
    (@AdminUserId, DATEADD(DAY, -3, GETDATE()), 30, 'Improvisation');

    PRINT 'Sample practice sessions inserted successfully.';
END
ELSE
BEGIN
    PRINT 'Practice sessions already exist. Skipping insertion.';
END
