USE [master]
GO

IF DB_ID('Rzeszow') IS NOT NULL
  set noexec on

CREATE DATABASE Rzeszow;
go

USE Rzeszow;
go

drop table if exists Customers
create TABLE Customers
(
  id int primary key identity (1,1),
  fname varchar(20),
  lname Varchar(20),
  pesel varchar(11) unique,
  dateOfBirth date,
  phone varchar(10) unique,
  email varchar(30) unique
)


drop table if exists Transactions
create table Transactions 
(
  id int primary key identity(1,1),
  fromAccount int not null,
  fromDepartment varchar(30) not null,
  toAccount int not null,
  toDepartment varchar(30) not null,
  value float,
  dateOfTransaction datetime default CURRENT_TIMESTAMP,
)
GO 

insert into Customers VALUES
('Jan', 'Wojewoda', '11111111111', '2002-05-05', '1234', 'email@email.pl'),
('Jan', 'Komoda', '11111111112', '2002-06-05', '12345','email2email.pl')

insert into Transactions (fromAccount, fromDepartment, toDepartment, toAccount, value) VALUES
(1, 'Kraków', 'Rzeszów', 2, 300),
(2, 'Kraków', 'Rzeszów', 1, 3.5)
GO