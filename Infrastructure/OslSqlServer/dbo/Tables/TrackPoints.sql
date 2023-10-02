CREATE TABLE [dbo].[TrackPoints] (
    [Id]             INT      IDENTITY (1, 1) NOT NULL,
    [Time1]          TEXT     NOT NULL,
    [Latitude]       REAL     NOT NULL,
    [Longitude]      REAL     NOT NULL,
    [Elevation]      REAL     NOT NULL,
    [HeartRate]      INT      NOT NULL,
    [Cadence]        INT      NOT NULL,
    [TrackSegmentId] INT      NOT NULL,
    [Power]          INT      DEFAULT ((0)) NOT NULL,
    [Temperature]    REAL     DEFAULT ((0)) NOT NULL,
    [Time]           DATETIME NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_TrackPointVO_TrackSegmentEntity_TrackSegmentEntityId] FOREIGN KEY ([TrackSegmentId]) REFERENCES [dbo].[TrackSegments] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_TrackPoint_TrackSegmentId]
    ON [dbo].[TrackPoints]([TrackSegmentId] ASC);

