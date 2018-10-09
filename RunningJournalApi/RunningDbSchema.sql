USE master
GO

IF EXISTS
(
    SELECT *
    FROM master.dbo.sysdatabases
    WHERE name = N'RunningJournal'
)
    BEGIN
        DROP DATABASE RunningJournal
    END
GO

CREATE DATABASE RunningJournal
GO

USE RunningJournal
GO

CREATE TABLE dbo.Users
(UserId   INT IDENTITY(1, 1) PRIMARY KEY CLUSTERED , 
 Username NVARCHAR(5) NOT NULL UNIQUE
)
GO

CREATE TABLE dbo.JournalEntry
(EntryId  INT IDENTITY(1, 1) PRIMARY KEY CLUSTERED , 
 UserId   INT NOT NULL
              REFERENCES dbo.Users(UserId), 
 Time     DATETIMEOFFSET(7) NOT NULL, 
 Distance INT NOT NULL, 
 Duration TIME NOT NULL
)
GO