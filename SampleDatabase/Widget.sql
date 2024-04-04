CREATE TABLE [dbo].[Widget] (
    [Id]    BIGINT        IDENTITY (1, 1) NOT NULL,
    [Name]  NVARCHAR (50) NULL,
    [Shape] NVARCHAR (50) NULL,
    [Color] NVARCHAR (50) NULL,
    CONSTRAINT [PK_Widget] PRIMARY KEY CLUSTERED ([Id] ASC)
);
