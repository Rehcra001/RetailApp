USE RetailAppDB;
GO

--Insert Company Detail
INSERT INTO dbo.CompanyDetail
(CompanyName, VatRegistrationNumber, AddressLine1, AddressLine2, City, Province,
 PostalCode, FirstName, LastName, EMailAddress, PhoneNumber)
 VALUES 
 ('General Hardware Supplier', 4563217895, '35 Stone rd', 'Eden Park',  'Edenvale',
  'Gauteng', 1600, 'James', 'Kruger', 'James@gmail.com', '0112365656');
GO

--Insert Vendors
INSERT INTO dbo.Vendors
(CompanyName, VatRegistrationNumber, AddressLine1, AddressLine2, City, Province,
 PostalCode, FirstName, LastName, EMailAddress, PhoneNumber)
 VALUES 
 ('Hydraulic Hardware Manufacturer', 4563225975, '18 Ridgeway rd', 'Glen Park', 'Johannesburg',
  'Gauteng', 1600, 'Jerry', 'Rigel', 'Jerry@gmail.com', '0115629999'),
 ('Garden Hardware Manufacture', 4915632584, '36 Sandwitch rd', 'Lombardy East', 'Johannesburg',
  'Gauteng', 1600, 'Shane', 'Manual', 'Shane@gmail.com', '0824596524'),
 ('Greenway Electrical Manufacturer', 4258963147, '55 Long rd', 'Isando', 'Johannesburg',
  'Gauteng', 1600, 'Gavin', 'Abraham', 'Gavin@gmail.com', '0765986512');
GO

--Insert Customers
INSERT INTO dbo.Customers
(CompanyName, VatRegistrationNumber, AddressLine1, AddressLine2, City, Province,
 PostalCode, FirstName, LastName, EMailAddress, PhoneNumber)
 VALUES 
 ('Gordon Hardware Suppliers', 4159753575, '55 Webber rd', 'Lambton', 'Germiston',
  'Gauteng', 1400, 'Gordon', 'Ramsey', 'Gordon@gmail.com', '0824653214'),
('Surge Electrical Suppliers', 4852654753, '78 Deckama Avennue', 'Wadeville', 'Germiston',
  'Gauteng', 1428, 'Margret', 'Madison', 'Margret@gmail.com', '0835463215');
GO

--Insert product units
INSERT INTO dbo.Units
(UnitPer, UnitPerDescription)
VALUES
('m', 'One Meter'),
('pc', 'One Piece'),
('kg', 'One Kilogram'),
('l', 'One Liter');
GO

--Insert product categories
INSERT INTO dbo.Category
(CategoryName)
VALUES
('Hydraulics'),
('Wire'),
('Electronics'),
('Garden');
GO

--Insert Products
INSERT INTO dbo.Products
(ProductName,ProductDescription,VendorID,VendorProductName, UnitPrice,
 ReorderPoint,UnitPerID,UnitWeight,	CategoryID)
VALUES
('5m Red 0.5 Wire', 'One roll of red 0.5 wire', 20002, '0.5-5-red', 100, 0, 2, 0.2, 2),
('Large Spade', 'Large Yellow Spade', 20001, 'Spade-L-Yellow', 550, 0, 2, 1.5, 4),
('Large Garden Fork', 'Large Yellow Garden Fork', 20001, 'Fork-L-Yellow', 456, 0, 2, 1.25, 4),
('Bleeding Nipple small', 'Small Bleeding Nipple', 20000, 'H236598', 0.1, 0, 2, 0.01, 1);
GO

--*********************************************************************************************************
--Insert Purchase orders
INSERT INTO dbo.PurchaseOrderHeader
(VendorID, VendorReference, OrderDate, OrderAmount, VATPercentage,
 VATAmount, TotalAmount, RequiredDate, OrderStatusID, IsImport)
 VALUES
 (20002, 89000236, GETDATE(), 400, 0.15,
  60.0, 460, DATEADD(WEEK, 2, GetDate()), 1, 0);

DECLARE @ID BIGINT;
SELECT @ID = MAX(PurchaseOrderID) FROM dbo.PurchaseOrderHeader;
  --Insert Order lines
INSERT INTO dbo.PurchaseOrderDetail
(PurchaseOrderID, ProductID, Quantity, UnitCost, UnitFreightCost)
VALUES
(@ID, 10000, 5, 80, 0);
GO

--Insert Purchase orders
INSERT INTO dbo.PurchaseOrderHeader
(VendorID, VendorReference, OrderDate, OrderAmount, VATPercentage,
 VATAmount, TotalAmount, RequiredDate, OrderStatusID, IsImport)
 VALUES
 (20002, 89000236, GETDATE(), 400, 0.15,
  60.0, 460, DATEADD(WEEK, 2, GetDate()), 1, 0);

DECLARE @ID BIGINT;
SELECT @ID = MAX(PurchaseOrderID) FROM dbo.PurchaseOrderHeader;
  --Insert Order lines
INSERT INTO dbo.PurchaseOrderDetail
(PurchaseOrderID, ProductID, Quantity, UnitCost, UnitFreightCost)
VALUES
(@ID, 10000, 5, 80, 0);
GO

--Receipt
INSERT INTO dbo.Receipts
(PurchaseOrderID, ProductID,QuantityReceipted, UnitCost)
VALUES
(450000000, 10000, 3, 80);
GO

--Mark line as complete
UPDATE dbo.PurchaseOrderDetail
SET LineFilled = 1
WHERE PurchaseOrderID = 450000000 AND ProductID = 10000;
GO

--Insert another purchase order to test the stored Procedure used to update OnOrder quantity
INSERT INTO dbo.PurchaseOrderHeader
(VendorID, VendorReference, OrderDate, OrderAmount, VATPercentage,
 VATAmount, TotalAmount, RequiredDate, OrderStatusID, IsImport)
 VALUES
 (20002, 89000236, GETDATE(), 400, 0.15,
  60.0, 460, DATEADD(WEEK, 2, GetDate()), 1, 0);

DECLARE @PurchaseOrderID BIGINT;
SELECT @PurchaseOrderID = MAX(PurchaseOrderID) FROM dbo.PurchaseOrderHeader;

DECLARE @ProductID INT = 10000;
DECLARE @Quantity INT = 5;
DECLARE @UnitCost MONEY = 80;
DECLARE @UnitFreightCost MONEY = 0;
DECLARE @LineFilled BIT = 0;


EXECUTE dbo.usp_InsertPurchaseOrderDetail @PurchaseOrderID, @ProductID, @Quantity, @UnitCost, @UnitFreightCost, @LineFilled
  --Insert Order lines
--****************************************************************************************************************