CREATE SCHEMA Core;
GO

CREATE SCHEMA Hr;
GO

CREATE SCHEMA Krn;
GO

CREATE SCHEMA Lnm;
GO

CREATE TABLE Core.Positions (
	[Key] BIGINT NOT NULL
	,PositionCode NVARCHAR(20) NOT NULL
	,PositionName NVARCHAR(100) NOT NULL
	,PositionDescription NVARCHAR(255)
	,PositionLevel INT NOT NULL
	,CONSTRAINT pk_core_positions PRIMARY KEY ([Key])
	);

CREATE TABLE Core.Branches (
	[Key] BIGINT NOT NULL
	,BranchCode NVARCHAR(10) NOT NULL
	,BranchName NVARCHAR(100) NOT NULL
	,BranchAdress NVARCHAR(255) NOT NULL
	,CONSTRAINT pk_core_branches PRIMARY KEY ([Key])
	);

CREATE TABLE Core.Categories (
	[Key] BIGINT NOT NULL PRIMARY KEY
	,CategoryCode NVARCHAR(10) NOT NULL
	,CategoryName NVARCHAR(100) NOT NULL
	);

CREATE TABLE Core.Roles (
	[Key] BIGINT NOT NULL PRIMARY KEY
	,RoleCode NVARCHAR(100) NOT NULL
	,RoleSecurityLevel INT NOT NULL
	);

CREATE TABLE Hr.Employees (
	[Key] BIGINT NOT NULL PRIMARY KEY
	,Name NVARCHAR(50) NOT NULL
	,Surname NVARCHAR(50) NOT NULL
	,Position BIGINT NOT NULL
	,Branch BIGINT NOT NULL
	,CONSTRAINT fk_hr_employees_core_positions FOREIGN KEY (Position) REFERENCES Core.Positions([Key])
	,CONSTRAINT fk_hr_employees_core_branches FOREIGN KEY (Branch) REFERENCES Core.Branches([Key])
	);

CREATE TABLE Krn.Customers (
	[Key] BIGINT NOT NULL PRIMARY KEY
	,Name NVARCHAR(50) NOT NULL
	,Surname NVARCHAR(50) NOT NULL
	,Category BIGINT NOT NULL
	,Branch BIGINT NOT NULL
	,CONSTRAINT fk_krn_customers_core_categories FOREIGN KEY (Category) REFERENCES Core.Categories([Key])
	,CONSTRAINT fk_krn_customers_core_branches FOREIGN KEY (Branch) REFERENCES Core.Branches([Key])
	);

CREATE TABLE Krn.Accounts (
	[Key] BIGINT NOT NULL PRIMARY KEY
	,Number NVARCHAR(30) NOT NULL
	,PlanCode NVARCHAR(20) NOT NULL
	,Balance DECIMAL(18, 2) NOT NULL
	,CustomerKey BIGINT NOT NULL
	,CONSTRAINT fk_krn_accounts_krn_customers FOREIGN KEY (CustomerKey) REFERENCES Krn.Customers([Key])
	);

CREATE TABLE Lnm.Loans (
	[Key] BIGINT NOT NULL PRIMARY KEY
	,Amount DECIMAL(18, 2) NOT NULL
	,InitialAmount DECIMAL(18, 2) NOT NULL
	,StartDate DATE NOT NULL
	,EndDate DATE NOT NULL
	,Rate INT NOT NULL
	,Type NVARCHAR(30) NOT NULL
	,BankerKey BIGINT NOT NULL
	,AccountKey BIGINT NOT NULL
	,CONSTRAINT fk_loans_hr_employees FOREIGN KEY (BankerKey) REFERENCES Hr.Employees([Key])
	,CONSTRAINT fk_loans_krn_accounts FOREIGN KEY (AccountKey) REFERENCES Krn.Accounts([Key])
	);

CREATE INDEX idx_loans_start_date ON Lnm.Loans (StartDate);

CREATE INDEX idx_loans_end_date ON Lnm.Loans (EndDate);

CREATE TABLE Krn.Transactions (
	[Key] BIGINT NOT NULL PRIMARY KEY
	,AccountKey BIGINT NOT NULL
	,IsDebit BIT NOT NULL
	,Category NVARCHAR(30) NOT NULL
	,Amount DECIMAL(18, 2) NOT NULL
	,DATE DATETIME NOT NULL
	,Comment NVARCHAR(255)
	,CONSTRAINT fk_krn_transactions_krn_accounts FOREIGN KEY (AccountKey) REFERENCES Krn.Accounts([Key])
	);

CREATE INDEX idx_transactions_date ON Krn.Transactions (DATE);

CREATE TABLE Krn.Users (
	[Key] BIGINT NOT NULL PRIMARY KEY
	,EmployeeKey BIGINT NOT NULL
	,RoleKey BIGINT NOT NULL
	,CONSTRAINT fk_krn_users_hr_employees FOREIGN KEY (EmployeeKey) REFERENCES Hr.Employees([Key])
	,CONSTRAINT fk_krn_users_core_roles FOREIGN KEY (RoleKey) REFERENCES Core.Roles([Key])
	);

