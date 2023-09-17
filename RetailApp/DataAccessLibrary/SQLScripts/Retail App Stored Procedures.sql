USE RetailAppDB;
GO

--**********Vendor--**********
ALTER PROCEDURE dbo.usp_InsertVendor
(
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

ALTER PROCEDURE dbo.usp_UpdateVendor
(
	@VendorID INT,
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
			SET	FirstName = @FirstName,
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

ALTER PROCEDURE dbo.usp_DeleteVendor
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

ALTER PROCEDURE dbo.usp_GetAllVendors AS
BEGIN
	BEGIN TRY
		SET NOCOUNT ON;

		SELECT VendorID, FirstName, LastName, CompanyName, AddressLine1, AddressLine2,
			   City, Province, PostalCode, EMailAddress, PhoneNumber
		FROM dbo.Vendors;
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

		SELECT VendorID, FirstName, LastName, CompanyName, AddressLine1, AddressLine2,
			   City, Province, PostalCode, EMailAddress, PhoneNumber
		FROM dbo.Vendors
		WHERE VendorID = @VendorID;
	END TRY

	BEGIN CATCH
		SELECT ERROR_MESSAGE() AS Message;
	END CATCH;
END;
GO