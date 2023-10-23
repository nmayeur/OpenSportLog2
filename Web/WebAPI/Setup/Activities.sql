CREATE TABLE [dbo].[Activities] (
    [Id]                INT           IDENTITY (1, 1) NOT NULL,
    [AthleteId]         INT           NULL,
    [OriginSystem]      TEXT          NULL,
    [Name]              TEXT          NULL,
    [Location]          TEXT          NULL,
    [Calories]          INT           NOT NULL,
    [Sport]             INT           NOT NULL,
    [Cadence]           INT           DEFAULT ((0)) NOT NULL,
    [HeartRate]         INT           DEFAULT ((0)) NOT NULL,
    [Power]             INT           DEFAULT ((0)) NOT NULL,
    [Temperature]       INT           DEFAULT ((0)) NOT NULL,
    [Time]              DATETIME      NULL,
    [OriginId]          VARCHAR (100) NULL,
    [TimeSpan]          BIGINT        NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ActivityEntity_Athletes_AthleteId] FOREIGN KEY ([AthleteId]) REFERENCES [dbo].[Athletes] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Activity_AthleteId]
    ON [dbo].[Activities]([AthleteId] ASC);

