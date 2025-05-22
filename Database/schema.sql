-- Создание таблицы Categories
CREATE TABLE Categories (
    CategoryId INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    Description TEXT
);

-- Создание таблицы Suppliers
CREATE TABLE Suppliers (
    SupplierId INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    ContactPerson TEXT,
    Phone TEXT,
    Email TEXT,
    Address TEXT
);

-- Создание таблицы Products
CREATE TABLE Products (
    ProductId INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    Description TEXT,
    Price DECIMAL(10,2) NOT NULL,
    CategoryId INTEGER,
    SupplierId INTEGER,
    GPUModel TEXT NOT NULL,
    MemorySize INTEGER NOT NULL,
    MemoryType TEXT NOT NULL,
    FOREIGN KEY (CategoryId) REFERENCES Categories(CategoryId),
    FOREIGN KEY (SupplierId) REFERENCES Suppliers(SupplierId)
);

-- Создание таблицы Customers
CREATE TABLE Customers (
    CustomerId INTEGER PRIMARY KEY AUTOINCREMENT,
    FirstName TEXT NOT NULL,
    LastName TEXT NOT NULL,
    Email TEXT UNIQUE,
    Phone TEXT,
    Address TEXT
);

-- Создание таблицы Orders
CREATE TABLE Orders (
    OrderId INTEGER PRIMARY KEY AUTOINCREMENT,
    CustomerId INTEGER NOT NULL,
    OrderDate DATETIME NOT NULL,
    TotalAmount DECIMAL(10,2) NOT NULL,
    Status TEXT NOT NULL,
    FOREIGN KEY (CustomerId) REFERENCES Customers(CustomerId)
);

-- Создание таблицы OrderItems
CREATE TABLE OrderItems (
    OrderItemId INTEGER PRIMARY KEY AUTOINCREMENT,
    OrderId INTEGER NOT NULL,
    ProductId INTEGER NOT NULL,
    Quantity INTEGER NOT NULL,
    UnitPrice DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (OrderId) REFERENCES Orders(OrderId),
    FOREIGN KEY (ProductId) REFERENCES Products(ProductId)
);

-- Создание таблицы Inventory
CREATE TABLE Inventory (
    InventoryId INTEGER PRIMARY KEY AUTOINCREMENT,
    ProductId INTEGER NOT NULL UNIQUE,
    Quantity INTEGER NOT NULL,
    LastUpdated DATETIME NOT NULL,
    FOREIGN KEY (ProductId) REFERENCES Products(ProductId)
);

-- Создание таблицы Discounts
CREATE TABLE Discounts (
    DiscountId INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    Description TEXT,
    DiscountPercent DECIMAL(5,2) NOT NULL,
    StartDate DATETIME NOT NULL,
    EndDate DATETIME NOT NULL
);

-- Создание таблицы ProductDiscounts (связь многие-ко-многим)
CREATE TABLE ProductDiscounts (
    ProductId INTEGER NOT NULL,
    DiscountId INTEGER NOT NULL,
    PRIMARY KEY (ProductId, DiscountId),
    FOREIGN KEY (ProductId) REFERENCES Products(ProductId),
    FOREIGN KEY (DiscountId) REFERENCES Discounts(DiscountId)
); 