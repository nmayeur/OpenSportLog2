CREATE TABLE [dbo].[Athletes] (
    [Id]   INT           IDENTITY (1, 1) NOT NULL,
    [Name] VARCHAR (100) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [AK_Athletes_Name] UNIQUE NONCLUSTERED ([Name] ASC)
);

