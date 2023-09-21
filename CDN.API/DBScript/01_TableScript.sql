IF EXISTS (SELECT * FROM sysobjects WHERE name='tblUser' and xtype='U')
BEGIN
	DROP TABLE [dbo].[tblUser]
END
GO

CREATE TABLE [dbo].[tblUser]
(
	[Id][uniqueidentifier] NOT NULL,
	[Username][nvarchar](50) NOT NULL,
	[Mail][nvarchar](20) NULL,
	[PhoneNumber][nvarchar](30) NULL,
	[Skillsets][nvarchar](MAX) NULL,
	[Hobby][nvarchar](20) NULL,
	[InserDateTime][datetime] NOT NULL,
	[UpdateDateTime][datetime] NULL,
	CONSTRAINT PK_tblUser_Id PRIMARY KEY NONCLUSTERED (Id)
)
GO

