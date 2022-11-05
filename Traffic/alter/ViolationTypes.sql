alter table ViolationTypes  add MinValue float, MaxValue float
go
alter table Invoices  add MinValue float, MaxValue float
,OwnerName nvarchar(100),Value float
alter table deletedInvoices  add MinValue float, MaxValue float
,OwnerName nvarchar(100),Value float
go
alter table Invoices  add IsPayed int ,PayDate datetime
alter table deletedInvoices  add IsPayed int ,PayDate datetime
go
--drop table InvoicesImages
create table InvoicesImages(InvoiceNo  int,FileName nvarchar(1000),ImageData Image,
LastUpdate datetime,UserName int,MyGetDate datetime)
go

alter table Invoices  add DocNo nvarchar(100)
alter table deletedInvoices  add DocNo nvarchar(100)

