CREATE DATABASE [CqrsInPractice]
go
USE [CqrsInPractice]
GO
/****** Object:  Table [dbo].[Course]    Script Date: 6/27/2018 9:40:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Course](
	[CourseID] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Credits] [int] NOT NULL,
 CONSTRAINT [PK_Course] PRIMARY KEY CLUSTERED 
(
	[CourseID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Disenrollment]    Script Date: 6/27/2018 9:40:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Disenrollment](
	[DisenrollmentID] [bigint] IDENTITY(1,1) NOT NULL,
	[CourseID] [bigint] NOT NULL,
	[StudentID] [bigint] NOT NULL,
	[DateTime] [datetime] NOT NULL,
	[Comment] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Disenrollment] PRIMARY KEY CLUSTERED 
(
	[DisenrollmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Enrollment]    Script Date: 6/27/2018 9:40:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Enrollment](
	[EnrollmentID] [bigint] IDENTITY(1,1) NOT NULL,
	[StudentID] [bigint] NOT NULL,
	[CourseID] [bigint] NOT NULL,
	[Grade] [int] NOT NULL,
 CONSTRAINT [PK_Enrollment] PRIMARY KEY CLUSTERED 
(
	[EnrollmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Student]    Script Date: 6/27/2018 9:40:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Student](
	[StudentID] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Student] PRIMARY KEY CLUSTERED 
(
	[StudentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[Course] ON 

GO
INSERT [dbo].[Course] ([CourseID], [Name], [Credits]) VALUES (1, N'Calculus', 3)
GO
INSERT [dbo].[Course] ([CourseID], [Name], [Credits]) VALUES (2, N'Chemistry', 3)
GO
INSERT [dbo].[Course] ([CourseID], [Name], [Credits]) VALUES (3, N'Composition', 3)
GO
INSERT [dbo].[Course] ([CourseID], [Name], [Credits]) VALUES (4, N'Literature', 4)
GO
INSERT [dbo].[Course] ([CourseID], [Name], [Credits]) VALUES (5, N'Trigonometry', 4)
GO
INSERT [dbo].[Course] ([CourseID], [Name], [Credits]) VALUES (6, N'Microeconomics', 3)
GO
INSERT [dbo].[Course] ([CourseID], [Name], [Credits]) VALUES (7, N'Macroeconomics', 3)
GO
SET IDENTITY_INSERT [dbo].[Course] OFF
GO
SET IDENTITY_INSERT [dbo].[Enrollment] ON 

GO
INSERT [dbo].[Enrollment] ([EnrollmentID], [StudentID], [CourseID], [Grade]) VALUES (5, 2, 2, 1)
GO
INSERT [dbo].[Enrollment] ([EnrollmentID], [StudentID], [CourseID], [Grade]) VALUES (13, 2, 3, 3)
GO
INSERT [dbo].[Enrollment] ([EnrollmentID], [StudentID], [CourseID], [Grade]) VALUES (20, 1, 1, 1)
GO
INSERT [dbo].[Enrollment] ([EnrollmentID], [StudentID], [CourseID], [Grade]) VALUES (38, 1, 2, 3)
GO
SET IDENTITY_INSERT [dbo].[Enrollment] OFF
GO
SET IDENTITY_INSERT [dbo].[Student] ON 

GO
INSERT [dbo].[Student] ([StudentID], [Name], [Email]) VALUES (1, N'Alice', N'alice@gmail.com')
GO
INSERT [dbo].[Student] ([StudentID], [Name], [Email]) VALUES (2, N'Bob', N'bob@outlook.com')
GO
SET IDENTITY_INSERT [dbo].[Student] OFF
GO
ALTER TABLE [dbo].[Disenrollment]  WITH CHECK ADD  CONSTRAINT [FK_Disenrollment_Course] FOREIGN KEY([CourseID])
REFERENCES [dbo].[Course] ([CourseID])
GO
ALTER TABLE [dbo].[Disenrollment] CHECK CONSTRAINT [FK_Disenrollment_Course]
GO
ALTER TABLE [dbo].[Disenrollment]  WITH CHECK ADD  CONSTRAINT [FK_Disenrollment_Student] FOREIGN KEY([StudentID])
REFERENCES [dbo].[Student] ([StudentID])
GO
ALTER TABLE [dbo].[Disenrollment] CHECK CONSTRAINT [FK_Disenrollment_Student]
GO
ALTER TABLE [dbo].[Enrollment]  WITH CHECK ADD  CONSTRAINT [FK_Enrollment_Course] FOREIGN KEY([CourseID])
REFERENCES [dbo].[Course] ([CourseID])
GO
ALTER TABLE [dbo].[Enrollment] CHECK CONSTRAINT [FK_Enrollment_Course]
GO
ALTER TABLE [dbo].[Enrollment]  WITH CHECK ADD  CONSTRAINT [FK_Enrollment_Student] FOREIGN KEY([StudentID])
REFERENCES [dbo].[Student] ([StudentID])
GO
ALTER TABLE [dbo].[Enrollment] CHECK CONSTRAINT [FK_Enrollment_Student]
GO
