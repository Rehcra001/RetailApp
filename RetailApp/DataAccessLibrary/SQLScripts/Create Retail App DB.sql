USE [master]
GO


/****** Object:  Database [RetailAppDB]    Script Date: 2023/09/15 08:18:30 ******/
CREATE DATABASE [RetailAppDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'RetailAppDB', FILENAME = N'C:\VCSDB\RetailAppDB.mdf' , SIZE = 8192KB , MAXSIZE = 10240000KB , FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'RetailAppDB_log', FILENAME = N'C:\VCSDB\RetailAppDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 10240000KB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [RetailAppDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [RetailAppDB] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [RetailAppDB] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [RetailAppDB] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [RetailAppDB] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [RetailAppDB] SET ARITHABORT OFF 
GO

ALTER DATABASE [RetailAppDB] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [RetailAppDB] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [RetailAppDB] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [RetailAppDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [RetailAppDB] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [RetailAppDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [RetailAppDB] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [RetailAppDB] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [RetailAppDB] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [RetailAppDB] SET  DISABLE_BROKER 
GO

ALTER DATABASE [RetailAppDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [RetailAppDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [RetailAppDB] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [RetailAppDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [RetailAppDB] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [RetailAppDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [RetailAppDB] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [RetailAppDB] SET RECOVERY FULL 
GO

ALTER DATABASE [RetailAppDB] SET  MULTI_USER 
GO

ALTER DATABASE [RetailAppDB] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [RetailAppDB] SET DB_CHAINING OFF 
GO

ALTER DATABASE [RetailAppDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [RetailAppDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO

ALTER DATABASE [RetailAppDB] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [RetailAppDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO

ALTER DATABASE [RetailAppDB] SET QUERY_STORE = OFF
GO

ALTER DATABASE [RetailAppDB] SET  READ_WRITE 
GO

USE RetailAppDB;
GO
--CREATE TABLES***************************************************************

--Customers
CREATE TABLE dbo.Customers
(
	CustomerID INT IDENTITY(10000, 1) PRIMARY KEY CLUSTERED,
	FirstName NVARCHAR(100) NOT NULL,
	LastName NVARCHAR(100) NOT NULL,
	AddressLine1 NVARCHAR(255) NOT NULL,
	AddressLine2 NVARCHAR(255) NULL,
	City NVARCHAR(50) NOT NULL,
	Province NVARCHAR (50) NOT NULL,
	PostalCode NVARCHAR (10) NOT NULL,
	EMailAddress NVARCHAR(255) NOT NULL,
	PhoneNumber NVARCHAR (10) NOT NULL
);
GO

--SalesOrderHeader
CREATE TABLE dbo.SalesOrderHeader
(
	SalesOrderID INT IDENTITY(200000000, 1) PRIMARY KEY,
	CustomerID INT NOT NULL,
	CustomerPurchaseOrder NVARCHAR(100) NULL,
	OrderDate DATE NOT NULL,
	OrderAmount MONEY NOT NULL,
	Invoiced BIT DEFAULT(0) NOT NULL
);
GO

--SalesOrderDetail
CREATE TABLE dbo.SalesOrderDetail
(
	SalesOrderID INT NOT NULL,
	ProductID INT NOT NULL,
	Quantity INT NOT NULL,
	UnitPrice MONEY NOT NULL,
	UnitCost MONEY NOT NULL	
);
GO


--Products
CREATE TABLE dbo.Products
(
	ProductID INT IDENTITY(10000, 1) PRIMARY KEY CLUSTERED,
	ProductName NVARCHAR(100) NOT NULL,
	ProductDescription NVARCHAR(255) NOT NULL,
	VendorID INT NOT NULL,
	VendorProductName NVARCHAR(100) NOT NULL,
	UnitPrice MONEY DEFAULT(0) NOT NULL,
	UnitCost MONEY DEFAULT(0) NOT NULL,
	OnHand INT DEFAULT(0) NOT NULL,
	OnOrder INT DEFAULT(0) NOT NULL,
	ReorderPoint INT DEFAULT(0) NOT NULL,
	UnitPerID INT NOT NULL,
	UnitWeight NUMERIC DEFAULT(0) NOT NULL,
	Obsolete BIT DEFAULT(0) NOT NULL
);
GO

--Units
CREATE TABLE dbo.Units
(
 UnitPerID INT IDENTITY PRIMARY KEY,
 UnitPer NVARCHAR(10) NOT NULL,
 UnitPerDescription NVARCHAR(30) NOT NULL
);
GO

----StockTransactions
--CREATE TABLE dbo.StockTransactions
--(
--	TransactionID INT IDENTITY(1,1) PRIMARY KEY,
--	TransactionType NCHAR(1) NOT NULL,	--'p'for purchase and 's' for sale
--	OrderID INT NOT NULL,
--	ProductID INT NOT NULL,
--	TransactionDate Date NOT NULL,	
--	Quantity INT NOT NULL
--);
--GO


--Purchase line receipts


--PurchaseOrderHeader
CREATE TABLE dbo.PurchaseOrderHeader
(
	PurchaseOrderID INT IDENTITY(450000000,1) PRIMARY KEY,
	VendorID INT NOT NULL,
	VendorReference NVARCHAR(20),
	OrderDate DATETIME NOT NULL,
	OrderAmount MONEY DEFAULT(0) NOT NULL,
	VATPercentage NUMERIC DEFAULT(0) NOT NULL,
	VATAmount MONEY DEFAULT(0) NOT NULL,
	TotalAmount MONEY DEFAULT(0) NOT NULL,
	RequiredDate DATE NOT NULL,
	OrderFilled BIT DEFAULT(0) NOT NULL
);
GO

--PurchaseOrderDetail
CREATE TABLE dbo.PurchaseOrderDetail
(
	PurchaseOrderID INT NOT NULL,
	ProductID INT NOT NULL,
	Quantity INT NOT NULL,
	UnitCost MONEY NOT NULL,
	UnitFreightCost MONEY DEFAULT(0) NOT NULL,
	QuantityReceipted INT DEFAULT(0) NOT NULL,
	LineFilled BIT DEFAULT(0) NOT NULL
);
GO

--Vendors
CREATE TABLE dbo.Vendors
(
	VendorID INT IDENTITY(20000, 1) PRIMARY KEY,
	FirstName NVARCHAR(100) NOT NULL,
	LastName NVARCHAR(100) NOT NULL,
	CompanyName NVARCHAR(100) NOT NULL,
	AddressLine1 NVARCHAR(255) NOT NULL,
	AddressLine2 NVARCHAR(255) NULL,
	City NVARCHAR(50) NOT NULL,
	Province NVARCHAR (50) NOT NULL,
	PostalCode NVARCHAR (10) NOT NULL,
	EMailAddress NVARCHAR(255) NOT NULL,
	PhoneNumber NVARCHAR (10) NOT NULL
);
GO

--CONSTRAINTS***********************************************
--Sales Order Header
ALTER TABLE dbo.SalesOrderHeader
ADD CONSTRAINT FK_SalesOrderHeader_Customers_CustomerID
FOREIGN KEY (CustomerID)
REFERENCES dbo.Customers(CustomerID)
GO


--Sales Order Detail
ALTER TABLE dbo.SalesOrderDetail
ADD CONSTRAINT PK_SalesOrderDetail_SalesOrderID_ProductID
PRIMARY KEY (SalesOrderID, ProductID);
GO

ALTER TABLE dbo.SalesOrderDetail
ADD CONSTRAINT FK_SalesOrderDetail_SalesOrderHeader_SalesOrderID
FOREIGN KEY (SalesOrderID)
REFERENCES dbo.SalesOrderHeader(SalesOrderID);
GO

ALTER TABLE dbo.SalesOrderDetail
ADD CONSTRAINT FK_SalesOrderDetail_Products_ProductID
FOREIGN KEY (ProductID)
REFERENCES dbo.Products(ProductID);
GO

--Products
ALTER TABLE dbo.Products
ADD CONSTRAINT FK_Products_Vendors_VendorID
FOREIGN KEY (VendorID)
REFERENCES dbo.Vendors(VendorID);
GO

ALTER TABLE dbo.Products
ADD CONSTRAINT FK_Products_Units_UnitPerID
FOREIGN KEY (UnitPerID)
REFERENCES dbo.Units(UnitPerID);
GO

--Purchase Order Header
ALTER TABLE dbo.PurchaseOrderHeader
ADD CONSTRAINT FK_PurchaseOrderHeader_Vendors_VendorID
FOREIGN KEY (VendorID)
REFERENCES dbo.Vendors(VendorID);
GO

--Purchase Order Detail
ALTER TABLE dbo.PurchaseOrderDetail
ADD CONSTRAINT PK_PurchaseOrderDetail_PurchaseOrderID_ProductID
PRIMARY KEY (PurchaseOrderID, ProductID);
GO

ALTER TABLE dbo.PurchaseOrderDetail
ADD CONSTRAINT FK_PurchaseOrderDetail_PurchaseOrderHeader_PurchaseOrderID
FOREIGN KEY (PurchaseOrderID)
REFERENCES dbo.PurchaseOrderHeader(PurchaseOrderID);
GO

ALTER TABLE dbo.PurchaseOrderDetail
ADD CONSTRAINT FK_PurchaseOrderDetail_Products_ProductID
FOREIGN KEY (ProductID)
REFERENCES dbo.Products(ProductID);
GO

----Stock Transactions
--ALTER TABLE dbo.StockTransactions
--ADD CONSTRAINT Unique_OrderID_ProductID
--UNIQUE(OrderID, ProductID);
--GO
----ALTER TABLE dbo.StockTransactions
----ADD CONSTRAINT PK_StockTransactions_TransactionType_OrderID_ProductID
----PRIMARY KEY (TransactionType, OrderID, ProductID);
----GO

--ALTER TABLE dbo.StockTransactions
--ADD CONSTRAINT FK_StockTransactions_PurchaseOrderDetail_OrderID_PurchaseOrderID_ProductID
--FOREIGN KEY (OrderID, ProductID)
--REFERENCES dbo.PurchaseOrderDetail(PurchaseOrderID, ProductID);
--GO

--ALTER TABLE dbo.StockTransactions
--ADD CONSTRAINT FK_StockTransactions_SalesOrderDetail_OrderID_SalesOrderID_ProductID
--FOREIGN KEY (OrderID, ProductID)
--REFERENCES dbo.SalesOrderDetail(SalesOrderID, ProductID);
--GO