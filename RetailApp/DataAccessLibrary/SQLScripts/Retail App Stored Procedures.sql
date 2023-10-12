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
	@Country NVARCHAR(100),
	@PostalCode NVARCHAR(15),	
	@EMailAddress NVARCHAR(255),
	@PhoneNumber NVARCHAR(20),
	@InternationalVendor BIT
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
				Country,
				PostalCode,
				EMailAddress,
				PhoneNumber,
				InternationalVendor
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
				@Country,
				@PostalCode,
				@EMailAddress,
				@PhoneNumber,
				@InternationalVendor
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
	@Country NVARCHAR(100),
	@PostalCode NVARCHAR(10),
	@EMailAddress NVARCHAR(255),
	@PhoneNumber NVARCHAR(10),
	@InternationalVendor BIT
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
				Country = @Country,
				PostalCode = @PostalCode,
				EMailAddress = @EMailAddress,
				PhoneNumber = @PhoneNumber,
				InternationalVendor = @InternationalVendor
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
			   City, Province, Country, PostalCode, EMailAddress, PhoneNumber, InternationalVendor
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
			   City, Province, Country, PostalCode, EMailAddress, PhoneNumber, InternationalVendor
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
								  WHERE ProductID = @ProductID AND OrderLineStatusID = 1)--Open
	),
	QtyOnOrder AS
	(
		SELECT ISNULL(SUM(pd.Quantity),0) AS QtyOrdered
		FROM dbo.PurchaseOrderDetail AS pd
		WHERE ProductID = @ProductID AND OrderLineStatusID = 1--Open
	)
	UPDATE dbo.Products
	SET OnOrder = (QtyOrdered - TotalQtyReceipted)
	FROM QtyOnOrder
	CROSS JOIN QtyReceipted
	WHERE ProductID = @ProductID;
END;
GO

--Update the specified product with current number in stock
--Use the Inventory transactions
--Sums all Inventory transactions of ProductID
CREATE PROCEDURE dbo.usp_UpdateProductOnHand
(
	@ProductID int
)AS
BEGIN
	SET NOCOUNT ON;

	--Get the sum of all transactions on ProductID
	DECLARE @OnHand INT;
	SELECT @OnHand = ISNULL(SUM(Quantity),0)
	FROM dbo.InventoryTransactions 
	WHERE ProductID = @ProductID;

	--Update Product quantity on hand
	UPDATE dbo.Products
	SET OnHand = @OnHand
	WHERE ProductID = @ProductID;
END;
GO

--Update the specified product with a weighted unit cost 
--of the products currently in stock
--based on First In First Out Principle


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

--**********Receipts**********
--Returns a list of receipts per PurchaseOrderID
--or an error message on failure
CREATE PROCEDURE dbo.ups_GetReceiptsByPurchaseOrderID
(
	@PurchaseOrderID BIGINT
)AS
BEGIN
	BEGIN TRY
		SET NOCOUNT ON;

		SELECT ReceiptID, PurchaseOrderID, ProductID,
			   ReceiptDate, QuantityReceipted, UnitCost
		FROM Receipts
		WHERE PurchaseOrderID = @PurchaseOrderID;
	END TRY

	BEGIN CATCH
		SELECT ERROR_MESSAGE() AS Message;
	END CATCH;
END;
GO

--Returns a ReceiptID on success
--or an error message on failure
--it also updates the product of productID with quantity on hand and on order
--and inserts a receipt transaction into the InventoryTransactions table
CREATE PROCEDURE dbo.usp_InsertReceipt
(
	@PurchaseOrderID BIGINT,
	@ProductID INT,
	@QuantityReceipted INT,
	@UnitCost MONEY
)AS
BEGIN
	BEGIN TRY
		BEGIN TRAN
			SET NOCOUNT ON;

			INSERT INTO dbo.Receipts
			(
				PurchaseOrderID,
				ProductID,
				QuantityReceipted,
				UnitCost
			)
			VALUES
			(
				@PurchaseOrderID,
				@ProductID,
				@QuantityReceipted,
				@UnitCost
			);

			--Insert Inventory transaction
			--Get the ID of the inserted receipt
			DECLARE @ReceiptID INT;
			SET @ReceiptID = SCOPE_IDENTITY();

			--Get the receipt date
			DECLARE @ReceiptDate DATETIME;
			SELECT @ReceiptDate = ReceiptDate
			FROM dbo.Receipts
			WHERE ReceiptID = @ReceiptID;

			EXECUTE dbo.usp_InsertInventoryTransactions 'R', @ReceiptDate, @ProductID, @ReceiptID, @QuantityReceipted;

			--Update Product On Order			
			--Update the number of this product on order
			EXECUTE dbo.usp_UpdateProductOnOrder @ProductID;

			--Update Product On Hand
			EXECUTE dbo.usp_UpdateProductOnHand @ProductID;

			--Update Purchase order detail with quantity receipted
			EXECUTE dbo.usp_UpdatePurchaseOrderDetailQuantityReceipted @PurchaseOrderID, @ProductID;

			--Return receipt id on success
			SELECT @ReceiptID AS ID;
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

--Inserts a line into the Inventory transactions table
--Returns a message 'No Error' on success
--or an error message on failure
CREATE PROCEDURE dbo.usp_InsertInventoryTransactions
(
	@TransactionType CHAR(1),
	@TransactionDate DATETIME,
	@ProductID INT,
	@OrderID INT, --Goods receipt ID or Goods issue ID
	@Quantity INT
)AS
BEGIN
	BEGIN TRY
		SET NOCOUNT ON;

		INSERT INTO dbo.InventoryTransactions
		(
			TransactionType,
			TransactionDate,
			ProductID,
			OrderID,
			Quantity
		)
		VALUES
		(
			@TransactionType,
			@TransactionDate,
			@ProductID,
			@OrderID,
			@Quantity
		);
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

--GetByOrderStatus
--Returns a list of purchase orders with specific order status
--or an error message on failure
CREATE PROCEDURE dbo.usp_GetPurchaseOrderHeaderByOrderStatus
(
	@OrderStatusID INT
)AS
BEGIN
	BEGIN TRY
		SET NOCOUNT ON;

		SELECT PurchaseOrderID, VendorID, VendorReference, OrderDate,
			   OrderAmount, VATPercentage, VATAmount, TotalAmount,
			   RequiredDate, OrderStatusID, IsImport
		FROM dbo.PurchaseOrderHeader
		WHERE OrderStatusID = @OrderStatusID;
	END TRY

	BEGIN CATCH
		SELECT ERROR_MESSAGE() AS Message;
	END CATCH;
END;
GO

--GetByOrderStatus
--Returns a list of purchase orders with specific vendor
--or an error message on failure
CREATE PROCEDURE dbo.usp_GetPurchaseOrderHeaderVendorID
(
	@VendorID INT
)AS
BEGIN
	BEGIN TRY
		SET NOCOUNT ON;

		SELECT PurchaseOrderID, VendorID, VendorReference, OrderDate,
			   OrderAmount, VATPercentage, VATAmount, TotalAmount,
			   RequiredDate, OrderStatusID, IsImport
		FROM dbo.PurchaseOrderHeader
		WHERE VendorID = @VendorID;
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
	@OrderLineStatusID INT
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
				OrderLineStatusID
			)
			VALUES
			(
				@PurchaseOrderID,
				@ProductID,
				@Quantity,
				@UnitCost,
				@UnitFreightCost,
				@OrderLineStatusID
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
	@OrderLineStatusID INT
)AS
BEGIN
	BEGIN TRY
		BEGIN TRAN
			SET NOCOUNT ON;

			UPDATE dbo.PurchaseOrderDetail
			SET Quantity = @Quantity,
				UnitCost = @UnitCost,
				UnitFreightCost = @UnitFreightCost,
				OrderLineStatusID = @OrderLineStatusID
			WHERE PurchaseOrderID = @PurchaseOrderID AND ProductID = @ProductID;

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

--GetAll
--Returns a list of purchase order details
--or an error message on failure
CREATE PROCEDURE dbo.usp_GetAllPurchaseOrderDetails AS
BEGIN
	BEGIN TRY
		SET NOCOUNT ON;

		SELECT PurchaseOrderID, ProductID, Quantity, UnitCost, UnitFreightCost, QuantityReceipted, OrderLineStatusID
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
		SELECT PurchaseOrderID, ProductID, Quantity, UnitCost, UnitFreightCost, QuantityReceipted, OrderLineStatusID
		FROM dbo.PurchaseOrderDetail
		WHERE PurchaseOrderID = @PurchaseOrderID;
	END TRY

	BEGIN CATCH
		SELECT ERROR_MESSAGE() AS Message;
	END CATCH;
END;
GO

--Update a specific purchase order detail of ProductID with qty receipted
CREATE PROCEDURE dbo.usp_UpdatePurchaseOrderDetailQuantityReceipted
(
	@PurchaseOrderID BIGINT,
	@ProductID INT
)AS
BEGIN
	--First sum the amount receipt so far
	;WITH QtyReceipted AS(
		SELECT ISNULL(SUM(QuantityReceipted), 0) AS QtyReceipted
		FROM Receipts
		WHERE PurchaseOrderID = @PurchaseOrderID AND ProductID = @ProductID)

	--Update the purchase order detail
	UPDATE dbo.PurchaseOrderDetail
	SET QuantityReceipted = (SELECT QtyReceipted FROM QtyReceipted)
	WHERE PurchaseOrderID = @PurchaseOrderID AND ProductID = @ProductID;
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
CREATE PROCEDURE dbo.usp_GetAllStatus AS
BEGIN
	BEGIN TRY
		SET NOCOUNT ON;

		SELECT StatusID, [Status]
		FROM StatusLK;
	END TRY

	BEGIN CATCH
		SELECT ERROR_MESSAGE() AS Message;
	END CATCH;
END;
GO

--GetByID
--Returns an order status
--or an error message on failure
CREATE PROCEDURE dbo.usp_GetStatusByID
(
	@StatusID INT
)AS
BEGIN
	BEGIN TRY
		SET NOCOUNT ON;

		SELECT StatusID, Status
		FROM StatusLK
		WHERE StatusID = @StatusID;
	END TRY

	BEGIN CATCH
		SELECT ERROR_MESSAGE() AS Message;
	END CATCH;
END;
GO