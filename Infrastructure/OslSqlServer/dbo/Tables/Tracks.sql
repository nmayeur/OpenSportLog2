﻿CREATE TABLE [dbo].[Tracks] (
    [Id]         INT           IDENTITY (1, 1) NOT NULL,
    [ActivityId] INT           NULL,
    [Name]       VARCHAR (100) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_TrackEntity_ActivityEntity_ActivityEntityId] FOREIGN KEY ([ActivityId]) REFERENCES [dbo].[Activities] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_TrackEntity_ActivityEntityId]
    ON [dbo].[Tracks]([ActivityId] ASC);

