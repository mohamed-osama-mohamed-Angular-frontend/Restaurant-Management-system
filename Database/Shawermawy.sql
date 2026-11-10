CREATE DATABASE Shawermawy;
GO

USE Shawermawy;
GO

-- Users Table
CREATE TABLE Users (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) UNIQUE NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    PhoneNumber NVARCHAR(20) NOT NULL,
    PasswordHash NVARCHAR(256) NOT NULL,
    IsAdmin BIT DEFAULT 0,
    CreatedAt DATETIME DEFAULT GETDATE()
);
GO

-- Menu Categories
CREATE TABLE Categories (
    CategoryId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    IconClass NVARCHAR(50)
);
GO

-- Menu Items
CREATE TABLE MenuItems (
    ItemId INT IDENTITY(1,1) PRIMARY KEY,
    CategoryId INT FOREIGN KEY REFERENCES Categories(CategoryId),
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500),
    Price DECIMAL(10,2) NOT NULL,
    ImageUrl NVARCHAR(300),
    IsAvailable BIT DEFAULT 1
);
GO

-- Orders
CREATE TABLE Orders (
    OrderId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT FOREIGN KEY REFERENCES Users(UserId),
    Status NVARCHAR(50) DEFAULT 'OnHold',
    PaymentMethod NVARCHAR(20),
    TotalAmount DECIMAL(10,2),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE()
);
GO

-- Order Items
CREATE TABLE OrderItems (
    OrderItemId INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT FOREIGN KEY REFERENCES Orders(OrderId),
    ItemId INT FOREIGN KEY REFERENCES MenuItems(ItemId),
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(10,2) NOT NULL
);
GO

-- Cart (OrderList before submission)
CREATE TABLE CartItems (
    CartItemId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT FOREIGN KEY REFERENCES Users(UserId),
    ItemId INT FOREIGN KEY REFERENCES MenuItems(ItemId),
    Quantity INT DEFAULT 1
);
GO

-- Reviews
CREATE TABLE Reviews (
    ReviewId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT FOREIGN KEY REFERENCES Users(UserId),
    Title NVARCHAR(200) NOT NULL,
    Message NVARCHAR(2000) NOT NULL,
    Type NVARCHAR(50), -- Review, Report, Suggestion
    CreatedAt DATETIME DEFAULT GETDATE()
);
GO

-- Seed Admin Accounts
INSERT INTO Users (Username, Email, PhoneNumber, PasswordHash, IsAdmin)
VALUES
('MwaelM',    'admin1@shawermawy.com', '0000000001', 'M95123570W', 1),
('HnaziH',    'admin2@shawermawy.com', '0000000002', 'H95123570H', 1),
('RrashdanR', 'admin3@shawermawy.com', '0000000003', 'R95123570R', 1);
GO

-- Seed Categories
INSERT INTO Categories (Name, IconClass) VALUES
('Shawarma',   'fa-bread-slice'),
('Sandwiches', 'fa-hamburger'),
('Sides',      'fa-utensils'),
('Drinks',     'fa-mug-hot');
GO

-- Seed Menu Items
INSERT INTO MenuItems (CategoryId, Name, Description, Price, ImageUrl) VALUES
(1, 'Classic Chicken Shawarma', 'Tender chicken with garlic sauce and pickles', 45.00, '/images/menu/chicken-shawarma.jpg'),
(1, 'Meat Shawarma',            'Juicy beef and lamb mix with tahini',           55.00, '/images/menu/meat-shawarma.jpg'),
(1, 'Mixed Shawarma',           'Chicken and meat combo',                        60.00, '/images/menu/mixed-shawarma.jpg'),
(2, 'Falafel Sandwich',         'Crispy falafel with fresh veggies',             30.00, '/images/menu/falafel.jpg'),
(2, 'Kofta Sandwich',           'Spiced kofta with tomatoes and onions',         40.00, '/images/menu/kofta.jpg'),
(3, 'French Fries',             'Golden crispy fries',                           20.00, '/images/menu/fries.jpg'),
(3, 'Garlic Bread',             'Toasted bread with garlic butter',              15.00, '/images/menu/garlic-bread.jpg'),
(4, 'Soft Drink',               'Pepsi, 7Up, or Mirinda',                        15.00, '/images/menu/drink.jpg'),
(4, 'Fresh Juice',              'Orange or mango fresh juice',                   25.00, '/images/menu/juice.jpg');
GO