CREATE TABLE [dbo].[User] (
    [Id]                 INT            IDENTITY (1, 1) NOT NULL,
    [FirstName]          VARCHAR (50)   NULL,
    [LastName]           VARCHAR (50)   NULL,
    [Email]              VARCHAR (50)   NULL,
    [Phone]              NCHAR (10)     NULL,
    [LastLogin]          DATETIME2 (7)  NULL,
    [CreationTime]       DATETIME2 (7)  NULL,
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([Id] ASC)
    );