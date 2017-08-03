USE [master]
GO

DECLARE @sql VARCHAR(MAX)
DECLARE @sql_path VARCHAR(1000)
DECLARE @data_path VARCHAR(1000)
DECLARE @log_path VARCHAR(1000)
DECLARE @version VARCHAR(2)

SELECT @version = SUBSTRING(CONVERT(VARCHAR(100), SERVERPROPERTY('productversion')), 1, 2)

SET @sql_path = N'C:\Program Files\Microsoft SQL Server\MSSQL' + @version + '.SQLEXPRESS\MSSQL\DATA\'
SET @data_path = @sql_path + N'EmployeeDirectory.mdf'
SET @log_path = @sql_path + N'EmployeeDirectory_log.ldf'

SET @sql = '
CREATE DATABASE [EmployeeDirectory] ON  PRIMARY 
( NAME = N''EmployeeDirectory'', FILENAME = N''' + @data_path + ''', SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N''EmployeeDirectory_log'', FILENAME = N''' + @log_path + ''' , SIZE = 2048KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
'
EXEC(@sql)


ALTER DATABASE [EmployeeDirectory] SET RECOVERY SIMPLE 
GO

-- Create employeeDirWeb Login
USE [master]
GO

CREATE LOGIN [employeeDirWeb] WITH PASSWORD='p@ssw0rd', DEFAULT_DATABASE=[EmployeeDirectory], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO

ALTER SERVER ROLE [sysadmin] ADD MEMBER [employeeDirWeb]
GO


USE [EmployeeDirectory]
GO

CREATE USER [employeeWebDir] FOR LOGIN [employeeDirWeb] WITH DEFAULT_SCHEMA=[dbo]
GO

USE [EmployeeDirectory]
GO

/****** Object:  Table [dbo].[User] ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[User](
	[ID] [int] NOT NULL,
	[FirstName] [nvarchar](200) NULL,
	[LastName] [nvarchar](200) NULL,
	[Email] [nvarchar](200) NOT NULL,
	[Password] [nvarchar](100) NOT NULL,
) ON [PRIMARY]

GO
CREATE TABLE [dbo].[Employee](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[JobTitle] [nvarchar](200) NOT NULL,
	[Location] [nvarchar](200) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[PhoneNumber] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_Employee] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

GO

CREATE TABLE [dbo].[PhoneNumber](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](200) NOT NULL,
	[Number] [nvarchar](200) NOT NULL,
	[EmployeeId] [int]  NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[PhoneNumber]  WITH CHECK ADD  CONSTRAINT [FK_PhoneNumber_Employee] FOREIGN KEY([EmployeeId])
REFERENCES [dbo].[Employee] ([ID])
GO

ALTER TABLE [dbo].[PhoneNumber] CHECK CONSTRAINT [FK_PhoneNumber_Employee]


/******* Create Admin User *******/
USE [EmployeeDirectory]
GO
INSERT INTO [dbo].[User] ([ID],[FirstName],[Email],[Password]) VALUES (0, 'Admin', 'admin@example.com', 'p@ssw0rd')
GO