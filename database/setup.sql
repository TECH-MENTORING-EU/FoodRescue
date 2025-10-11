-- FoodRescue Database Setup Script
-- Run this script to create the database and tables

-- Create the database if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'FoodRescue')
BEGIN
    CREATE DATABASE FoodRescue;
END
GO

USE FoodRescue;
GO

-- Create FoodDonations table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FoodDonations]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[FoodDonations] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [DonorName] NVARCHAR(200) NOT NULL,
        [FoodType] NVARCHAR(100) NOT NULL,
        [Quantity] INT NOT NULL,
        [Unit] NVARCHAR(50) NOT NULL,
        [DonationDate] DATETIME2 NOT NULL,
        [PickupLocation] NVARCHAR(500) NOT NULL,
        [IsPickedUp] BIT NOT NULL DEFAULT 0,
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [UpdatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE()
    );

    PRINT 'FoodDonations table created successfully.';
END
ELSE
BEGIN
    PRINT 'FoodDonations table already exists.';
END
GO

-- Create index for better query performance
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_FoodDonations_DonationDate' AND object_id = OBJECT_ID('FoodDonations'))
BEGIN
    CREATE INDEX IX_FoodDonations_DonationDate ON FoodDonations(DonationDate DESC);
    PRINT 'Index IX_FoodDonations_DonationDate created successfully.';
END
GO

-- Create index on IsPickedUp for filtering
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_FoodDonations_IsPickedUp' AND object_id = OBJECT_ID('FoodDonations'))
BEGIN
    CREATE INDEX IX_FoodDonations_IsPickedUp ON FoodDonations(IsPickedUp);
    PRINT 'Index IX_FoodDonations_IsPickedUp created successfully.';
END
GO

-- Insert sample data (optional - you can also use Bogus in the app)
IF NOT EXISTS (SELECT * FROM FoodDonations)
BEGIN
    INSERT INTO FoodDonations (DonorName, FoodType, Quantity, Unit, DonationDate, PickupLocation, IsPickedUp)
    VALUES 
        ('Local Bakery', 'Bread', 50, 'units', GETDATE(), '123 Main St, City', 0),
        ('Grocery Store', 'Vegetables', 25, 'kg', GETDATE(), '456 Oak Ave, City', 0),
        ('Restaurant Chain', 'Prepared Meals', 30, 'units', GETDATE(), '789 Elm St, City', 1);
    
    PRINT 'Sample data inserted successfully.';
END
GO

PRINT 'Database setup completed successfully!';
GO
