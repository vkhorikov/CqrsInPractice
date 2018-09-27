CREATE DATABASE [CqrsInPracticeReads]
go
USE [CqrsInPracticeReads]
GO
CREATE TABLE [dbo].[Student](
	[StudentID] [bigint] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[NumberOfEnrollments] [int] NOT NULL,
	[FirstCourseName] [nvarchar](50) NULL,
	[FirstCourseCredits] [int] NULL,
	[FirstCourseGrade] [int] NULL,
	[SecondCourseName] [nvarchar](50) NULL,
	[SecondCourseCredits] [int] NULL,
	[SecondCourseGrade] [int] NULL,
 CONSTRAINT [PK_Student] PRIMARY KEY CLUSTERED 
(
	[StudentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
INSERT [dbo].[Student] ([StudentID], [Name], [Email], [NumberOfEnrollments], [FirstCourseName], [FirstCourseCredits], [FirstCourseGrade], [SecondCourseName], [SecondCourseCredits], [SecondCourseGrade]) VALUES (1, N'Alice', N'alice@gmail.com', 2, N'Calculus', 3, 1, N'Composition', 3, 2)
GO
INSERT [dbo].[Student] ([StudentID], [Name], [Email], [NumberOfEnrollments], [FirstCourseName], [FirstCourseCredits], [FirstCourseGrade], [SecondCourseName], [SecondCourseCredits], [SecondCourseGrade]) VALUES (2, N'Bob', N'bob@outlook.com', 1, N'Composition', 3, 1, NULL, NULL, NULL)
GO
