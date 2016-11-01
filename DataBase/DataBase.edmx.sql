
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 10/31/2016 15:25:13
-- Generated from EDMX file: D:\Facultate\Anul III Semestru II\.NET\Licenta 1\DataBase\DataBase.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Licenta Baza de date];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_UserHistory]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Histories] DROP CONSTRAINT [FK_UserHistory];
GO
IF OBJECT_ID(N'[dbo].[FK_UserHistory1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Histories] DROP CONSTRAINT [FK_UserHistory1];
GO
IF OBJECT_ID(N'[dbo].[FK_UserRequest]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Requests] DROP CONSTRAINT [FK_UserRequest];
GO
IF OBJECT_ID(N'[dbo].[FK_UserAvatarUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserAvatars] DROP CONSTRAINT [FK_UserAvatarUser];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO
IF OBJECT_ID(N'[dbo].[Histories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Histories];
GO
IF OBJECT_ID(N'[dbo].[Requests]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Requests];
GO
IF OBJECT_ID(N'[dbo].[UserAvatars]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserAvatars];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [IdUser] int IDENTITY(1,1) NOT NULL,
    [Username] nvarchar(max)  NOT NULL,
    [Password] nvarchar(max)  NOT NULL,
    [Email] nvarchar(max)  NULL,
    [Status] int  NULL
);
GO

-- Creating table 'Histories'
CREATE TABLE [dbo].[Histories] (
    [IdHistory] int IDENTITY(1,1) NOT NULL,
    [Conversation] nvarchar(max)  NOT NULL,
    [UserIdUser] int  NOT NULL,
    [UserIdUser1] int  NOT NULL
);
GO

-- Creating table 'Requests'
CREATE TABLE [dbo].[Requests] (
    [IdRequest] int IDENTITY(1,1) NOT NULL,
    [FromUsername] nvarchar(max)  NOT NULL,
    [FromUserId] int  NOT NULL,
    [UserIdUser] int  NOT NULL
);
GO

-- Creating table 'UserAvatars'
CREATE TABLE [dbo].[UserAvatars] (
    [IdUserAvatar] int IDENTITY(1,1) NOT NULL,
    [Image] varbinary(max)  NULL,
    [User_IdUser] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [IdUser] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([IdUser] ASC);
GO

-- Creating primary key on [IdHistory] in table 'Histories'
ALTER TABLE [dbo].[Histories]
ADD CONSTRAINT [PK_Histories]
    PRIMARY KEY CLUSTERED ([IdHistory] ASC);
GO

-- Creating primary key on [IdRequest] in table 'Requests'
ALTER TABLE [dbo].[Requests]
ADD CONSTRAINT [PK_Requests]
    PRIMARY KEY CLUSTERED ([IdRequest] ASC);
GO

-- Creating primary key on [IdUserAvatar] in table 'UserAvatars'
ALTER TABLE [dbo].[UserAvatars]
ADD CONSTRAINT [PK_UserAvatars]
    PRIMARY KEY CLUSTERED ([IdUserAvatar] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [UserIdUser] in table 'Histories'
ALTER TABLE [dbo].[Histories]
ADD CONSTRAINT [FK_UserHistory]
    FOREIGN KEY ([UserIdUser])
    REFERENCES [dbo].[Users]
        ([IdUser])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserHistory'
CREATE INDEX [IX_FK_UserHistory]
ON [dbo].[Histories]
    ([UserIdUser]);
GO

-- Creating foreign key on [UserIdUser1] in table 'Histories'
ALTER TABLE [dbo].[Histories]
ADD CONSTRAINT [FK_UserHistory1]
    FOREIGN KEY ([UserIdUser1])
    REFERENCES [dbo].[Users]
        ([IdUser])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserHistory1'
CREATE INDEX [IX_FK_UserHistory1]
ON [dbo].[Histories]
    ([UserIdUser1]);
GO

-- Creating foreign key on [UserIdUser] in table 'Requests'
ALTER TABLE [dbo].[Requests]
ADD CONSTRAINT [FK_UserRequest]
    FOREIGN KEY ([UserIdUser])
    REFERENCES [dbo].[Users]
        ([IdUser])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserRequest'
CREATE INDEX [IX_FK_UserRequest]
ON [dbo].[Requests]
    ([UserIdUser]);
GO

-- Creating foreign key on [User_IdUser] in table 'UserAvatars'
ALTER TABLE [dbo].[UserAvatars]
ADD CONSTRAINT [FK_UserAvatarUser]
    FOREIGN KEY ([User_IdUser])
    REFERENCES [dbo].[Users]
        ([IdUser])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserAvatarUser'
CREATE INDEX [IX_FK_UserAvatarUser]
ON [dbo].[UserAvatars]
    ([User_IdUser]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------