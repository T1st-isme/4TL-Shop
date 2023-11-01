CREATE TABLE [dbo].[OrderDetail] (
    [ID]        INT        IDENTITY (1, 1) NOT NULL,
    [IDProduct] INT        NULL,
    [IDOrder]   INT        NULL,
    [Quantity]  INT        NULL,
    [UnitPrice] FLOAT (53) NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([IDOrder]) REFERENCES [dbo].[OrderPro] ([ID]),
    FOREIGN KEY ([IDProduct]) REFERENCES [dbo].[Products] ([ProductID])
);

