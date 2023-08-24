USE [master]
GO
/****** Object:  Database [SWP391OnGoingReport]    Script Date: 24/8/2023 8:13:42 AM ******/
CREATE DATABASE [SWP391OnGoingReport]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'SWP391OnGoingReport', FILENAME = N'E:\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\SWP391OnGoingReport.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'SWP391OnGoingReport_log', FILENAME = N'E:\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\SWP391OnGoingReport_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [SWP391OnGoingReport] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [SWP391OnGoingReport].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [SWP391OnGoingReport] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [SWP391OnGoingReport] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [SWP391OnGoingReport] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [SWP391OnGoingReport] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [SWP391OnGoingReport] SET ARITHABORT OFF 
GO
ALTER DATABASE [SWP391OnGoingReport] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [SWP391OnGoingReport] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [SWP391OnGoingReport] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [SWP391OnGoingReport] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [SWP391OnGoingReport] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [SWP391OnGoingReport] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [SWP391OnGoingReport] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [SWP391OnGoingReport] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [SWP391OnGoingReport] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [SWP391OnGoingReport] SET  DISABLE_BROKER 
GO
ALTER DATABASE [SWP391OnGoingReport] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [SWP391OnGoingReport] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [SWP391OnGoingReport] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [SWP391OnGoingReport] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [SWP391OnGoingReport] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [SWP391OnGoingReport] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [SWP391OnGoingReport] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [SWP391OnGoingReport] SET RECOVERY FULL 
GO
ALTER DATABASE [SWP391OnGoingReport] SET  MULTI_USER 
GO
ALTER DATABASE [SWP391OnGoingReport] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [SWP391OnGoingReport] SET DB_CHAINING OFF 
GO
ALTER DATABASE [SWP391OnGoingReport] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [SWP391OnGoingReport] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [SWP391OnGoingReport] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [SWP391OnGoingReport] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [SWP391OnGoingReport] SET QUERY_STORE = ON
GO
ALTER DATABASE [SWP391OnGoingReport] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [SWP391OnGoingReport]
GO
/****** Object:  Table [dbo].[Class]    Script Date: 24/8/2023 8:13:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Class](
	[classID] [uniqueidentifier] NOT NULL,
	[userID] [uniqueidentifier] NOT NULL,
	[courseID] [uniqueidentifier] NOT NULL,
	[className] [nvarchar](100) NOT NULL,
	[EnrollCode] [nvarchar](50) NOT NULL,
	[timeStart] [datetime] NOT NULL,
	[timeEnd] [datetime] NULL,
	[isDeleted] [bit] NOT NULL,
	[isCompleted] [bit] NOT NULL,
 CONSTRAINT [PK_Class] PRIMARY KEY CLUSTERED 
(
	[classID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Course]    Script Date: 24/8/2023 8:13:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Course](
	[courseID] [uniqueidentifier] NOT NULL,
	[userID] [uniqueidentifier] NOT NULL,
	[courseCode] [varchar](50) NOT NULL,
	[courseName] [nvarchar](100) NOT NULL,
	[timeCreated] [datetime] NOT NULL,
	[isActive] [bit] NOT NULL,
	[isDelete] [bit] NOT NULL,
 CONSTRAINT [PK_Course] PRIMARY KEY CLUSTERED 
(
	[courseID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Project]    Script Date: 24/8/2023 8:13:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Project](
	[projectID] [uniqueidentifier] NOT NULL,
	[classID] [uniqueidentifier] NOT NULL,
	[projectName] [nvarchar](100) NOT NULL,
	[description] [nvarchar](500) NOT NULL,
	[isSelected] [bit] NOT NULL,
	[isDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Project] PRIMARY KEY CLUSTERED 
(
	[projectID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProjectTeam]    Script Date: 24/8/2023 8:13:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectTeam](
	[projectTeamID] [uniqueidentifier] NOT NULL,
	[projectID] [uniqueidentifier] NOT NULL,
	[teamName] [nvarchar](50) NOT NULL,
	[timeStart] [datetime] NOT NULL,
	[timeEnd] [datetime] NULL,
	[status] [int] NOT NULL,
 CONSTRAINT [PK_ProjectTeam] PRIMARY KEY CLUSTERED 
(
	[projectTeamID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 24/8/2023 8:13:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[roleID] [uniqueidentifier] NOT NULL,
	[roleName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[roleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StudentClass]    Script Date: 24/8/2023 8:13:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StudentClass](
	[studentClassID] [uniqueidentifier] NOT NULL,
	[userID] [uniqueidentifier] NOT NULL,
	[classID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_StudentClass] PRIMARY KEY CLUSTERED 
(
	[studentClassID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StudentTask]    Script Date: 24/8/2023 8:13:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StudentTask](
	[studentTaskID] [uniqueidentifier] NOT NULL,
	[taskID] [uniqueidentifier] NOT NULL,
	[userID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_StudentTask] PRIMARY KEY CLUSTERED 
(
	[studentTaskID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Task]    Script Date: 24/8/2023 8:13:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Task](
	[taskID] [uniqueidentifier] NOT NULL,
	[userID] [uniqueidentifier] NOT NULL,
	[projectID] [uniqueidentifier] NOT NULL,
	[taskName] [nvarchar](100) NOT NULL,
	[description] [nvarchar](500) NOT NULL,
	[startTime] [datetime] NOT NULL,
	[endTime] [datetime] NULL,
	[status] [int] NOT NULL,
	[isDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Task] PRIMARY KEY CLUSTERED 
(
	[taskID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TeamMember]    Script Date: 24/8/2023 8:13:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TeamMember](
	[teamMemberID] [uniqueidentifier] NOT NULL,
	[projectTeamID] [uniqueidentifier] NOT NULL,
	[userID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_TeamMember] PRIMARY KEY CLUSTERED 
(
	[teamMemberID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TeamRequest]    Script Date: 24/8/2023 8:13:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TeamRequest](
	[requestID] [uniqueidentifier] NOT NULL,
	[userID] [uniqueidentifier] NOT NULL,
	[classID] [uniqueidentifier] NOT NULL,
	[team] [uniqueidentifier] NOT NULL,
	[projectID] [uniqueidentifier] NOT NULL,
	[teamName] [nvarchar](50) NOT NULL,
	[status] [char](10) NOT NULL,
 CONSTRAINT [PK_TeamRequest] PRIMARY KEY CLUSTERED 
(
	[requestID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 24/8/2023 8:13:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[userID] [uniqueidentifier] NOT NULL,
	[fullName] [nvarchar](50) NOT NULL,
	[email] [varchar](50) NOT NULL,
	[password] [varchar](50) NOT NULL,
	[roleID] [uniqueidentifier] NOT NULL,
	[isBan] [bit] NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[userID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserCourse]    Script Date: 24/8/2023 8:13:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserCourse](
	[userCourseID] [uniqueidentifier] NOT NULL,
	[courseID] [uniqueidentifier] NOT NULL,
	[userID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_UserCourse] PRIMARY KEY CLUSTERED 
(
	[userCourseID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Class] ([classID], [userID], [courseID], [className], [EnrollCode], [timeStart], [timeEnd], [isDeleted], [isCompleted]) VALUES (N'6df4c312-07ac-4538-9a40-66cab5c0044f', N'a03b0b85-0143-49d1-89d8-1d7b7f56efc6', N'ff8e08d1-5b0d-4165-a68c-ec7d8a020bd8', N'Class 2', N'123', CAST(N'2023-08-13T08:42:45.433' AS DateTime), CAST(N'2023-08-13T08:42:45.433' AS DateTime), 0, 0)
INSERT [dbo].[Class] ([classID], [userID], [courseID], [className], [EnrollCode], [timeStart], [timeEnd], [isDeleted], [isCompleted]) VALUES (N'7daa8050-d826-43e4-85e0-aa63e53fe173', N'a03b0b85-0143-49d1-89d8-1d7b7f56efc6', N'e91ca1e5-9262-49fb-84f5-1f14560afeea', N'Class 3', N'123', CAST(N'2023-08-16T02:08:02.700' AS DateTime), CAST(N'2023-08-16T02:08:02.700' AS DateTime), 0, 0)
INSERT [dbo].[Class] ([classID], [userID], [courseID], [className], [EnrollCode], [timeStart], [timeEnd], [isDeleted], [isCompleted]) VALUES (N'c719cee9-346f-4d24-aa16-ce37c0a53c51', N'a03b0b85-0143-49d1-89d8-1d7b7f56efc6', N'ff8e08d1-5b0d-4165-a68c-ec7d8a020bd8', N'SE1601', N'123', CAST(N'2023-08-13T07:10:30.707' AS DateTime), CAST(N'2023-08-13T07:10:30.707' AS DateTime), 0, 0)
GO
INSERT [dbo].[Course] ([courseID], [userID], [courseCode], [courseName], [timeCreated], [isActive], [isDelete]) VALUES (N'e91ca1e5-9262-49fb-84f5-1f14560afeea', N'a03b0b85-0143-49d1-89d8-1d7b7f56efc6', N'SWP490', N'Đồ án tốt nghiệp', CAST(N'2023-08-16T09:07:35.307' AS DateTime), 1, 0)
INSERT [dbo].[Course] ([courseID], [userID], [courseCode], [courseName], [timeCreated], [isActive], [isDelete]) VALUES (N'ff8e08d1-5b0d-4165-a68c-ec7d8a020bd8', N'c7b1ce04-c419-4e12-a34b-57d7729fa60f', N'SWP391', N'Do an nho', CAST(N'2023-08-12T16:41:04.977' AS DateTime), 1, 0)
GO
INSERT [dbo].[Project] ([projectID], [classID], [projectName], [description], [isSelected], [isDeleted]) VALUES (N'87e92589-f3b9-4a4d-8359-39303726d441', N'6df4c312-07ac-4538-9a40-66cab5c0044f', N'sKBFAIBIA', N'OABGOAGBOA', 0, 0)
GO
INSERT [dbo].[Role] ([roleID], [roleName]) VALUES (N'be3960be-a4c8-4932-8635-3d5005c5e8dc', N'Student')
INSERT [dbo].[Role] ([roleID], [roleName]) VALUES (N'7008a7fb-4e50-47c4-8378-506c2fcf5c2b', N'Admin')
INSERT [dbo].[Role] ([roleID], [roleName]) VALUES (N'767fe372-c5a9-4ca0-bdf4-81b288bac864', N'Teacher')
GO
INSERT [dbo].[Task] ([taskID], [userID], [projectID], [taskName], [description], [startTime], [endTime], [status], [isDeleted]) VALUES (N'da228b3e-a39c-4d19-bea4-05cd2679b7f9', N'566b1628-b245-42bc-99ab-b6074c4d4acd', N'87e92589-f3b9-4a4d-8359-39303726d441', N'sfaaefafa', N'stâfafriâfng', CAST(N'2023-08-11T00:00:00.000' AS DateTime), CAST(N'2023-08-11T00:00:00.000' AS DateTime), 0, 1)
INSERT [dbo].[Task] ([taskID], [userID], [projectID], [taskName], [description], [startTime], [endTime], [status], [isDeleted]) VALUES (N'f0fcef8e-96a1-4445-a7bc-64b8c34587e4', N'566b1628-b245-42bc-99ab-b6074c4d4acd', N'87e92589-f3b9-4a4d-8359-39303726d441', N'ưgroigoisondsonsdo', N'ươgogwowgnipgw', CAST(N'2023-08-11T00:00:00.000' AS DateTime), CAST(N'2023-08-11T00:00:00.000' AS DateTime), 0, 0)
INSERT [dbo].[Task] ([taskID], [userID], [projectID], [taskName], [description], [startTime], [endTime], [status], [isDeleted]) VALUES (N'65a61e69-c645-4a9b-acb1-bcf76d7bdec3', N'1b26bbe5-cc0a-4c28-b1d2-235e10f9dc03', N'87e92589-f3b9-4a4d-8359-39303726d441', N'abcxyz', N'abcxyz', CAST(N'2023-08-11T00:00:00.000' AS DateTime), CAST(N'2023-08-11T00:00:00.000' AS DateTime), 1, 0)
GO
INSERT [dbo].[User] ([userID], [fullName], [email], [password], [roleID], [isBan]) VALUES (N'a03b0b85-0143-49d1-89d8-1d7b7f56efc6', N'Nguyễn Thị Minh Khai', N'nguyenthiminhkhai@gmail.com', N'123456', N'767fe372-c5a9-4ca0-bdf4-81b288bac864', 0)
INSERT [dbo].[User] ([userID], [fullName], [email], [password], [roleID], [isBan]) VALUES (N'1b26bbe5-cc0a-4c28-b1d2-235e10f9dc03', N'Học sinh 1', N'hocsinh1@gmail.com', N'123456', N'be3960be-a4c8-4932-8635-3d5005c5e8dc', 0)
INSERT [dbo].[User] ([userID], [fullName], [email], [password], [roleID], [isBan]) VALUES (N'c7b1ce04-c419-4e12-a34b-57d7729fa60f', N'Võ Thị Sáu - University', N'vothisau@gmail.com', N'123456', N'7008a7fb-4e50-47c4-8378-506c2fcf5c2b', 0)
INSERT [dbo].[User] ([userID], [fullName], [email], [password], [roleID], [isBan]) VALUES (N'566b1628-b245-42bc-99ab-b6074c4d4acd', N'Lê Văn Việt', N'levanviet@gmail.com', N'123456', N'be3960be-a4c8-4932-8635-3d5005c5e8dc', 0)
GO
INSERT [dbo].[UserCourse] ([userCourseID], [courseID], [userID]) VALUES (N'04247a99-4213-4466-a8e0-26650a4af7d5', N'9bd68e11-cd7c-4bdf-9ea0-d31f466ab5f1', N'a03b0b85-0143-49d1-89d8-1d7b7f56efc6')
GO
ALTER TABLE [dbo].[Class]  WITH CHECK ADD  CONSTRAINT [FK_Class_Course] FOREIGN KEY([courseID])
REFERENCES [dbo].[Course] ([courseID])
GO
ALTER TABLE [dbo].[Class] CHECK CONSTRAINT [FK_Class_Course]
GO
ALTER TABLE [dbo].[Class]  WITH CHECK ADD  CONSTRAINT [FK_Class_User] FOREIGN KEY([userID])
REFERENCES [dbo].[User] ([userID])
GO
ALTER TABLE [dbo].[Class] CHECK CONSTRAINT [FK_Class_User]
GO
ALTER TABLE [dbo].[Course]  WITH CHECK ADD  CONSTRAINT [FK_Course_User] FOREIGN KEY([userID])
REFERENCES [dbo].[User] ([userID])
GO
ALTER TABLE [dbo].[Course] CHECK CONSTRAINT [FK_Course_User]
GO
ALTER TABLE [dbo].[Project]  WITH CHECK ADD  CONSTRAINT [FK_Project_Class] FOREIGN KEY([classID])
REFERENCES [dbo].[Class] ([classID])
GO
ALTER TABLE [dbo].[Project] CHECK CONSTRAINT [FK_Project_Class]
GO
ALTER TABLE [dbo].[ProjectTeam]  WITH CHECK ADD  CONSTRAINT [FK_ProjectTeam_Project] FOREIGN KEY([projectID])
REFERENCES [dbo].[Project] ([projectID])
GO
ALTER TABLE [dbo].[ProjectTeam] CHECK CONSTRAINT [FK_ProjectTeam_Project]
GO
ALTER TABLE [dbo].[StudentClass]  WITH CHECK ADD  CONSTRAINT [FK_StudentClass_Class] FOREIGN KEY([classID])
REFERENCES [dbo].[Class] ([classID])
GO
ALTER TABLE [dbo].[StudentClass] CHECK CONSTRAINT [FK_StudentClass_Class]
GO
ALTER TABLE [dbo].[StudentClass]  WITH CHECK ADD  CONSTRAINT [FK_StudentClass_User] FOREIGN KEY([userID])
REFERENCES [dbo].[User] ([userID])
GO
ALTER TABLE [dbo].[StudentClass] CHECK CONSTRAINT [FK_StudentClass_User]
GO
ALTER TABLE [dbo].[StudentTask]  WITH CHECK ADD  CONSTRAINT [FK_StudentTask_Task] FOREIGN KEY([taskID])
REFERENCES [dbo].[Task] ([taskID])
GO
ALTER TABLE [dbo].[StudentTask] CHECK CONSTRAINT [FK_StudentTask_Task]
GO
ALTER TABLE [dbo].[StudentTask]  WITH CHECK ADD  CONSTRAINT [FK_StudentTask_User] FOREIGN KEY([userID])
REFERENCES [dbo].[User] ([userID])
GO
ALTER TABLE [dbo].[StudentTask] CHECK CONSTRAINT [FK_StudentTask_User]
GO
ALTER TABLE [dbo].[Task]  WITH CHECK ADD  CONSTRAINT [FK_Task_Project] FOREIGN KEY([projectID])
REFERENCES [dbo].[Project] ([projectID])
GO
ALTER TABLE [dbo].[Task] CHECK CONSTRAINT [FK_Task_Project]
GO
ALTER TABLE [dbo].[TeamMember]  WITH CHECK ADD  CONSTRAINT [FK_TeamMember_ProjectTeam] FOREIGN KEY([projectTeamID])
REFERENCES [dbo].[ProjectTeam] ([projectTeamID])
GO
ALTER TABLE [dbo].[TeamMember] CHECK CONSTRAINT [FK_TeamMember_ProjectTeam]
GO
ALTER TABLE [dbo].[TeamMember]  WITH CHECK ADD  CONSTRAINT [FK_TeamMember_User] FOREIGN KEY([userID])
REFERENCES [dbo].[User] ([userID])
GO
ALTER TABLE [dbo].[TeamMember] CHECK CONSTRAINT [FK_TeamMember_User]
GO
ALTER TABLE [dbo].[TeamRequest]  WITH CHECK ADD  CONSTRAINT [FK_TeamRequest_Class] FOREIGN KEY([classID])
REFERENCES [dbo].[Class] ([classID])
GO
ALTER TABLE [dbo].[TeamRequest] CHECK CONSTRAINT [FK_TeamRequest_Class]
GO
ALTER TABLE [dbo].[TeamRequest]  WITH CHECK ADD  CONSTRAINT [FK_TeamRequest_User] FOREIGN KEY([userID])
REFERENCES [dbo].[User] ([userID])
GO
ALTER TABLE [dbo].[TeamRequest] CHECK CONSTRAINT [FK_TeamRequest_User]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Role] FOREIGN KEY([roleID])
REFERENCES [dbo].[Role] ([roleID])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Role]
GO
USE [master]
GO
ALTER DATABASE [SWP391OnGoingReport] SET  READ_WRITE 
GO
