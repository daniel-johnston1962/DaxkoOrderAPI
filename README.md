# DaxkoOrderAPI

This is a .Net Core 2.2 Restful Web API.  This project was for a interview for a local company. 

### SQL 

```sql

-- CREATE SCHEMA Orders 

CREATE TABLE  [Daxko].[Orders].[Item]
(
	[ID] [bigint] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Description] [varchar](200) NOT NULL,
	[Price] [decimal](9, 2) NOT NULL,
	[IsActive] [bit] NOT NULL CONSTRAINT [df_Item_IsActive]  DEFAULT ((1)),
	[CreatedDateTime] [datetime] NOT NULL CONSTRAINT [df_Item_CreatedDateTime]  DEFAULT (getdate())
)

GO
----------------------------------------------

CREATE TABLE [Daxko].[Orders].[OrderDetail](
	[ID] [bigint] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[ShippedOrderID] [uniqueidentifier] NOT NULL,
	[ItemID] [bigint] NOT NULL,
	[Quantity] [int] NOT NULL,
	[Price] [decimal](9, 2) NOT NULL,
	[TotalPrice] [decimal](12, 2) NOT NULL
)

GO
----------------------------------------------

CREATE TABLE [Daxko].[Orders].[ShippedOrder](
	[ID] [bigint] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[ShippedOrderID] [uniqueidentifier] NOT NULL,
	[CreatedDateTime] [datetime] NOT NULL  CONSTRAINT [df_OrderDetail_CreatedDateTime]  DEFAULT (getdate())
)

GO

----------------------------------------------

INSERT INTO [Daxko].[Orders].[Item]
	(Description,Price)
VALUES
	('Red Stapler',50),
	('TPS Report',3),
	('Printer',400),
	('Baseball bat',80),
	('Michael Bolton CD',12)

GO

```
