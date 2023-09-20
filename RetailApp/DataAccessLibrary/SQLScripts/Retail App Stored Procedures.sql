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
		WHERE CustomerID = @CustomerID;
	END TRY

	BEGIN CATCH
		SELECT ERROR_MESSAGE() AS Message;
	END CATCH;
END;
GO

--**********Company Detail**********
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