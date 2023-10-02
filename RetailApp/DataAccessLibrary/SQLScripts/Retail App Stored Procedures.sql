USE RetailAppDB;
GO

--**********Vendors**********
CREATE PROCEDURE dbo.usp_InsertVendor
(
	@VatRegistrationNumber NVARCHAR(10),
	@FirstName NVARCHAR(100),
	@LastName NVARCHAR(100),
	@CompanyName NVARCHAR(100),
	@AddressLine1 NVARCHAR(255),
	@AddressLine2 NVARCHAR(255),
	@City NVARCHAR(50),
	@Province NVARCHAR(50),
	@PostalCode NVARCHAR(10),
	@EMailAddress NVARCHAR(255),
	@PhoneNumber NVARCHAR(10)
)AS
BEGIN
	BEGIN TRY
		BEGIN TRAN
			SET NOCOUNT ON;

			INSERT INTO dbo.Vendors
			(
				VatRegistrationNumber,
				FirstName,
				LastName,
				CompanyName,
				AddressLine1,
				AddressLine2,
				City,
				Province,
				PostalCode,
				EMailAddress,
				PhoneNumber
			)
			VALUES
			(
				@VatRegistrationNumber,
				@FirstName,
				@LastName,
				@CompanyName,
				@AddressLine1,
				@AddressLine2,
				@City,
				@Province,
				@PostalCode,
				@EMailAddress,
				@PhoneNumber
			);

			SELECT SCOPE_IDENTITY() AS ID;
		COMMIT TRAN
	END TRY

	BEGIN CATCH
		IF (@@TRANCOUNT > 0)
		BEGIN
			ROLLBACK TRAN;
		END
		SELECT ERROR_MESSAGE() AS Message;
	END CATCH;
END;
GO

CREATE PROCEDURE dbo.usp_UpdateVendor
(
	@VendorID INT,
	@VatRegistrationNumber NVARCHAR(10),
	@FirstName NVARCHAR(100),
	@LastName NVARCHAR(100),
	@CompanyName NVARCHAR(100),
	@AddressLine1 NVARCHAR(255),
	@AddressLine2 NVARCHAR(255),
	@City NVARCHAR(50),
	@Province NVARCHAR(50),
	@PostalCode NVARCHAR(10),
	@EMailAddress NVARCHAR(255),
	@PhoneNumber NVARCHAR(10)
)AS
BEGIN
	BEGIN TRY
		BEGIN TRAN
			SET NOCOUNT ON;

			UPDATE dbo.Vendors
			SET	VatRegistrationNumber = @VatRegistrationNumber,
				FirstName = @FirstName,
				LastName = @LastName,
				CompanyName = @CompanyName,
				AddressLine1 = @AddressLine1,
				AddressLine2 = @AddressLine2,
				City = @City,
				Province = @Province,
				PostalCode = @PostalCode,
				EMailAddress = @EMailAddress,
				PhoneNumber = @PhoneNumber
			WHERE VendorID	= @VendorID;

			SELECT 'No Error' AS Message;
		COMMIT TRAN
	END TRY

	BEGIN CATCH
		IF (@@TRANCOUNT > 0)
		BEGIN
			ROLLBACK TRAN;
		END
		SELECT ERROR_MESSAGE() AS Message;
	END CATCH;
END;
GO

CREATE PROCEDURE dbo.usp_DeleteVendor
(
	@VendorID INT
)AS
BEGIN
	BEGIN TRY
		BEGIN TRAN
			DELETE FROM dbo.Vendors
			WHERE VendorID = @VendorID;

			SELECT 'No Error' AS Message;
		COMMIT TRAN;
	END TRY

	BEGIN CATCH
		IF (@@TRANCOUNT > 0)
		BEGIN
			ROLLBACK TRAN;
		END;
		SELECT ERROR_MESSAGE() AS Message;
	END CATCH;
END;
GO

CREATE PROCEDURE dbo.usp_GetAllVendors AS
BEGIN
	BEGIN TRY
		SET NOCOUNT ON;

		SELECT VendorID, VatRegistrationNumber, FirstName, LastName, CompanyName, AddressLine1, AddressLine2,
			   City, Province, PostalCode, EMailAddress, PhoneNumber
		FROM dbo.Vendors
		ORDER BY CompanyName;
	END TRY

	BEGIN CATCH
		SELECT ERROR_MESSAGE() AS Message;
	END CATCH;
END;
GO

CREATE PROCEDURE dbo.usp_GetVendorByID
(
	@VendorID INT
)AS
BEGIN
	BEGIN TRY
		SET NOCOUNT ON;

		SELECT VendorID, VatRegistrationNumber, FirstName, LastName, CompanyName, AddressLine1, AddressLine2,
			   City, Province, PostalCode, EMailAddress, PhoneNumber
		FROM dbo.Vendors
		WHERE VendorID = @VendorID;
	END TRY

	BEGIN CATCH
		SELECT ERROR_MESSAGE() AS Message;
	END CATCH;
END;
GO

--**********Customers**********
CREATE PROCEDURE dbo.usp_InsertCustomer
(
	@VatRegistrationNumber NVARCHAR(10),
	@FirstName NVARCHAR(100),
	@LastName NVARCHAR(100),
	@CompanyName NVARCHAR(100),
	@AddressLine1 NVARCHAR(255),
	@AddressLine2 NVARCHAR(255),
	@City NVARCHAR(50),
	@Province NVARCHAR(50),
	@PostalCode NVARCHAR(10),
	@EMailAddress NVARCHAR(255),
	@PhoneNumber NVARCHAR(10)
)AS
BEGIN
	BEGIN TRY
		BEGIN TRAN
			SET NOCOUNT ON;

			INSERT INTO dbo.Customers
			(
				VatRegistrationNumber,
				FirstName,
				LastName,
				CompanyName,
				AddressLine1,
				AddressLine2,
				City,
				Province,
				PostalCode,
				EMailAddress,
				PhoneNumber
			)
			VALUES
			(
				@VatRegistrationNumber,
				@FirstName,
				@LastName,
				@CompanyName,
				@AddressLine1,
				@AddressLine2,
				@City,
				@Province,
				@PostalCode,
				@EMailAddress,
				@PhoneNumber
			);

			SELECT SCOPE_IDENTITY() AS ID;
		COMMIT TRAN
	END TRY

	BEGIN CATCH
		IF (@@TRANCOUNT > 0)
		BEGIN
			ROLLBACK TRAN;
		END
		SELECT ERROR_MESSAGE() AS Message;
	END CATCH;
END;
GO

CREATE PROCEDURE dbo.usp_UpdateCustomer
(
	@CustomerID INT,
	@VatRegistrationNumber NVARCHAR(10),
	@FirstName NVARCHAR(100),
	@LastName NVARCHAR(100),
	@CompanyName NVARCHAR(100),
	@AddressLine1 NVARCHAR(255),
	@AddressLine2 NVARCHAR(255),
	@City NVARCHAR(50),
	@Province NVARCHAR(50),
	@PostalCode NVARCHAR(10),
	@EMailAddress NVARCHAR(255),
	@PhoneNumber NVARCHAR(10)
)AS
BEGIN
	BEGIN TRY
		BEGIN TRAN
			SET NOCOUNT ON;

			UPDATE dbo.Customers
			SET	VatRegistrationNumber = @VatRegistrationNumber,
				FirstName = @FirstName,
				LastName = @LastName,
				CompanyName = @CompanyName,
				AddressLine1 = @AddressLine1,
				AddressLine2 = @AddressLine2,
				City = @City,
				Province = @Province,
				PostalCode = @PostalCode,
				EMailAddress = @EMailAddress,
				PhoneNumber = @PhoneNumber
			WHERE CustomerID	= @CustomerID;

			SELECT 'No Error' AS Message;
		COMMIT TRAN
	END TRY

	BEGIN CATCH
		IF (@@TRANCOUNT > 0)
		BEGIN
			ROLLBACK TRAN;
		END
		SELECT ERROR_MESSAGE() AS Message;
	END CATCH;
END;
GO

CREATE PROCEDURE dbo.usp_DeleteCustomer
(
	@CustomerID INT
)AS
BEGIN
	BEGIN TRY
		BEGIN TRAN
			DELETE FROM dbo.Customers
			WHERE CustomerID = @CustomerID;

			SELECT 'No Error' AS Message;
		COMMIT TRAN;
	END TRY

	BEGIN CATCH
		IF (@@TRANCOUNT > 0)
		BEGIN
			ROLLBACK TRAN;
		END;
		SELECT ERROR_MESSAGE() AS Message;
	END CATCH;
END;
GO

CREATE PROCEDURE dbo.usp_GetAllCustomers AS
BEGIN
	BEGIN TRY
		SET NOCOUNT ON;

		SELECT CustomerID, VatRegistrationNumber, FirstName, LastName, CompanyName, AddressLine1, AddressLine2,
			   City, Province, PostalCode, EMailAddress, PhoneNumber
		FROM dbo.Customers
		ORDER BY CompanyName;
	END TRY

	BEGIN CATCH
		SELECT ERROR_MESSAGE() AS Message;
	END CATCH;
END;
GO

CREATE PROCEDURE dbo.usp_GetCustomerByID
(
	@CustomerID INT
)AS
BEGIN
	BEGIN TRY
		SET NOCOUNT ON;

		SELECT CustomerID, VatRegistrationNumber, FirstName, LastName, CompanyName, AddressLine1, AddressLine2,
			   City, Province, PostalCode, EMailAddress, PhoneNumber
		FROM dbo.Customers
		WITH (NOLOCK)
		WHERE CustomerID = @CustomerID;
	END TRY

	BEGIN CATCH
		SELECT ERROR_MESSAGE() AS Message;
	END CATCH;
END;
GO


CREATE PROCEDURE dbo.usp_InsertCompanyDetail
(
	@VatRegistrationNumber NVARCHAR(10),
	@FirstName NVARCHAR(100),
	@LastName NVARCHAR(100),
	@CompanyName NVARCHAR(100),
	@AddressLine1 NVARCHAR(255),
	@AddressLine2 NVARCHAR(255),
	@City NVARCHAR(50),
	@Province NVARCHAR(50),
	@PostalCode NVARCHAR(10),
	@EMailAddress NVARCHAR(255),
	@PhoneNumber NVARCHAR(10)
)AS
BEGIN
	BEGIN TRY
		BEGIN TRAN
			SET NOCOUNT ON;

			INSERT INTO dbo.CompanyDetail
			(
				VatRegistrationNumber,
				FirstName,
				LastName,
				CompanyName,
				AddressLine1,
				AddressLine2,
				City,
				Province,
				PostalCode,
				EMailAddress,
				PhoneNumber
			)
			VALUES
			(
				@VatRegistrationNumber,
				@FirstName,
				@LastName,
				@CompanyName,
				@AddressLine1,
				@AddressLine2,
				@City,
				@Province,
				@PostalCode,
				@EMailAddress,
				@PhoneNumber
			);

			SELECT SCOPE_IDENTITY() AS ID;
		COMMIT TRAN
	END TRY

	BEGIN CATCH
		IF (@@TRANCOUNT > 0)
		BEGIN
			ROLLBACK TRAN;
		END
		SELECT ERROR_MESSAGE() AS Message;
	END CATCH;
END;
GO

CREATE PROCEDURE dbo.usp_UpdateCompanyDetail
(
	@CompanyID INT,
	@VatRegistrationNumber NVARCHAR(10),
	@FirstName NVARCHAR(100),
	@LastName NVARCHAR(100),
	@CompanyName NVARCHAR(100),
	@AddressLine1 NVARCHAR(255),
	@AddressLine2 NVARCHAR(255),
	@City NVARCHAR(50),
	@Province NVARCHAR(50),
	@PostalCode NVARCHAR(10),
	@EMailAddress NVARCHAR(255),
	@PhoneNumber NVARCHAR(10)
)AS
BEGIN
	BEGIN TRY
		BEGIN TRAN
			SET NOCOUNT ON;

			UPDATE dbo.CompanyDetail
			SET	VatRegistrationNumber = @VatRegistrationNumber,
				FirstName = @FirstName,
				LastName = @LastName,
				CompanyName = @CompanyName,
				AddressLine1 = @AddressLine1,
				AddressLine2 = @AddressLine2,
				City = @City,
				Province = @Province,
				PostalCode = @PostalCode,
				EMailAddress = @EMailAddress,
				PhoneNumber = @PhoneNumber
			WHERE CompanyID	= @CompanyID;

			SELECT 'No Error' AS Message;
		COMMIT TRAN
	END TRY

	BEGIN CATCH
		IF (@@TRANCOUNT > 0)
		BEGIN
			ROLLBACK TRAN;
		END
		SELECT ERROR_MESSAGE() AS Message;
	END CATCH;
END;
GO

CREATE PROCEDURE dbo.usp_GetCompanyDetail AS
BEGIN
	BEGIN TRY
		SELECT TOP(1) CompanyID, CompanyName, VatRegistrationNumber, AddressLine1, AddressLine2, City,
				      Province, PostalCode, FirstName, LastName, EMailAddress, PhoneNumber
		FROM dbo.CompanyDetail;
	END TRY

	BEGIN CATCH
		SELECT ERROR_MESSAGE() AS Message;
	END CATCH;
END;
GO

--**********Products**********
--Returns a ProductID on success or an error message on failure
CREATE PROCEDURE dbo.usp_InsertProduct
(
	--Only these fields need to be added
	--The remaining product fields will be
	--caluclated on reciept, creation of sale order
	--and invoicing where applicable
	@ProductName NVARCHAR(100),
	@ProductDescription NVARCHAR(255),
	@VendorID INT,
	@VendorProductName NVARCHAR(100),
	@UnitPrice MONEY,--Can be entered at a later stage
	@ReorderPoint INT, --Can be entered at a later stage
	@UnitPerID INT,
	@UnitWeight FLOAT,
	@CategoryID INT
)AS
BEGIN
	BEGIN TRY
		BEGIN TRAN
			SET NOCOUNT ON;

			INSERT INTO dbo.Products
			(
				ProductName,
				ProductDescription,
				VendorID,
				VendorProductName,
				UnitPrice,
				ReorderPoint,
				UnitPerID,
				UnitWeight,
				CategoryID
			)
			VALUES
			(
				@ProductName,
				@ProductDescription,
				@VendorID,
				@VendorProductName,
				@UnitPrice,
				@ReorderPoint,
				@UnitPerID,
				@UnitWeight,
				@CategoryID
			);

			SELECT SCOPE_IDENTITY() AS ID;
		COMMIT TRAN
	END TRY
		
	BEGIN CATCH
	IF (@@TRANCOUNT > 0)
	BEGIN
		ROLLBACK TRAN;
	END
	SELECT ERROR_MESSAGE() AS Message;
	END CATCH;
END;
GO

--Return a message on success or failure on updating
CREATE PROCEDURE dbo.usp_UpdateProduct
(
	--Only these fields need to be update
	--The remaining product fields will be
	--caluclated on reciept, creation of sale order
	--and invoicing where applicable
	@ProductID INT,
	@ProductName NVARCHAR(100),
	@ProductDescription NVARCHAR(255),
	@VendorID INT,
	@VendorProductName NVARCHAR(100),
	@UnitPrice MONEY,--Can be entered at a later stage
	@ReorderPoint INT,
	@UnitPerID INT,
	@UnitWeight FLOAT,
	@CategoryID INT
)AS
BEGIN
	BEGIN TRY
		BEGIN TRAN
			SET NOCOUNT ON;

			UPDATE dbo.Products
			SET ProductName = @ProductName,
				ProductDescription = @ProductDescription,
				VendorID = @VendorID,
				VendorProductName = @VendorProductName,
				UnitPrice = @UnitPrice,
				ReorderPoint = @ReorderPoint,
				UnitPerID = @UnitPerID,
				UnitWeight = @UnitWeight,
				CategoryID = @CategoryID
			WHERE ProductID = @ProductID;

			SELECT 'No Error' AS Message;	
		COMMIT TRAN
	END TRY

	BEGIN CATCH
		IF (@@TRANCOUNT > 0)
		BEGIN
			ROLLBACK TRAN;
		END;
		SELECT ERROR_MESSAGE() AS Message;
	END CATCH;
END;
GO

--Returns a list of all products on success
--or an error message on failure
CREATE PROCEDURE dbo.usp_GetAllProducts AS
BEGIN
	BEGIN TRY
		SELECT ProductID, ProductName, ProductDescription, VendorID, VendorProductName,
			   UnitPrice, UnitCost, OnHand, OnOrder, SalesDemand, ReorderPoint,
			   UnitPerID, UnitWeight, Obsolete, CategoryID
		FROM dbo.Products;
	END TRY

	BEGIN CATCH
		SELECT ERROR_MESSAGE() AS Message;
	END CATCH;
END;
GO

--Returns a list of all products on success
--or an error message on failure
CREATE PROCEDURE dbo.usp_GetProductsByVendorID 
(
	@VendorID INT
)AS
BEGIN
	BEGIN TRY
		SELECT ProductID, ProductName, ProductDescription, VendorID, VendorProductName,
			   UnitPrice, UnitCost, OnHand, OnOrder, SalesDemand, ReorderPoint,
			   UnitPerID, UnitWeight, Obsolete, CategoryID
		FROM dbo.Products
		WHERE VendorID = @VendorID;
	END TRY

	BEGIN CATCH
		SELECT ERROR_MESSAGE() AS Message;
	END CATCH;
END;
GO

--Returns a product on success
--or an error message on failure
CREATE PROCEDURE dbo.usp_GetProductByID
(
	@ProductID INT
)AS
BEGIN
	BEGIN TRY
		SELECT ProductID, ProductName, ProductDescription, VendorID, VendorProductName,
			   UnitPrice, UnitCost, OnHand, OnOrder, SalesDemand, ReorderPoint,
			   UnitPerID, UnitWeight, Obsolete, CategoryID
		FROM dbo.Products
		WHERE ProductID = @ProductID;
	END TRY

	BEGIN CATCH
		SELECT ERROR_MESSAGE() AS Message;
	END CATCH;
END;
GO

--Updates the specified product with the current number on order
CREATE PROCEDURE dbo.usp_UpdateProductOnOrder
(
	@ProductID INT
)AS
BEGIN
	;WITH QtyReceipted AS
	(
		SELECT ISNULL(SUM(QuantityReceipted), 0) AS TotalQtyReceipted
		FROM Receipts
		WHERE ProductID = @ProductID AND 
			  PurchaseOrderID IN (SELECT PurchaseOrderID
								  FROM PurchaseOrderDetail
								  WHERE ProductID = @ProductID AND LineFilled = 0)
	),
	QtyOnOrder AS
	(
		SELECT ISNULL(SUM(pd.Quantity),0) AS QtyOrdered
		FROM dbo.PurchaseOrderDetail AS pd
		WHERE ProductID = @ProductID AND LineFilled = 0
	)
	UPDATE dbo.Products
	SET OnOrder = (QtyOrdered - TotalQtyReceipted)
	FROM QtyOnOrder
	CROSS JOIN QtyReceipted
	WHERE ProductID = @ProductID;
END;
GO
--**********Units**********

--Returns the UnitPer ID on success
--or error message on failure
CREATE PROCEDURE dbo.usp_InsertUnit
(
	@UnitPer NVARCHAR(10),
	@UnitPerDescription NVARCHAR(30)
)AS
BEGIN
	BEGIN TRY
		BEGIN TRAN
			SET NOCOUNT ON;

			INSERT INTO dbo.Units
			(
				UnitPer,
				UnitPerDescription
			)
			VALUES
			(
				@UnitPer,
				@UnitPerDescription
			);

			SELECT SCOPE_IDENTITY() AS ID;
		COMMIT TRAN;
	END TRY

	BEGIN CATCH
		IF (@@TRANCOUNT > 0)
		BEGIN
			ROLLBACK TRAN;
		END;
		SELECT ERROR_MESSAGE() AS Message;
	END CATCH;
END;
GO

--Returns a message on success or failure of the update
CREATE PROCEDURE dbo.usp_UpdateUnit
(
	@UnitPerID INT,
	@UnitPer NVARCHAR(10),
	@UnitPerDescription NVARCHAR(30)
)AS
BEGIN
	BEGIN TRY
		BEGIN TRAN
			SET NOCOUNT ON;

			UPDATE dbo.Units
			SET UnitPer = @UnitPer,
				UnitPerDescription = @UnitPerDescription
			WHERE UnitPerID = @UnitPerID;

			SELECT 'No Error' AS Message;
		COMMIT TRAN;
	END TRY

	BEGIN CATCH
		IF (@@TRANCOUNT > 0)
		BEGIN
			ROLLBACK TRAN;
		END;
		SELECT ERROR_MESSAGE() AS Message;
	END CATCH;
END;
GO


--Returns a list of all units on success
--or an error message on failure
CREATE PROCEDURE dbo.usp_GetAllUnits AS
BEGIN
	BEGIN TRY
		SET NOCOUNT ON;

		SELECT UnitPerID, UnitPer, UnitPerDescription
		FROM dbo.Units;
	END TRY

	BEGIN CATCH
		SELECT ERROR_MESSAGE() AS Message;
	END CATCH;
END;
GO

--Returns a unitPer 
--or an error message on failure
CREATE PROCEDURE dbo.usp_GetUnitByID
(
	@UnitPerID INT
)AS
BEGIN
	BEGIN TRY
		SELECT UnitPerID, UnitPer, UnitPerDescription
		FROM dbo.Units
		WHERE UnitPerID = @UnitPerID;
	END TRY

	BEGIN CATCH
		SELECT ERROR_MESSAGE() AS Message;
	END CATCH;
END;
GO


--**********Category**********

--Returns a CategoryID on success
--or an error message on failure
CREATE PROCEDURE dbo.usp_InsertCategory
(
	@CategoryName NVARCHAR(50)
)AS
BEGIN
	BEGIN TRY
		BEGIN TRAN
			SET NOCOUNT ON;

			INSERT INTO dbo.Category (CategoryName)
			VALUES (@CategoryName);

			SELECT SCOPE_IDENTITY() AS ID;
		COMMIT TRAN;
	END TRY

	BEGIN CATCH
	 IF (@@TRANCOUNT > 0)
	 BEGIN
		ROLLBACK TRAN;
	 END;
	 SELECT ERROR_MESSAGE() AS Message;
	END CATCH;
END;
GO

--Returns a message "No Error" on success
--or an error message on failure
CREATE PROCEDURE dbo.usp_UpdateCategory
(
	@CategoryID INT,
	@CategoryName NVARCHAR(50)
)AS
BEGIN
	BEGIN TRY
		BEGIN TRAN
			UPDATE dbo.Category
			SET CategoryName = @CategoryName
			WHERE CategoryID = @CategoryID;

			SELECT 'No Error' AS Message;
		COMMIT TRAN;
	END TRY

	BEGIN CATCH
		IF (@@TRANCOUNT > 0)
		BEGIN
			ROLLBACK TRAN;
		END;
		SELECT ERROR_MESSAGE() AS Message;
	END CATCH;
END;
GO

--Returns a list of all categories on success
--or an error message on failure
CREATE PROCEDURE dbo.usp_GetAllCategories AS
BEGIN
	BEGIN TRY
		SET NOCOUNT ON;

		SELECT CategoryID, CategoryName
		FROM dbo.Category
		ORDER BY CategoryName;
	END TRY

	BEGIN CATCH
		SELECT ERROR_MESSAGE() AS Message;
	END CATCH;
END;
GO

--Returns a category by id on success
--or an error message on failure
CREATE PROCEDURE dbo.usp_GetCategoryByID
(
	@CategoryID INT
)AS
BEGIN
	BEGIN TRY
		SET NOCOUNT ON;

		SELECT CategoryID, CategoryName
		FROM dbo.Category
		WHERE CategoryID = @CategoryID;
	END TRY

	BEGIN CATCH
		SELECT ERROR_MESSAGE() AS Message;
	END CATCH;
END;
GO

--**********Inventory Transactions**********

--Returns a list of inventory transactions per ProductID
--or an error message on failure
CREATE PROCEDURE dbo.usp_GetInventoryTransactionsByProductID
(
	@ProductID INT
)AS
BEGIN
	BEGIN TRY
		SET NOCOUNT ON;

		SELECT TransactionID, TransactionType, TransactionDate,
			   ProductID, OrderID, Quantity
		FROM InventoryTransactions
		WHERE ProductID = @ProductID;
	END TRY

	BEGIN CATCH
		SELECT ERROR_MESSAGE() AS Message;
	END CATCH;
END;
GO

--**********Purchase Order Header**********
--Insert
--Returns the purchase order id
--or an error message on failure
CREATE PROCEDURE dbo.usp_InsertPurchaseOrderHeader
(
	@VendorID INT,
	@VendorReference NVARCHAR(20),
	@OrderDate DATETIME,
	@OrderAmount MONEY,
	@VATPercentage DECIMAL(5,4),
	@VATAmount MONEY,
	@TotalAmount MONEY,
	@RequiredDate DATE,
	@OrderStatusID INT = 1,
	@IsImport BIT
)AS
BEGIN
	BEGIN TRY
		BEGIN TRAN
			SET NOCOUNT ON;

			INSERT INTO dbo.PurchaseOrderHeader
			(
				VendorID,
				VendorReference,
				OrderDate,
				OrderAmount,
				VATPercentage,
				VATAmount,
				TotalAmount,
				RequiredDate,
				OrderStatusID,
				IsImport
			)
			VALUES
			(
				@VendorID,
				@VendorReference,
				@OrderDate,
				@OrderAmount,
				@VATPercentage,
				@VATAmount,
				@TotalAmount,
				@RequiredDate,
				@OrderStatusID,
				@IsImport
			);
			--Return purchase order id
			SELECT SCOPE_IDENTITY() AS ID;
		COMMIT TRAN;
	END TRY

	BEGIN CATCH
		IF (@@TRANCOUNT > 0)
		BEGIN
			ROLLBACK TRAN;
		END;
		SELECT ERROR_MESSAGE() AS Message;
	END CATCH;
END;
GO

--Update
--Returns 'No Error' on success
--or an error message on failure
CREATE PROCEDURE dbo.usp_UpdatePurchaseOrderHeader
(
	@PurchaseOrderID BIGINT,
	@VendorID INT,
	@VendorReference NVARCHAR(20),
	@OrderDate DATETIME,
	@OrderAmount MONEY,
	@VATPercentage DECIMAL(5,4),
	@VATAmount MONEY,
	@TotalAmount MONEY,
	@RequiredDate DATE,
	@OrderStatusID INT = 1,
	@IsImport BIT
)AS
BEGIN
	BEGIN TRY
		BEGIN TRAN
			SET NOCOUNT ON;

			UPDATE dbo.PurchaseOrderHeader
			SET 
				VendorID = @VendorID,
				VendorReference = @VendorReference,
				OrderDate = @OrderDate,
				OrderAmount = @OrderAmount,
				VATPercentage = @VATPercentage,
				VATAmount = @VATAmount,
				TotalAmount = @TotalAmount,
				RequiredDate = @RequiredDate,
				OrderStatusID = @OrderStatusID,
				IsImport = @IsImport
			WHERE PurchaseOrderID = @PurchaseOrderID;

			SELECT 'No Error' AS Message;
		COMMIT TRAN;
	END TRY

	BEGIN CATCH
		IF (@@TRANCOUNT > 0)
		BEGIN
			ROLLBACK TRAN;
		END;
		SELECT ERROR_MESSAGE() AS Message;
	END CATCH;
END;
GO

--GetAll
--Returns a list of Purchase order headers
--or an error message on failure
CREATE PROCEDURE dbo.usp_GetAllPurchaseOrderHeaders AS
BEGIN
	BEGIN TRY
		SET NOCOUNT ON;

		SELECT PurchaseOrderID, VendorID, VendorReference, OrderDate,
			   OrderAmount, VATPercentage, VATAmount, TotalAmount,
			   RequiredDate, OrderStatusID, IsImport
		FROM dbo.PurchaseOrderHeader;
	END TRY

	BEGIN CATCH
		SELECT ERROR_MESSAGE() AS Message;
	END CATCH;
END;
GO

--GetByID
--Returns a purchase order header
--or an error message on failure
CREATE PROCEDURE dbo.usp_GetPurchaseOrderHeaderByID
(
	@PurchaseOrderID BIGINT
)AS
BEGIN
	BEGIN TRY
		SET NOCOUNT ON;

		SELECT PurchaseOrderID, VendorID, VendorReference, OrderDate,
			   OrderAmount, VATPercentage, VATAmount, TotalAmount,
			   RequiredDate, OrderStatusID, IsImport
		FROM dbo.PurchaseOrderHeader
		WHERE PurchaseOrderID = @PurchaseOrderID;
	END TRY

	BEGIN CATCH
		SELECT ERROR_MESSAGE() AS Message;
	END CATCH;
END;
GO

--**********Purchase Order Details**********
--Insert
--Return 'No Error' on success
--or an error message on failure
CREATE PROCEDURE dbo.usp_InsertPurchaseOrderDetail
(
	@PurchaseOrderID BIGINT,
	@ProductID INT,
	@Quantity INT,
	@UnitCost MONEY,
	@UnitFreightCost MONEY,
	@LineFilled BIT
)AS
BEGIN
	BEGIN TRY
		BEGIN TRAN
			SET NOCOUNT ON;

			INSERT INTO dbo.PurchaseOrderDetail
			(
				PurchaseOrderID,
				ProductID,
				Quantity,
				UnitCost,
				UnitFreightCost,
				LineFilled
			)
			VALUES
			(
				@PurchaseOrderID,
				@ProductID,
				@Quantity,
				@UnitCost,
				@UnitFreightCost,
				@LineFilled
			);

			SELECT 'No Error' AS Message;

			--Update the number of this product on order
			EXECUTE dbo.usp_UpdateProductOnOrder @ProductID;
		COMMIT TRAN;
	END TRY

	BEGIN CATCH
		IF (@@TRANCOUNT > 0)
		BEGIN
			ROLLBACK TRAN;
		END;
		SELECT ERROR_MESSAGE() AS Message;
	END CATCH;
END;
GO

--Update
--Return 'No Error' on success
--or an error message on failure
CREATE PROCEDURE dbo.usp_UpdatePurchaseOrderDetail
(
	@PurchaseOrderID BIGINT,
	@ProductID INT,
	@Quantity INT,
	@UnitCost MONEY,
	@UnitFreightCost MONEY,
	@LineFilled BIT
)AS
BEGIN
	BEGIN TRY
		BEGIN TRAN
			SET NOCOUNT ON;

			UPDATE dbo.PurchaseOrderDetail
			SET Quantity = @Quantity,
				UnitCost = @UnitCost,
				UnitFreightCost = @UnitFreightCost,
				LineFilled = @LineFilled
			WHERE PurchaseOrderID = @PurchaseOrderID AND ProductID = @ProductID;

			SELECT 'No Error' AS Message;
		COMMIT TRAN;
	END TRY

	BEGIN CATCH
		IF (@@TRANCOUNT > 0)
		BEGIN
			ROLLBACK TRAN;
		END;
		SELECT ERROR_MESSAGE() AS Message;
	END CATCH;
END;
GO

--GetAll
--Returns a list of purchase order details
--or an error message on failure
CREATE PROCEDURE dbo.usp_GetAllPurchaseOrderDetails AS
BEGIN
	BEGIN TRY
		SET NOCOUNT ON;

		SELECT PurchaseOrderID, ProductID, Quantity, UnitCost, UnitFreightCost, LineFilled
		FROM dbo.PurchaseOrderDetail;
	END TRY

	BEGIN CATCH
		SELECT ERROR_MESSAGE() AS Message;
	END CATCH;
END;
GO

--GetByPurchaseOrderID
--Returns one or more purchase order lines that have PurchaseOrderID
CREATE PROCEDURE dbo.usp_GetPurchaseOrderDetailByPurchaseOrderID
(
	@PurchaseOrderID BIGINT
)AS
BEGIN
	BEGIN TRY
		SELECT PurchaseOrderID, ProductID, Quantity, UnitCost, UnitFreightCost, LineFilled
		FROM dbo.PurchaseOrderDetail
		WHERE PurchaseOrderID = @PurchaseOrderID;
	END TRY

	BEGIN CATCH
		SELECT ERROR_MESSAGE() AS Message;
	END CATCH;
END;
GO

--**********VAT**********
--Get VAT
--Returns the VAT
--or an error message on failure
CREATE PROCEDURE dbo.usp_GetVat AS
BEGIN
	BEGIN TRY
		SELECT TOP(1) VAT, VATDecimal
		FROM dbo.VAT;
	END TRY

	BEGIN CATCH
		SELECT ERROR_MESSAGE() AS Message;
	END CATCH;
END;
GO

--**********Order Status Look up**********
--GetAll
--Returns a list of Order status
--or an error message on failure
CREATE PROCEDURE dbo.usp_GetAllOrderStatus AS
BEGIN
	BEGIN TRY
		SET NOCOUNT ON;

		SELECT OrderStatusID, OrderStatus
		FROM OrderStatusLK;
	END TRY

	BEGIN CATCH
		SELECT ERROR_MESSAGE() AS Message;
	END CATCH;
END;
GO

--GetByID
--Returns an order status
--or an error message on failure
CREATE PROCEDURE dbo.usp_GetOrderStatusByID
(
	@OrderStatusID INT
)AS
BEGIN
	BEGIN TRY
		SET NOCOUNT ON;

		SELECT OrderStatusID, OrderStatus
		FROM OrderStatusLK
		WHERE OrderStatusID = @OrderStatusID;
	END TRY

	BEGIN CATCH
		SELECT ERROR_MESSAGE() AS Message;
	END CATCH;
END;
GO