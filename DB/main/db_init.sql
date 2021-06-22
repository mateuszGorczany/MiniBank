USE [master]
GO

IF DB_ID('Main') IS NOT NULL
  set noexec on

CREATE DATABASE Main;
go

USE Main;
go

drop table if exists Departments;
create table Departments
(
  id int primary key identity(1,1),
  name varchar(30) unique
);
GO

drop table if exists Accounts;
GO
create table Accounts
(
  id int primary key identity (1,1),
  department_id int not null,
  customer_id int not null,
  balance float not null default 0,
  
  constraint [ERROR: Balance value cannot be < 0]
  check (balance >= 0)
);
GO

create trigger NonEmptyDeletion
on Accounts
for DELETE
as 
	if (select balance from deleted) <> 0
    BEGIN
        THROW 50001, 'Cannot delete non-empty account', 10;
    END
GO

insert into Departments values
('Rzeszów'),
('Kraków')

insert into Accounts (department_id, customer_id, balance) values 
(1, 1, 3000),
(2, 2, 1300.0)
