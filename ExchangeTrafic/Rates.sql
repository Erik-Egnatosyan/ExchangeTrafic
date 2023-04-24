create database Rates
use Rates
create table Options
(
ID int not null identity,
[URL] nvarchar(1000) not null,
Headers nvarchar(255) not null,
Setting1 nvarchar(255) not null,
Setting2 nvarchar(255) not null,
Setting3 nvarchar(255) not null
)
create table TransactionLog
(
Id int not null identity,
UserID int not null,
RequestURL nvarchar(1000) not null,
ResponseLog nvarchar(max) not null,
CreatedDate datetime not null,
TransactionType bit not null
)
create table JsonKeyValues
(
ID int not null identity,
Currency nvarchar(10),
RateMoney money
)
select * from JsonKeyValues