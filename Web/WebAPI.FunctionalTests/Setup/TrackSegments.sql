CREATE TABLE [dbo].[TrackSegments] (
    [Id]      INT IDENTITY (1, 1) NOT NULL,
    [TrackId] INT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_TrackSegmentEntity_TrackEntity_TrackEntityId] FOREIGN KEY ([TrackId]) REFERENCES [dbo].[Tracks] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_TrackEntity_TrackId]
    ON [dbo].[TrackSegments]([TrackId] ASC);

