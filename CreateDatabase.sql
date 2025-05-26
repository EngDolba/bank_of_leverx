create schema Core;
go
create schema Hr;
go
create schema Krn;
go
create schema Lnm;
go

---

create table Core.Positions (
    [Key] bigint not null,
    PositionCode nvarchar(20) not null,
    PositionName nvarchar(100) not null,
    PositionDescription nvarchar(255),
    PositionLevel int not null,
    constraint PKCorePositions primary key ([Key])
);

create table Core.Branches (
    [Key] bigint not null,
    BranchCode nvarchar(10) not null,
    BranchName nvarchar(100) not null,
    BranchAdress nvarchar(255) not null,
    constraint PKCoreBranches primary key ([Key])
);

create table Core.Categories (
    [Key] bigint not null primary key,
    CategoryCode nvarchar(10) not null,
    CategoryName nvarchar(100) not null
);

create table Core.Roles (
    [Key] bigint not null primary key,
    RoleCode nvarchar(100) not null,
    RoleSecurityLevel int not null
);

create table Hr.Employees (
    [Key] bigint not null primary key,
    Name nvarchar(50) not null,
    Surname nvarchar(50) not null,
    Position bigint not null,
    Branch bigint not null,
    constraint FKHrEmployeesCorePositions foreign key (Position) references Core.Positions ([Key]),
    constraint FKHrEmployeesCoreBranches foreign key (Branch) references Core.Branches ([Key])
);

create table Krn.Customers (
    [Key] bigint not null primary key,
    Name nvarchar(50) not null,
    Surname nvarchar(50) not null,
    Category bigint not null,
    Branch bigint not null,
    constraint FKKrnCustomersCoreCategories foreign key (Category) references Core.Categories ([Key]),
    constraint FKKrnCustomersCoreBranches foreign key (Branch) references Core.Branches ([Key])
);

create table Krn.Accounts (
    [Key] bigint not null primary key,
    Number nvarchar(30) not null,
    PlanCode nvarchar(20) not null,
    Balance decimal(18,2) not null,
    CustomerKey bigint not null,
    constraint FKKrnAccountsKrnCustomers foreign key (CustomerKey) references Krn.Customers ([Key])
);

create table Lnm.Loans (
    [Key] bigint not null primary key,
    Amount decimal(18,2) not null,
    InitialAmount decimal(18,2) not null,
    StartDate date not null,
    EndDate date not null,
    Rate int not null,
    Type nvarchar(30) not null,
    BankerKey bigint not null,
    AccountKey bigint not null,
    constraint FKLoansHrEmployees foreign key (BankerKey) references Hr.Employees ([Key]),
    constraint FKLoansKrnAccounts foreign key (AccountKey) references Krn.Accounts ([Key])
);

create index IdxLoansStartDate on Lnm.Loans (StartDate);
create index IdxLoansEndDate on Lnm.Loans (EndDate);

create table Krn.Transactions (
    [Key] bigint not null primary key,
    AccountKey bigint not null,
    IsDebit bit not null,
    Category nvarchar(30) not null,
    Amount decimal(18,2) not null,
    Date datetime not null,
    Comment nvarchar(255),
    constraint FKKrnTransactionsKrnAccounts foreign key (AccountKey) references Krn.Accounts ([Key])
);

create index IdxTransactionsDate on Krn.Transactions (Date);

create table Krn.Users (
    [Key] bigint not null primary key,
    EmployeeKey bigint not null,
    RoleKey bigint not null,
    constraint FKKrnUsersHrEmployees foreign key (EmployeeKey) references Hr.Employees ([Key]),
    constraint FKKrnUsersCoreRoles foreign key (RoleKey) references Core.Roles ([Key])
);
