
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 05/04/2023 21:25:40
-- Generated from EDMX file: C:\Users\Grigoriy\Videos\DiplomWork\ShiraDK\BDEntities.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [ShiraDKDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_BuyingTickets_Events]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BuyingTickets] DROP CONSTRAINT [FK_BuyingTickets_Events];
GO
IF OBJECT_ID(N'[dbo].[FK_BuyingTickets_Users]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BuyingTickets] DROP CONSTRAINT [FK_BuyingTickets_Users];
GO
IF OBJECT_ID(N'[dbo].[FK_BuyingTickets_Users1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BuyingTickets] DROP CONSTRAINT [FK_BuyingTickets_Users1];
GO
IF OBJECT_ID(N'[dbo].[FK_Events_Organizers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Events] DROP CONSTRAINT [FK_Events_Organizers];
GO
IF OBJECT_ID(N'[dbo].[FK_ItemsForEvents_Events]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ItemsForEvents] DROP CONSTRAINT [FK_ItemsForEvents_Events];
GO
IF OBJECT_ID(N'[dbo].[FK_ItemsForEvents_Items]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ItemsForEvents] DROP CONSTRAINT [FK_ItemsForEvents_Items];
GO
IF OBJECT_ID(N'[dbo].[FK_Landings_BuyingTickets]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Landings] DROP CONSTRAINT [FK_Landings_BuyingTickets];
GO
IF OBJECT_ID(N'[dbo].[FK_Users_Roles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Users] DROP CONSTRAINT [FK_Users_Roles];
GO
IF OBJECT_ID(N'[dbo].[FK_WareHouse_Items]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[WareHouse] DROP CONSTRAINT [FK_WareHouse_Items];
GO
IF OBJECT_ID(N'[dbo].[FK_WareHouse_Users]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[WareHouse] DROP CONSTRAINT [FK_WareHouse_Users];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[BuyingTickets]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BuyingTickets];
GO
IF OBJECT_ID(N'[dbo].[Events]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Events];
GO
IF OBJECT_ID(N'[dbo].[Items]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Items];
GO
IF OBJECT_ID(N'[dbo].[ItemsForEvents]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ItemsForEvents];
GO
IF OBJECT_ID(N'[dbo].[Landings]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Landings];
GO
IF OBJECT_ID(N'[dbo].[Organizers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Organizers];
GO
IF OBJECT_ID(N'[dbo].[Roles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Roles];
GO
IF OBJECT_ID(N'[dbo].[sysdiagrams]', 'U') IS NOT NULL
    DROP TABLE [dbo].[sysdiagrams];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO
IF OBJECT_ID(N'[dbo].[WareHouse]', 'U') IS NOT NULL
    DROP TABLE [dbo].[WareHouse];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'BuyingTickets'
CREATE TABLE [dbo].[BuyingTickets] (
    [Number] int IDENTITY(1,1) NOT NULL,
    [PurchaseDate] datetime  NOT NULL,
    [SalesmanID] int  NOT NULL,
    [BuyerID] int  NULL,
    [EventID] int  NOT NULL,
    [Count] int  NULL
);
GO

-- Creating table 'Events'
CREATE TABLE [dbo].[Events] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(50)  NOT NULL,
    [DateStart] datetime  NOT NULL,
    [Duration] float  NOT NULL,
    [Description] varchar(50)  NULL,
    [OrganizerID] int  NOT NULL,
    [Price] float  NULL,
    [NumberOfSeats] int  NULL,
    [AvailableOfSeats] int  NULL
);
GO

-- Creating table 'Items'
CREATE TABLE [dbo].[Items] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nchar(10)  NOT NULL,
    [Width] float  NULL,
    [Length] float  NULL,
    [Height] float  NULL,
    [Image] varbinary(max)  NULL,
    [Description] varchar(50)  NULL,
    [Weight] float  NULL,
    [Count] int  NULL
);
GO

-- Creating table 'ItemsForEvents'
CREATE TABLE [dbo].[ItemsForEvents] (
    [Number] int IDENTITY(1,1) NOT NULL,
    [ItemsID] int  NULL,
    [Quantity] int  NULL,
    [EventsID] int  NULL
);
GO

-- Creating table 'Landings'
CREATE TABLE [dbo].[Landings] (
    [ID] int  NOT NULL,
    [PlaceNumber] int  NULL,
    [BuyingTicketsID] int  NULL
);
GO

-- Creating table 'Organizers'
CREATE TABLE [dbo].[Organizers] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(50)  NULL,
    [INN] varchar(11)  NOT NULL
);
GO

-- Creating table 'Roles'
CREATE TABLE [dbo].[Roles] (
    [ID] int  NOT NULL,
    [Name] varchar(50)  NOT NULL
);
GO

-- Creating table 'sysdiagrams'
CREATE TABLE [dbo].[sysdiagrams] (
    [name] nvarchar(128)  NOT NULL,
    [principal_id] int  NOT NULL,
    [diagram_id] int IDENTITY(1,1) NOT NULL,
    [version] int  NULL,
    [definition] varbinary(max)  NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [FirstName] varchar(50)  NULL,
    [LastName] varchar(50)  NULL,
    [RoleID] int  NOT NULL,
    [Login] varchar(50)  NOT NULL,
    [Password] varchar(50)  NOT NULL
);
GO

-- Creating table 'WareHouses'
CREATE TABLE [dbo].[WareHouses] (
    [Number] int IDENTITY(1,1) NOT NULL,
    [ItemID] int  NOT NULL,
    [DateOfReceipt] datetime  NOT NULL,
    [Quantity] int  NULL,
    [UserID] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Number] in table 'BuyingTickets'
ALTER TABLE [dbo].[BuyingTickets]
ADD CONSTRAINT [PK_BuyingTickets]
    PRIMARY KEY CLUSTERED ([Number] ASC);
GO

-- Creating primary key on [ID] in table 'Events'
ALTER TABLE [dbo].[Events]
ADD CONSTRAINT [PK_Events]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Items'
ALTER TABLE [dbo].[Items]
ADD CONSTRAINT [PK_Items]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [Number] in table 'ItemsForEvents'
ALTER TABLE [dbo].[ItemsForEvents]
ADD CONSTRAINT [PK_ItemsForEvents]
    PRIMARY KEY CLUSTERED ([Number] ASC);
GO

-- Creating primary key on [ID] in table 'Landings'
ALTER TABLE [dbo].[Landings]
ADD CONSTRAINT [PK_Landings]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Organizers'
ALTER TABLE [dbo].[Organizers]
ADD CONSTRAINT [PK_Organizers]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Roles'
ALTER TABLE [dbo].[Roles]
ADD CONSTRAINT [PK_Roles]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [diagram_id] in table 'sysdiagrams'
ALTER TABLE [dbo].[sysdiagrams]
ADD CONSTRAINT [PK_sysdiagrams]
    PRIMARY KEY CLUSTERED ([diagram_id] ASC);
GO

-- Creating primary key on [ID] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [Number] in table 'WareHouses'
ALTER TABLE [dbo].[WareHouses]
ADD CONSTRAINT [PK_WareHouses]
    PRIMARY KEY CLUSTERED ([Number] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [EventID] in table 'BuyingTickets'
ALTER TABLE [dbo].[BuyingTickets]
ADD CONSTRAINT [FK_BuyingTickets_Events]
    FOREIGN KEY ([EventID])
    REFERENCES [dbo].[Events]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BuyingTickets_Events'
CREATE INDEX [IX_FK_BuyingTickets_Events]
ON [dbo].[BuyingTickets]
    ([EventID]);
GO

-- Creating foreign key on [SalesmanID] in table 'BuyingTickets'
ALTER TABLE [dbo].[BuyingTickets]
ADD CONSTRAINT [FK_BuyingTickets_Users]
    FOREIGN KEY ([SalesmanID])
    REFERENCES [dbo].[Users]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BuyingTickets_Users'
CREATE INDEX [IX_FK_BuyingTickets_Users]
ON [dbo].[BuyingTickets]
    ([SalesmanID]);
GO

-- Creating foreign key on [BuyerID] in table 'BuyingTickets'
ALTER TABLE [dbo].[BuyingTickets]
ADD CONSTRAINT [FK_BuyingTickets_Users1]
    FOREIGN KEY ([BuyerID])
    REFERENCES [dbo].[Users]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BuyingTickets_Users1'
CREATE INDEX [IX_FK_BuyingTickets_Users1]
ON [dbo].[BuyingTickets]
    ([BuyerID]);
GO

-- Creating foreign key on [BuyingTicketsID] in table 'Landings'
ALTER TABLE [dbo].[Landings]
ADD CONSTRAINT [FK_Landings_BuyingTickets]
    FOREIGN KEY ([BuyingTicketsID])
    REFERENCES [dbo].[BuyingTickets]
        ([Number])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Landings_BuyingTickets'
CREATE INDEX [IX_FK_Landings_BuyingTickets]
ON [dbo].[Landings]
    ([BuyingTicketsID]);
GO

-- Creating foreign key on [OrganizerID] in table 'Events'
ALTER TABLE [dbo].[Events]
ADD CONSTRAINT [FK_Events_Organizers]
    FOREIGN KEY ([OrganizerID])
    REFERENCES [dbo].[Organizers]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Events_Organizers'
CREATE INDEX [IX_FK_Events_Organizers]
ON [dbo].[Events]
    ([OrganizerID]);
GO

-- Creating foreign key on [EventsID] in table 'ItemsForEvents'
ALTER TABLE [dbo].[ItemsForEvents]
ADD CONSTRAINT [FK_ItemsForEvents_Events]
    FOREIGN KEY ([EventsID])
    REFERENCES [dbo].[Events]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ItemsForEvents_Events'
CREATE INDEX [IX_FK_ItemsForEvents_Events]
ON [dbo].[ItemsForEvents]
    ([EventsID]);
GO

-- Creating foreign key on [ItemsID] in table 'ItemsForEvents'
ALTER TABLE [dbo].[ItemsForEvents]
ADD CONSTRAINT [FK_ItemsForEvents_Items]
    FOREIGN KEY ([ItemsID])
    REFERENCES [dbo].[Items]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ItemsForEvents_Items'
CREATE INDEX [IX_FK_ItemsForEvents_Items]
ON [dbo].[ItemsForEvents]
    ([ItemsID]);
GO

-- Creating foreign key on [ItemID] in table 'WareHouses'
ALTER TABLE [dbo].[WareHouses]
ADD CONSTRAINT [FK_WareHouse_Items]
    FOREIGN KEY ([ItemID])
    REFERENCES [dbo].[Items]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_WareHouse_Items'
CREATE INDEX [IX_FK_WareHouse_Items]
ON [dbo].[WareHouses]
    ([ItemID]);
GO

-- Creating foreign key on [RoleID] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [FK_Users_Roles]
    FOREIGN KEY ([RoleID])
    REFERENCES [dbo].[Roles]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Users_Roles'
CREATE INDEX [IX_FK_Users_Roles]
ON [dbo].[Users]
    ([RoleID]);
GO

-- Creating foreign key on [UserID] in table 'WareHouses'
ALTER TABLE [dbo].[WareHouses]
ADD CONSTRAINT [FK_WareHouse_Users]
    FOREIGN KEY ([UserID])
    REFERENCES [dbo].[Users]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_WareHouse_Users'
CREATE INDEX [IX_FK_WareHouse_Users]
ON [dbo].[WareHouses]
    ([UserID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------