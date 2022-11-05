CREATE TABLE [dbo].[Invoices2](
	[InvoiceNo] [int] NOT NULL,
	[DayDate] [datetime] NULL,
	[LabelTypeId] [int] NULL,
	[CarTypeId] [int] NULL,
	"EmpId" [int] NULL,
	[Notes] [nvarchar](1000) NULL,
	[UserName] [int] NULL,
	[MyGetDate] [datetime] NULL,
	[LabelData] [nvarchar](100) NULL,
	[OwnerName] [nvarchar](100) NULL,
	[Value] [float] NULL,
	[DocNo] [nvarchar](100) NULL,
	[IssueNo] [nvarchar](100) NULL,
 CONSTRAINT [PK_Invoices2] PRIMARY KEY CLUSTERED 
(
	[InvoiceNo] ASC
) ) 
go
CREATE TABLE [dbo].[DeletedInvoices2](
	[DeletedDate] [datetime] NULL,
	[UserDelete] [int] NULL,
	[LastLine] [int] NULL,
	[State] [varchar](100) NULL,
	[InvoiceNo] [int] NOT NULL,
	[DayDate] [datetime] NULL,
	[LabelTypeId] [int] NULL,
	[CarTypeId] [int] NULL,
	"EmpId" [int] NULL,
	[Notes] [nvarchar](1000) NULL,
	[UserName] [int] NULL,
	[MyGetDate] [datetime] NULL,
	[LabelData] [nvarchar](100) NULL,
	[OwnerName] [nvarchar](100) NULL,
	[Value] [float] NULL,
	[DocNo] [nvarchar](100) NULL,
	[IssueNo] [nvarchar](100) NULL,
 ) 
go
