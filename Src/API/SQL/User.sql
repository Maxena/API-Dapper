CREATE TABLE [dbo].[User] (
    [Id]                 INT            IDENTITY (1, 1) NOT NULL,
    [FirstName]          VARCHAR (50)   NULL,
    [LastName]           VARCHAR (50)   NULL,
    [Email]              VARCHAR (50)   NULL,
    [Phone]              NCHAR (10)     NULL,
    [LastLogin]          DATETIME2 (7)  NULL,
    [CreationTime]       DATETIME2 (7)  NULL,
    [ActivationCode]     INT            NULL,
    [Login]              VARCHAR (50)   NOT NULL,
    [Password]           VARCHAR (50)   NOT NULL,
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([Id] ASC)
    );