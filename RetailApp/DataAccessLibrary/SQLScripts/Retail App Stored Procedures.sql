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
	@ProductID INT
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
CREATE PROCEDURE dbo.usp_UpdateProductWeightedUnitCostFIFO
(
	@ProductID INT
)AS
BEGIN
	DECLARE @QuantityInStock INT;
	DECLARE @CostPerUnit MONEY;

	--Check if there is any stock on hand for this product
	SELECT @QuantityInStock = ISNULL(SUM(QuantityReceipted),0) FROM Receipts WHERE ProductID = @ProductID;

	IF (@QuantityInStock > 0)
	BEGIN
		--There is stock. Determine the weight unit cost of the stock on hand
		;WITH productIssued AS
		(
			--sum up the number of this product issued out of stock/sold
			SELECT ISNULL(SUM(QuantityIssued),0) AS SumIssued
			FROM dbo.Issues
			WHERE ProductID = @ProductID
		),
		productReceipts AS 
		(
			--CREATE a record set of all vaild receipts for this product 
			SELECT QuantityReceipted, UnitCost, ReceiptDate,
			       (ISNULL(SUM(QuantityReceipted) OVER(ORDER BY ReceiptDate),0) + p.SumIssued) AS QtyLeft			   
			FROM dbo.Receipts AS r
			CROSS JOIN productIssued AS p
			WHERE ReverseReferenceID IS NULL AND r.ProductID = @ProductID
		),
		productReceiptsWithRowNum AS
		(
			--Only keep the rows with a QtyLeft greater than zero and then add row nummbers
			SELECT QuantityReceipted, UnitCost, QtyLeft,
				   ROW_NUMBER() OVER(ORDER BY ReceiptDate) AS RowNum
			FROM productReceipts
			WHERE QtyLeft > 0
		)
		SELECT @CostPerUnit = ISNULL(SUM(CASE WHEN RowNum = 1 THEN (QtyLeft * UnitCost) ELSE (QuantityReceipted * UnitCost) END) /
							  SUM(CASE WHEN RowNum = 1 THEN QtyLeft ELSE QuantityReceipted END),0)
		FROM productReceiptsWithRowNum;		
	END;

	ELSE --No Stock just set unit cost to zero
	BEGIN
		SET @CostPerUnit = 0;
	END

	--Update the weighted unit cost of the product
	UPDATE dbo.Products
	SET UnitCost = @CostPerUnit
	WHERE ProductID = @ProductID;
END;
GO

--Update the product sales demand based on open sales orders
CREATE PROCEDURE dbo.usp_UpdateProductSalesDemand
(
	@ProductID INT
)AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @SalesDemand INT;
	SELECT @SalesDemand = ISNULL(SUM(SOD.Quantity - SOD.QuantityInvoiced),0)
	FROM dbo.SalesOrderDetail AS SOD
	WHERE SOD.ProductID = @ProductID AND SOD.OrderLineStatusID = 1;--OPEN

	--Update product sales demand
	UPDATE dbo.Products
	SET SalesDemand = @SalesDemand
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
			   ReceiptDate, QuantityReceipted, UnitCost,
			   ReverseReferenceID
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

			--Check if receipt fills the order line
			--If yes then change line to filled
			EXECUTE dbo.usp_UpdatePurchaseOrderDetailToFilled @PurchaseOrderID, @ProductID;		
			
			--Update the weight unit cost of the product
			EXECUTE dbo.usp_UpdateProductWeightedUnitCostFIFO @ProductID

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

--Reverse a receipt
--Returns 'No Error' on success
--or error message on failure
CREATE PROCEDURE dbo.usp_ReverseReceiptByID
(
	@ReceiptID INT --Receipt to reverse
)AS
BEGIN
	BEGIN TRY
		BEGIN TRAN
			SET NOCOUNT ON;
			--First ensure it has not been reversed before
			DECLARE @IsReversed BIT;
			DECLARE @PurchaseOrderID BIGINT;
			DECLARE @ProductID INT;
			DECLARE @QuantityReceipted INT;
			DECLARE @UnitCost MONEY;
			DECLARE @ReversedReceiptID INT;

			SELECT @PurchaseOrderID = PurchaseOrderID,
				   @ProductID = ProductID,
				   @QuantityReceipted = QuantityReceipted,
				   @UnitCost = UnitCost,
				   @ReversedReceiptID = ReverseReferenceID
			FROM dbo.Receipts
			WHERE ReceiptID = @ReceiptID;

			
			IF (@ReversedReceiptID IS NULL)--Not reversed previously
			BEGIN				
				--Generate the new reversed receipt
				INSERT INTO dbo.Receipts
				(
					PurchaseOrderID,
					ProductID,
					QuantityReceipted,
					UnitCost,
					ReverseReferenceID
				)
				VALUES
				(
					@PurchaseOrderID,
					@ProductID,
					@QuantityReceipted * -1,
					@UnitCost,
					@ReceiptID
				);

				SET @ReversedReceiptID = SCOPE_IDENTITY();
				
				--Add the receipt id of the reversal to the original receipt
				UPDATE dbo.Receipts
				SET ReverseReferenceID = @ReversedReceiptID
				WHERE ReceiptID = @ReceiptID;

				--Check if the purchase order line was filled or completed
			--if yes then change to open
			--OPEN: 1
			--COMPLETE: 2
			--FILLED: 3
			--CANCELLED: 4
			DECLARE @OrderLineStatusID INT;

			SELECT @OrderLineStatusID = OrderLineStatusID
			FROM dbo.PurchaseOrderDetail
			WHERE PurchaseOrderID = @PurchaseOrderID AND ProductID = @ProductID;

			IF (@OrderLineStatusID = 2 OR @OrderLineStatusID = 3)
			BEGIN
				UPDATE dbo.PurchaseOrderDetail
				SET OrderLineStatusID = 1
				WHERE PurchaseOrderID = @PurchaseOrderID AND ProductID = @ProductID;
			END;

			--Check if the purchase order was filled or completed
			--if yes then change to open
			--OPEN: 1
			--COMPLETE: 2
			--FILLED: 3
			--CANCELLED: 4
			DECLARE @OrderStatusID INT;

			SELECT @OrderStatusID = OrderStatusID
			FROM dbo.PurchaseOrderHeader
			WHERE PurchaseOrderID = @PurchaseOrderID;
			
			IF (@OrderStatusID = 2 OR @OrderStatusID = 3)
			BEGIN
				UPDATE dbo.PurchaseOrderHeader
				SET OrderStatusID = 1
				WHERE PurchaseOrderID = @PurchaseOrderID;
			END;

			--Inventory transaction
			--Get the receipt date
			DECLARE @ReceiptDate DATETIME;
			SELECT @ReceiptDate = ReceiptDate
			FROM dbo.Receipts
			WHERE ReceiptID = @ReversedReceiptID;

			--change to negative value
			SET @QuantityReceipted = @QuantityReceipted * -1;
			
			--Add to Inventory transactions
			EXECUTE dbo.usp_InsertInventoryTransactions 'R', @ReceiptDate, @ProductID, @ReversedReceiptID, @QuantityReceipted;
			
			--Update the number of this product on order
			EXECUTE dbo.usp_UpdateProductOnOrder @ProductID;

			--Update Product On Hand
			EXECUTE dbo.usp_UpdateProductOnHand @ProductID;

			--Update Purchase order detail with quantity receipted
			EXECUTE dbo.usp_UpdatePurchaseOrderDetailQuantityReceipted @PurchaseOrderID, @ProductID;

			--Update the weight unit cost of the product
			EXECUTE dbo.usp_UpdateProductWeightedUnitCostFIFO @ProductID
			END;

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

--Check if all lines are filled 
--if yes then change the order header status to filled
CREATE PROCEDURE dbo.usp_UpdatePurchaseOrderHeaderToFilled
(
@PurchaseOrderID BIGINT
)AS
BEGIN
	SET NOCOUNT ON;
	--OPEN: 1
	--COMPLETE: 2
	--FILLED: 3
	--CANCELLED: 4
	DECLARE @NumLines INT;
	DECLARE @NumLinesFilled INT;

	--if num line = num filled lines then change order status to filled
	SELECT @NumLines = ISNULL(COUNT(*),0)
	FROM dbo.PurchaseOrderDetail
	WHERE PurchaseOrderID = @PurchaseOrderID;

	--Count the number of filled lines
	SELECT @NumLinesFilled = ISNULL(COUNT(*),0)
	FROM dbo.PurchaseOrderDetail
	WHERE PurchaseOrderID = @PurchaseOrderID AND OrderLineStatusID = 3;

	IF (@NumLines = @NumLinesFilled)
	BEGIN
		--All lines filled change order status to filled
		UPDATE dbo.PurchaseOrderHeader
		SET OrderStatusID = 3 --Filled
		WHERE PurchaseOrderID = @PurchaseOrderID;
	END;
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

--Check if receipt fills the order line
--If yes then change line to filled
CREATE PROCEDURE dbo.usp_UpdatePurchaseOrderDetailToFilled
(
	@PurchaseOrderID BIGINT, 
	@ProductID INT
)AS
BEGIN
	--SET NOCOUNT ON;

	DECLARE @IsFilled BIT = 0;
	--0 if the line is not filled 1 otherwise
	SELECT @IsFilled = IIF(QuantityReceipted < Quantity, 0, 1)
	FROM dbo.PurchaseOrderDetail
	WHERE PurchaseOrderID = @PurchaseOrderID AND ProductID = @ProductID;
	
	IF (@IsFilled = 1)--Line filled
	BEGIN
		UPDATE dbo.PurchaseOrderDetail
		SET OrderLineStatusID = 3
		WHERE PurchaseOrderID = @PurchaseOrderID AND ProductID = @ProductID; 
		
		--A line has been CREATEed to filled
		--Check if all lines are filled 
		--if yes then change the order header status to filled
		EXECUTE dbo.usp_UpdatePurchaseOrderHeaderToFilled @PurchaseOrderID;
	END;
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

--**********Sales Order Header**********

---Insert new sales order
--Returns the ID of the new sales order
--or an error message on failure
CREATE PROCEDURE dbo.usp_InsertSalesOrderHeader
(
	@CustomerID INT,
	@CustomerPurchaseOrder NVARCHAR(100),
	@OrderDate DATETIME,
	@OrderAmount MONEY,
	@VATPercentage DECIMAL(5,4),
	@VATAmount MONEY,
	@TotalAmount MONEY,
	@DeliveryDate DATETIME,
	@OrderStatusID INT
)AS
BEGIN
	BEGIN TRY
		BEGIN TRAN
			SET NOCOUNT ON;

			INSERT INTO dbo.SalesOrderHeader
			(
				CustomerID,
				CustomerPurchaseOrder,
				OrderDate,
				OrderAmount,
				VATPercentage,
				VATAmount,
				TotalAmount,
				DeliveryDate,
				OrderStatusID
			)
			VALUES
			(
				@CustomerID,
				@CustomerPurchaseOrder,
				@OrderDate,
				@OrderAmount,
				@VATPercentage,
				@VATAmount,
				@TotalAmount,
				@DeliveryDate,
				@OrderStatusID
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

--Update sales order header
--Returns 'No Error' on success
--or error message on failure
CREATE PROCEDURE dbo.usp_UpdateSalesOrderHeader
(
	@SalesOrderID BIGINT,
	@CustomerID INT,
	@CustomerPurchaseOrder NVARCHAR(100),
	@OrderDate DATETIME,
	@OrderAmount MONEY,
	@VATPercentage DECIMAL(5,4),
	@VATAmount MONEY,
	@TotalAmount MONEY,
	@DeliveryDate DATETIME,
	@OrderStatusID INT
)AS
BEGIN
	BEGIN TRY
		BEGIN TRAN
			SET NOCOUNT ON;

			UPDATE dbo.SalesOrderHeader
			SET CustomerID = @CustomerID,
				CustomerPurchaseOrder = @CustomerPurchaseOrder,
				OrderDate = @OrderDate,
				OrderAmount = @OrderAmount,
				VATPercentage = @VATPercentage,
				VATAmount = @VATAmount,
				TotalAmount = @TotalAmount,
				DeliveryDate = @DeliveryDate,
				OrderStatusID = @OrderStatusID
			WHERE SalesOrderID = @SalesOrderID;

			--Update Sales order header status if needed
			EXECUTE dbo.usp_UpdateSalesOrderHeaderStatus @SalesOrderID;

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

--Get all sales order headers
--Returns a list Sales Order Headers
---or an error message on failure
CREATE PROCEDURE dbo.usp_GetAllSalesOrderHeaders AS
BEGIN
	BEGIN TRY
		SET NOCOUNT ON;

		SELECT SalesOrderID, CustomerID, CustomerPurchaseOrder, OrderDate,
			   OrderAmount, VATPercentage, VATAmount, TotalAmount,
			   DeliveryDate, OrderStatusID
		FROM dbo.SalesOrderHeader;

	END TRY

	BEGIN CATCH
		SELECT ERROR_MESSAGE() AS Message;
	END CATCH;
END;
GO

--Get Sales order header by ID
--Returns a Sales order header on success
--or an error message on failure
CREATE PROCEDURE dbo.usp_GetSalesOrderHeaderByID
(
	@SalesOrderID BIGINT
)AS
BEGIN
	BEGIN TRY
		SET NOCOUNT ON;

		SELECT SalesOrderID, CustomerID, CustomerPurchaseOrder, OrderDate,
			   OrderAmount, VATPercentage, VATAmount, TotalAmount,
			   DeliveryDate, OrderStatusID
		FROM dbo.SalesOrderHeader
		WHERE SalesOrderID = @SalesOrderID;
	END TRY

	BEGIN CATCH
		SELECT ERROR_MESSAGE() AS Message;
	END CATCH;
END;
GO

--Update sales order header to filled if all lines filled
CREATE PROCEDURE dbo.UpdateSalesOrderHeaderToFilled
(
	@SalesOrderID BIGINT
)AS
BEGIN
	SET NOCOUNT ON;
	--OPEN: 1
	--COMPLETE: 2
	--FILLED: 3
	--CANCELLED: 4
	DECLARE @NumLines INT;
	DECLARE @NumLinesFilled INT;

	--if num line = num filled lines then change order status to filled
	SELECT @NumLines = ISNULL(COUNT(*),0)
	FROM dbo.SalesOrderDetail
	WHERE SalesOrderID = @SalesOrderID;

	--Count the number of filled lines
	SELECT @NumLinesFilled = ISNULL(COUNT(*),0)
	FROM dbo.SalesOrderDetail
	WHERE SalesOrderID = @SalesOrderID AND OrderLineStatusID = 3;

	IF (@NumLines = @NumLinesFilled)
	BEGIN
		--All lines filled change order status to filled
		UPDATE dbo.SalesOrderHeader
		SET OrderStatusID = 3 --Filled
		WHERE SalesOrderID = @SalesOrderID;
	END;
END;
GO

--Will update the Sales order status based on the status of order lines
CREATE PROCEDURE dbo.usp_UpdateSalesOrderHeaderStatus
(
	@SalesOrderID BIGINT
)
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @OrderStatus INT;
	DECLARE @NumLines INT = 0;
	DECLARE @NumLinesOpen INT = 0;
	DECLARE @NumLinesCompleted INT = 0;
	DECLARE @NumLinesFilled INT = 0;
	DECLARE @NumLinesCancelled INT = 0;
	
	--OPEN: 1
	--COMPLETE: 2
	--FILLED: 3
	--CANCELLED: 4

	--Get the current status of the sales order header for comparison
	SELECT @OrderStatus = OrderStatusID
	FROM dbo.SalesOrderHeader
	WHERE SalesOrderID = @SalesOrderID;

	--Count total number of lines for each status in the sales order and add to variables
	;WITH OrderLines AS
	(
		SELECT * 
		FROM dbo.SalesOrderDetail
		WHERE SalesOrderID = @SalesOrderID
	),
	LineCounts AS
	(
		SELECT SUM(1) AS NumLines,
			   SUM(CASE WHEN OrderLineStatusID = 1 THEN 1 ELSE 0 END) AS OpenLines,
			   SUM(CASE WHEN OrderLineStatusID = 2 THEN 1 ELSE 0 END) AS CompletedLines,
			   SUM(CASE WHEN OrderLineStatusID = 3 THEN 1 ELSE 0 END) AS FilledLines,
			   SUM(CASE WHEN OrderLineStatusID = 4 THEN 1 ELSE 0 END) AS CancelledLines
		FROM OrderLines
	)
	SELECT @NumLines = NumLines, 
		   @NumLinesOpen = OpenLines, 
		   @NumLinesCompleted = CompletedLines,
		   @NumLinesFilled = FilledLines,
		   @NumLinesCancelled = CancelledLines
	FROM LineCounts;

	--OPEN: 1
	--COMPLETE: 2
	--FILLED: 3
	--CANCELLED: 4

	--open lines - If any lines are open then the status must be open
	IF (@NumLinesOpen > 0 AND @OrderStatus <> 1)
	BEGIN
		--Update order status to open
		UPDATE dbo.SalesOrderHeader
		SET OrderStatusID = 1
		WHERE SalesOrderID = @SalesOrderID;
	END;

	--cancelled lines - if all lines cancelled then the status must be cancelled
	IF (@NumLinesOpen = 0 AND @NumLinesCancelled = @NumLines AND @OrderStatus <> 4)
	BEGIN
		--Update order status to cancelled
		UPDATE dbo.SalesOrderHeader
		SET OrderStatusID = 4
		WHERE SalesOrderID = @SalesOrderID;
	END;

	--completed lines - if no lines open and one line is completed status must be completed
	IF (@NumLinesOpen = 0 AND @NumLinesCompleted > 0 AND @OrderStatus <> 2)
	BEGIN
		--Update order status to completed
		UPDATE dbo.SalesOrderHeader
		SET OrderStatusID = 2
		WHERE SalesOrderID = @SalesOrderID;
	END;

	--filled lines - if all lines filled then status must be filled. 
	IF (@NumLinesOpen = 0 AND @NumLinesFilled = @NumLines AND @OrderStatus <> 3)
	BEGIN
		--Update order status to filled
		UPDATE dbo.SalesOrderHeader
		SET OrderStatusID = 3
		WHERE SalesOrderID = @SalesOrderID;
	END;

	--filled lines - if no open lines and no completed lines and at least one line is filled the status must be filled
	IF (@NumLinesOpen = 0 AND @NumLinesCompleted = 0 AND @NumLinesFilled > 0 AND @OrderStatus <> 3)
	BEGIN
		--Update order status to filled
		UPDATE dbo.SalesOrderHeader
		SET OrderStatusID = 3
		WHERE SalesOrderID = @SalesOrderID;
	END;
END;
GO

--**********Sales Order Detail**********

--Insert a new sales order detail
--Return 'No Error' on success
--or an error message on failure
CREATE PROCEDURE dbo.usp_InsertSalesOrderDetail
(
	@SalesOrderID BIGINT,
	@ProductID INT,
	@Quantity INT,
	@UnitPrice MONEY,
	--@UnitCost MONEY, --Unit Cost to be determined on invoice? From weighted product unit cost FIFO
	@Discount DECIMAL(3,2),
	--@QuantityInvoiced INT, --To be added when invoiced?
	@OrderLineStatusID INT
)AS
BEGIN
	BEGIN TRY
		BEGIN TRAN
			SET NOCOUNT ON;

			INSERT INTO dbo.SalesOrderDetail
			(
				SalesOrderID,
				ProductID,
				Quantity,
				UnitPrice,
				Discount,
				OrderLineStatusID
			)
			VALUES
			(
				@SalesOrderID,
				@ProductID,
				@Quantity,
				@UnitPrice,
				@Discount,
				@OrderLineStatusID
			);

			--Update sales demand for product
			EXECUTE dbo.usp_UpdateProductSalesDemand @ProductID;

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

--Update sales order detail
--return 'No Error' on success
--or error message on failure
CREATE PROCEDURE dbo.usp_UpdateSalesOrderDetail
(
	@SalesOrderID BIGINT,
	@ProductID INT,
	@Quantity INT,
	@UnitPrice MONEY,
	--@UnitCost MONEY, --Unit Cost to be determined on invoice / reverse? From weighted product unit cost FIFO
	@Discount DECIMAL(3,2),
	--@QuantityInvoiced INT, --To be added when invoiced?
	@OrderLineStatusID INT
)AS
BEGIN
	BEGIN TRY
		BEGIN TRAN
			SET NOCOUNT ON;

			UPDATE dbo.SalesOrderDetail
			SET Quantity = @Quantity,
				UnitPrice = @UnitPrice,
				--UnitCost = @UnitCost,
				Discount = @Discount,
				--QuantityInvoiced = @QuantityInvoiced,
				OrderLineStatusID = @OrderLineStatusID
			WHERE SalesOrderID = @SalesOrderID AND ProductID = @ProductID;

			--Update sales demand for product
			EXECUTE dbo.usp_UpdateProductSalesDemand @ProductID;

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

--Get all sales order detail
--Returns a list of sales order detail
--or an error message on failure
CREATE PROCEDURE dbo.usp_GetAllSalesOrderDetails AS
BEGIN
	BEGIN TRY
		SET NOCOUNT ON;

		SELECT SalesOrderID, ProductID, Quantity, UnitPrice,
			   UnitCost, Discount, QuantityInvoiced, OrderLineStatusID
		FROM dbo.SalesOrderDetail;
	END TRY

	BEGIN CATCH
		SELECT ERROR_MESSAGE() AS Message;
	END CATCH;
END;
GO

--Get all Sales order detail by SalesOrderID
--Returns a list of sales order detail by SalesOrderID
--or an error message on failure
CREATE PROCEDURE dbo.usp_GetSalesOrderDetailBySalesOrderID 
(
	@SalesOrderID BIGINT
)AS
BEGIN
	BEGIN TRY
		SET NOCOUNT ON;

		SELECT SalesOrderID, ProductID, Quantity, UnitPrice,
			   UnitCost, Discount, QuantityInvoiced, OrderLineStatusID
		FROM dbo.SalesOrderDetail
		WHERE SalesOrderID = @SalesOrderID;
	END TRY

	BEGIN CATCH
		SELECT ERROR_MESSAGE() AS Message;
	END CATCH;
END;
GO

--Update the specified sales order with quantity invoiced/goods issued
CREATE PROCEDURE dbo.usp_UpdateSalesOrderDetailQuantityInvoiced
(
	@SalesOrderID BIGINT,
	@ProductID INT
)AS
BEGIN
	DECLARE @QtyInvoiced INT;
	SELECT @QtyInvoiced = ISNULL(SUM(QuantityIssued),0)
	FROM dbo.Issues
	WHERE SalesOrderID = @SalesOrderID AND ProductID = @ProductID;

	--Update the associated sales order detail
	UPDATE dbo.SalesOrderDetail
	SET QuantityInvoiced = (@QtyInvoiced * -1)
	WHERE SalesOrderID = @SalesOrderID AND ProductID = @ProductID;
END;
GO

--Update Sales order detail to filled if qty invoiced = qty required by customer
CREATE PROCEDURE dbo.usp_UpdateSalesOrderDetailToFilled
(
	@SalesOrderID BIGINT,
	@ProductID INT
)AS
BEGIN
	--SET NOCOUNT ON;

	DECLARE @IsFilled BIT = 0;
	--0 if the line is not filled 1 otherwise
	SELECT @IsFilled = IIF(QuantityInvoiced < Quantity, 0, 1)
	FROM dbo.SalesOrderDetail
	WHERE SalesOrderID = @SalesOrderID AND ProductID = @ProductID;
	
	IF (@IsFilled = 1)--Line filled
	BEGIN
		UPDATE dbo.SalesOrderDetail
		SET OrderLineStatusID = 3
		WHERE SalesOrderID = @SalesOrderID AND ProductID = @ProductID; 
		
		--A line has been CREATEed to filled
		--Check if all lines are filled 
		--if yes then change the order header status to filled
		EXECUTE dbo.UpdateSalesOrderHeaderToFilled @SalesOrderID;
	END;
END;
GO



--Updates the specified sales order detail with the weighted cost of sales
CREATE PROCEDURE dbo.usp_UpdateSalesOrderDetailWithWeightedUnitCost
(
	@SalesOrderID BIGINT,
	@ProductID INT
)AS
BEGIN
	--Determine the weighted cost of sales for the sales order detail
	DECLARE @UnitCost MONEY;
	SELECT @UnitCost = ISNULL(SUM(QuantityIssued * UnitCost) / SUM(QuantityIssued),0)
	FROM dbo.Issues
	WHERE SalesOrderID = @SalesOrderID AND ProductID = @ProductID AND ReverseReferenceID IS NULL;

	--Update the sales order detail UnitCost
	UPDATE dbo.SalesOrderDetail
	SET UnitCost = @UnitCost
	WHERE SalesOrderID = @SalesOrderID AND ProductID = @ProductID;
END;
GO

--**********Issues**********
--Goods issued / invoiced

--Insert new goods issued transaction
--Returns a goods issue on success
--or an error message on failure
CREATE PROCEDURE dbo.usp_InsertGoodsIssued
(
	@SalesOrderID BIGINT,
	@ProductID INT,
	@QuantityIssued INT --Must be a positive value greater than zero
)AS
BEGIN
	BEGIN TRY
		BEGIN TRAN
			SET NOCOUNT ON;

			DECLARE @IssueID INT;
			DECLARE @UnitCost MONEY;	
			
			--Determine the weighted unit cost of goods beign issued and add to issue CREATEd
			EXECUTE dbo.usp_ReturnGoodsIssuedWeightedUnitCostFIFO @ProductID, @QuantityIssued, @IssueUnitCost = @UnitCost OUTPUT;

			--Check if any errors
			IF (@UnitCost = -1)--Not enough stock to issued
			BEGIN
				;THROW 999999, 'Not enough stock to issue the amount passed in', 1;
			END;

			--Change quantity issued to negative
			SET @QuantityIssued = @QuantityIssued * -1;

			INSERT INTO dbo.Issues
			(
				SalesOrderID,
				ProductID,
				QuantityIssued,
				UnitCost
			)
			VALUES
			(
				@SalesOrderID,
				@ProductID,
				@QuantityIssued,
				@UnitCost
			);

			--Get id for issue
			SELECT @IssueID = SCOPE_IDENTITY();

			--Get issue date
			DECLARE @IssueDate DATETIME;
			SELECT @IssueDate = IssueDate
			FROM Issues
			WHERE IssueID = @IssueID;

			--Add to Inventory transactions
			EXECUTE dbo.usp_InsertInventoryTransactions 'I', @IssueDate, @ProductID, @IssueID, @QuantityIssued;

			--update quantity of product on hand
			EXECUTE dbo.usp_UpdateProductOnHand @ProductID;			

			--Update the weighted unit cost of product still in stock
			EXECUTE dbo.usp_UpdateProductWeightedUnitCostFIFO @ProductID;

			--Update the associated sales order detail with the unit cost of goods issued
			EXECUTE dbo.usp_UpdateSalesOrderDetailWithWeightedUnitCost @SalesOrderID, @ProductID;

			--Update the associated invoiced quantity for this sales order detail
			EXECUTE dbo.usp_UpdateSalesOrderDetailQuantityInvoiced @SalesOrderID, @ProductID;

			--Update quantity of sales demand for product
			EXECUTE dbo.usp_UpdateProductSalesDemand @ProductID;

			--Change to filled status if the goods issued / invoice completes the line
			EXECUTE dbo.usp_UpdateSalesOrderDetailToFilled @SalesOrderID, @ProductID;

			--Return the issue CREATE
			SELECT IssueID, SalesOrderID, ProductID,
				   IssueDate, QuantityIssued, UnitCost,
				   ReverseReferenceID
			FROM dbo.Issues
			WHERE IssueID = @IssueID;		

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

--Get by sales order id
--Returns a list of goods issued for the specified sales order
--or an error message on failure
CREATE PROCEDURE dbo.usp_GetIssuesBySalesOrderID
(
	@SalesOrderID BIGINT
)AS
BEGIN
	BEGIN TRY
		SET NOCOUNT ON;

		SELECT IssueID, SalesOrderID, ProductID,
			   IssueDate, QuantityIssued, UnitCost,
			   ReverseReferenceID
		FROM dbo.Issues
		WHERE SalesOrderID = @SalesOrderID;
	END TRY

	BEGIN CATCH
		SELECT ERROR_MESSAGE() AS Message;
	END CATCH;
END;
GO

--Reverse a goods issued / invoice
CREATE PROCEDURE dbo.usp_ReverseIssueByID
(
	@IssueID INT --goods issue to reverse
)AS
BEGIN
	BEGIN TRY
		BEGIN TRAN
			SET NOCOUNT ON;
			--First ensure it has not been reversed before
			DECLARE @SalesOrderID BIGINT;
			DECLARE @ProductID INT;
			DECLARE @QuantityIssued INT;
			DECLARE @UnitCost MONEY;
			DECLARE @ReversedIssueID INT;

			SELECT @SalesOrderID = SalesOrderID,
				   @ProductID = ProductID,
				   @QuantityIssued = QuantityIssued,
				   @UnitCost = UnitCost,
				   @ReversedIssueID = ReverseReferenceID
			FROM dbo.Issues
			WHERE IssueID = @IssueID;

			
			IF (@ReversedIssueID IS NULL)--Not reversed previously
			BEGIN				
				--Generate the new reversed issue
				INSERT INTO dbo.Issues
				(
					SalesOrderID,
					ProductID,
					QuantityIssued,
					UnitCost,
					ReverseReferenceID
				)
				VALUES
				(
					@SalesOrderID,
					@ProductID,
					@QuantityIssued * -1,
					@UnitCost,
					@IssueID
				);

				SET @ReversedIssueID = SCOPE_IDENTITY();
				
				--Add the issue id of the reversal to the original issue
				UPDATE dbo.Issues
				SET ReverseReferenceID = @ReversedIssueID
				WHERE IssueID = @IssueID;

				--Check if the Sales order line was filled or completed
			--if yes then change to open
			--OPEN: 1
			--COMPLETE: 2
			--FILLED: 3
			--CANCELLED: 4
			DECLARE @OrderLineStatusID INT;

			SELECT @OrderLineStatusID = OrderLineStatusID
			FROM dbo.SalesOrderDetail
			WHERE SalesOrderID = @SalesOrderID AND ProductID = @ProductID;

			IF (@OrderLineStatusID = 2 OR @OrderLineStatusID = 3)
			BEGIN
				UPDATE dbo.SalesOrderDetail
				SET OrderLineStatusID = 1 --Open
				WHERE SalesOrderID = @SalesOrderID AND ProductID = @ProductID;
			END;

			--Check if the Sales order was filled or completed
			--if yes then change to open
			--OPEN: 1
			--COMPLETE: 2
			--FILLED: 3
			--CANCELLED: 4
			DECLARE @OrderStatusID INT;

			SELECT @OrderStatusID = OrderStatusID
			FROM dbo.SalesOrderHeader
			WHERE SalesOrderID = @SalesOrderID;
			
			IF (@OrderStatusID = 2 OR @OrderStatusID = 3)
			BEGIN
				UPDATE dbo.SalesOrderHeader
				SET OrderStatusID = 1 --Open
				WHERE SalesOrderID = @SalesOrderID;
			END;

			--Inventory transaction
			--Get the issue date
			DECLARE @IssueDate DATETIME;
			SELECT @IssueDate = IssueDate
			FROM dbo.Issues
			WHERE IssueID = @ReversedIssueID;

			--change to negative value
			SET @QuantityIssued = @QuantityIssued * -1;
			
			--Add to Inventory transactions
			EXECUTE dbo.usp_InsertInventoryTransactions 'I', @IssueDate, @ProductID, @ReversedIssueID, @QuantityIssued;			

			--Update Product On Hand
			EXECUTE dbo.usp_UpdateProductOnHand @ProductID; --increase with a reversal

			--Update the product weighted unit cost as there is more on hand
			EXECUTE dbo.usp_UpdateProductWeightedUnitCostFIFO @ProductID;

			--Update the associated sales order detail with the weighted unit cost of sales of goods issued / invoiced
			EXECUTE dbo.usp_UpdateSalesOrderDetailWithWeightedUnitCost @SalesOrderID, @ProductID;

			--Update Sales order detail with quantity invoiced
			EXECUTE dbo.usp_UpdateSalesOrderDetailQuantityInvoiced @SalesOrderID, @ProductID;

			--Update the product sales demand
			EXECUTE dbo.usp_UpdateProductSalesDemand @ProductID;

			END;

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
--Determines the unit cost of the product to be issued/invoiced 
--Based on a First In First Out principle
--Cannot be used to check an existing goods issue/ invoice
--as it takes all previous goods issued into account
CREATE PROCEDURE dbo.usp_ReturnGoodsIssuedWeightedUnitCostFIFO
(
	@ProductID INT,
	@QtyToIssued INT,
	@IssueUnitCost MONEY OUTPUT
)AS
BEGIN
	--First check that their is enough stock on hand to issue
	DECLARE @QtyOnHand INT;
	SELECT @QtyOnHand = ISNULL(SUM(Quantity),0)
	FROM dbo.InventoryTransactions
	WHERE ProductID = @ProductID;

	IF (@QtyOnHand < @QtyToIssued)
	BEGIN
		--Return -1 if not enough stock to issue
		SET @IssueUnitCost = -1;
	END;

	ELSE
	BEGIN
		--Sum up all goods issued
		;WITH SumIssued AS
		(
			--sum up all previous goods issued for this product
			SELECT ISNULL(SUM(QuantityIssued),0) AS TotalIssued
			FROM dbo.Issues
			WHERE ProductID = @ProductID AND ReverseReferenceID IS NULL --Only non revered goods issues
		),
		--Retrieve the remaining receipts with stock left on them base on FIFO issued
		ReceiptsWithRunningSum AS
		(
			SELECT ReceiptID, QuantityReceipted, UnitCost,
				   SUM(QuantityReceipted) OVER(ORDER BY ReceiptID) AS RunningSumReceipted,
				   TotalIssued,
				   SUM(QuantityReceipted) OVER(ORDER BY ReceiptID) + TotalIssued AS QtyLeft
			FROM dbo.Receipts
			CROSS JOIN SumIssued
			WHERE ProductID = @ProductID AND ReverseReferenceID IS NULL --Only non reversed receipts
		),
		--retrieve only the rows where QtyLeft is > 0 from ReceiptsWithRunningSum CTE and add row numbers
		ReceiptsWithStock AS
		(
			SELECT *,
				   ROW_NUMBER() OVER(ORDER BY ReceiptID) AS RowNum,
				   QtyLeft - @QtyToIssued AS Balance
			FROM ReceiptsWithRunningSum
			WHERE QtyLeft > 0
		),
		--Find the first row that is greater than or equal to zero from ReceiptsWithStock
		FirstPositiveRow AS
		(
			SELECT MIN(RowNum) AS MinRowNum
			FROM ReceiptsWithStock
			WHERE Balance >= 0
		),
		RowCosts AS
		(
			SELECT CASE WHEN RowNum = 1  AND Balance < 0 THEN QtyLeft * UnitCost 
						WHEN RowNum = 1 AND Balance >= 0 THEN (QtyLeft - Balance) * UnitCost
						WHEN RowNum <> 1 AND Balance < 0 THEN QuantityReceipted * UnitCost
						WHEN RowNum <> 1 AND Balance >= 0 THEN (QuantityReceipted - Balance) * UnitCost
						ELSE 0 END AS RowCost
			FROM ReceiptsWithStock
			WHERE RowNum <= (SELECT MinRowNum FROM FirstPositiveRow)
		)
		--Get weighted unit cost for quantity issued
		SELECT @IssueUnitCost = ISNULL(SUM(RowCost),0) / @QtyToIssued
		FROM RowCosts;		
	END;
END;
GO

--***************Sales Metrics***************--

--Year to date
--Total revenue / sales volume
--Returns the total revenue for the current year
--or an errormessage if not successfull
CREATE PROCEDURE dbo.usp_SalesRevenueYearToDate AS
BEGIN
	BEGIN TRY
		SELECT SUM(SOD.QuantityInvoiced * (SOD.UnitPrice - (SOD.UnitPrice * SOD.Discount))) AS YearToDateRevenue
		FROM SalesOrderDetail AS SOD
		LEFT OUTER JOIN Issues AS I ON SOD.SalesOrderID = I.SalesOrderID AND SOD.ProductID = I.ProductID
		WHERE YEAR(I.IssueDate) = YEAR(GETDATE()) AND I.ReverseReferenceID IS NULL;
	END TRY

	BEGIN CATCH
		SELECT ERROR_MESSAGE() AS Message;
	END CATCH;
END;
GO

--Total revenue by month
--Return the Year to Date Sales Revenue by Month
--or an Error message on failure
CREATE PROCEDURE dbo.usp_SalesRevenueYearToDatePerMonth AS
BEGIN
	BEGIN TRY
		;WITH SalesByMonthYearToDate AS
		(
			SELECT  FORMAT(I.IssueDate, 'MMMM') AS [MonthName], MONTH(I.IssueDate) AS [MonthNumber], SUM(SOD.QuantityInvoiced * (SOD.UnitPrice - (SOD.UnitPrice * SOD.Discount))) AS YearToDateRevenue
					FROM SalesOrderDetail AS SOD
					LEFT OUTER JOIN Issues AS I ON SOD.SalesOrderID = I.SalesOrderID AND SOD.ProductID = I.ProductID
					WHERE YEAR(I.IssueDate) = YEAR(GETDATE())
					GROUP BY MONTH(I.IssueDate), FORMAT(I.IssueDate, 'MMMM')
		)
		SELECT MonthName, YearToDateRevenue
		FROM SalesByMonthYearToDate
		ORDER BY MonthNumber;
	END TRY

	BEGIN CATCH
		SELECT ERROR_MESSAGE() AS Message;
	END CATCH;
END;
GO

--Top 10 products and revenue generated by each for the current year
--returns the top 10 products by revenue YTD
--or an error message on failure
CREATE PROCEDURE dbo.usp_SalesRevenueTop10ProductsYearToDate AS
BEGIN
	BEGIN TRY
		SELECT  TOP(10) PR.ProductName, SUM(SOD.QuantityInvoiced * (SOD.UnitPrice - (SOD.UnitPrice * SOD.Discount))) AS YearToDateRevenue
		FROM SalesOrderDetail AS SOD
		LEFT OUTER JOIN Issues AS I ON SOD.SalesOrderID = I.SalesOrderID AND I.ProductID = SOD.ProductID
		LEFT OUTER JOIN Products AS PR ON I.ProductID = PR.ProductID
		WHERE I.ReverseReferenceID IS NULL AND YEAR(GETDATE()) = YEAR(I.IssueDate)
		GROUP BY PR.ProductName
		ORDER BY YearToDateRevenue DESC;
	END TRY

	BEGIN CATCH
		SELECT ERROR_MESSAGE() AS Message;
	END CATCH;
END;
GO
--Top 10 products and revenue generated as a percentage of revenue
--Returns the percentage of the top 10 product revenues to total sales revenue YTD
--or an error message on failure
CREATE PROCEDURE dbo.usp_PercentageOfTop10ProductsRevenueToSalesRevenueYTD AS
BEGIN
	BEGIN TRY
		;WITH YearToDateSalesRevenue AS
		(
			SELECT SUM(SOD.QuantityInvoiced * (SOD.UnitPrice - (SOD.UnitPrice * SOD.Discount))) AS YearToDateRevenue
			FROM SalesOrderDetail AS SOD
			LEFT OUTER JOIN Issues AS I ON SOD.SalesOrderID = I.SalesOrderID AND SOD.ProductID = I.ProductID
			WHERE YEAR(I.IssueDate) = YEAR(GETDATE()) AND I.ReverseReferenceID IS NULL
		),
		Top10Products AS
		(
			SELECT  TOP(10) PR.ProductName, SUM(SOD.QuantityInvoiced * (SOD.UnitPrice - (SOD.UnitPrice * SOD.Discount))) AS YearToDateRevenue
			FROM SalesOrderDetail AS SOD
			LEFT OUTER JOIN Issues AS I ON SOD.SalesOrderID = I.SalesOrderID AND I.ProductID = SOD.ProductID
			LEFT OUTER JOIN Products AS PR ON I.ProductID = PR.ProductID
			WHERE I.ReverseReferenceID IS NULL AND YEAR(GETDATE()) = YEAR(I.IssueDate)
			GROUP BY PR.ProductName
			ORDER BY YearToDateRevenue DESC
		),
		Top10ProductRevenue AS
		(
			SELECT SUM(TOP10.YearToDateRevenue) AS YearToDateRevenue
			FROM Top10Products AS TOP10
		)
		SELECT CASE WHEN YTDR.YearToDateRevenue > 0 THEN (YTDP.YearToDateRevenue / YTDR.YearToDateRevenue) * 100 ELSE 0 END AS PercentageOfYTDRevenue
		FROM YearToDateSalesRevenue AS YTDR
		CROSS JOIN Top10ProductRevenue AS YTDP
	END TRY

	BEGIN CATCH
		SELECT ERROR_MESSAGE() AS Message;
	END CATCH;
END;
GO

--Returns the top 10 products by revenue YTD
--or an error message on failure
CREATE PROCEDURE dbo.usp_Top10ProductsByRevenueYTD AS
BEGIN
	BEGIN TRY
		WITH Top10 AS 
		(
			SELECT  TOP(10) PR.ProductName, ISNULL(SUM(SOD.QuantityInvoiced * (SOD.UnitPrice - (SOD.UnitPrice * SOD.Discount))),0) AS YearToDateRevenue
			FROM SalesOrderDetail AS SOD
			LEFT OUTER JOIN Issues AS I ON SOD.SalesOrderID = I.SalesOrderID AND I.ProductID = SOD.ProductID
			LEFT OUTER JOIN Products AS PR ON I.ProductID = PR.ProductID
			WHERE I.ReverseReferenceID IS NULL AND YEAR(GETDATE()) = YEAR(I.IssueDate)
			GROUP BY PR.ProductName
		)
		SELECT ISNULL(SUM(YearToDateRevenue),0) AS YearToDateRevenue
		FROM Top10;
	END TRY

	BEGIN CATCH
		SELECT ERROR_MESSAGE() AS Message;
	END CATCH;
END;
GO

--Count of days to close sales order
--Only orders that have been filled or completed
--CREATE PROCEDURE dbo.usp_DaysCountToCloseSalesOrder
--number of open sales orders
--number of orders

--Month to date