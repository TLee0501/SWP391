USE [master]
GO
/****** Object:  Database [SWP391OnGoingReport]    Script Date: 28-Aug-23 8:51:38 AM ******/
CREATE DATABASE [SWP391OnGoingReport]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'SWP391OnGoingReport', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\SWP391OnGoingReport.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'SWP391OnGoingReport_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\SWP391OnGoingReport_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [SWP391OnGoingReport] SET COMPATIBILITY_LEVEL = 160
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
ALTER DATABASE [SWP391OnGoingReport] SET  ENABLE_BROKER 
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
EXEC sys.sp_db_vardecimal_storage_format N'SWP391OnGoingReport', N'ON'
GO
ALTER DATABASE [SWP391OnGoingReport] SET QUERY_STORE = ON
GO
ALTER DATABASE [SWP391OnGoingReport] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [SWP391OnGoingReport]
GO
/****** Object:  Table [dbo].[Class]    Script Date: 28-Aug-23 8:51:38 AM ******/
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
	[isDeleted] [bit] NOT NULL,
	[status] [int] NOT NULL,
	[semesterId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Class] PRIMARY KEY CLUSTERED 
(
	[classID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Course]    Script Date: 28-Aug-23 8:51:38 AM ******/
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
	[semesterId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Course] PRIMARY KEY CLUSTERED 
(
	[courseID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Project]    Script Date: 28-Aug-23 8:51:38 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Project](
	[projectID] [uniqueidentifier] NOT NULL,
	[classID] [uniqueidentifier] NOT NULL,
	[projectName] [nvarchar](max) NOT NULL,
	[description] [nvarchar](max) NULL,
	[functionalReq] [nvarchar](max) NULL,
	[nonfunctionalReq] [nvarchar](max) NULL,
	[isSelected] [bit] NOT NULL,
	[isDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Project] PRIMARY KEY CLUSTERED 
(
	[projectID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProjectTeam]    Script Date: 28-Aug-23 8:51:38 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectTeam](
	[projectTeamID] [uniqueidentifier] NOT NULL,
	[projectID] [uniqueidentifier] NOT NULL,
	[teamName] [nvarchar](50) NOT NULL,
	[status] [int] NOT NULL,
	[leaderId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ProjectTeam] PRIMARY KEY CLUSTERED 
(
	[projectTeamID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 28-Aug-23 8:51:38 AM ******/
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
/****** Object:  Table [dbo].[Semester]    Script Date: 28-Aug-23 8:51:38 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Semester](
	[semesterId] [uniqueidentifier] NOT NULL,
	[semeterName] [nvarchar](50) NOT NULL,
	[startTime] [datetime] NOT NULL,
	[endTime] [datetime] NOT NULL,
 CONSTRAINT [PK_Semester] PRIMARY KEY CLUSTERED 
(
	[semesterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StudentClass]    Script Date: 28-Aug-23 8:51:38 AM ******/
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
/****** Object:  Table [dbo].[StudentTask]    Script Date: 28-Aug-23 8:51:38 AM ******/
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
/****** Object:  Table [dbo].[Task]    Script Date: 28-Aug-23 8:51:38 AM ******/
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
	[startTime] [datetime] NULL,
	[endTime] [datetime] NULL,
	[status] [int] NOT NULL,
	[isDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Task] PRIMARY KEY CLUSTERED 
(
	[taskID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TeamMember]    Script Date: 28-Aug-23 8:51:38 AM ******/
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
/****** Object:  Table [dbo].[User]    Script Date: 28-Aug-23 8:51:38 AM ******/
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
	[MSSV] [varchar](50) NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[userID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserClass]    Script Date: 28-Aug-23 8:51:38 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserClass](
	[userClassID] [uniqueidentifier] NOT NULL,
	[classID] [uniqueidentifier] NOT NULL,
	[userID] [uniqueidentifier] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserCourse]    Script Date: 28-Aug-23 8:51:38 AM ******/
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
INSERT [dbo].[Class] ([classID], [userID], [courseID], [className], [EnrollCode], [isDeleted], [status], [semesterId]) VALUES (N'805f5cc0-2210-43a5-950c-2f57ff329629', N'14176936-bddc-4f5d-b928-a891bbc7caef', N'ff8e08d1-5b0d-4165-a68c-ec7d8a020bd8', N'Class 1', N'123', 0, 0, N'00000000-0000-0000-0000-000000000000')
GO
INSERT [dbo].[Course] ([courseID], [userID], [courseCode], [courseName], [timeCreated], [isActive], [isDelete], [semesterId]) VALUES (N'e91ca1e5-9262-49fb-84f5-1f14560afeea', N'a03b0b85-0143-49d1-89d8-1d7b7f56efc6', N'SWP333', N'string regrt', CAST(N'2023-08-16T09:07:35.307' AS DateTime), 1, 0, N'6a9366a3-6c69-402e-b783-53b596ee1c20')
INSERT [dbo].[Course] ([courseID], [userID], [courseCode], [courseName], [timeCreated], [isActive], [isDelete], [semesterId]) VALUES (N'ff8e08d1-5b0d-4165-a68c-ec7d8a020bd8', N'c7b1ce04-c419-4e12-a34b-57d7729fa60f', N'SWP391', N'Do an nho', CAST(N'2023-08-12T16:41:04.977' AS DateTime), 1, 0, N'6a9366a3-6c69-402e-b783-53b596ee1c20')
GO
INSERT [dbo].[Project] ([projectID], [classID], [projectName], [description], [functionalReq], [nonfunctionalReq], [isSelected], [isDeleted]) VALUES (N'fd400739-d1ba-4e32-9f6d-36b7e372cdd6', N'805f5cc0-2210-43a5-950c-2f57ff329629', N'Project 1', N'description', N'functionalReq', N'nonfunctionalReq', 0, 0)
INSERT [dbo].[Project] ([projectID], [classID], [projectName], [description], [functionalReq], [nonfunctionalReq], [isSelected], [isDeleted]) VALUES (N'fcc3feee-3398-448c-afd1-a2977e95b3a6', N'805f5cc0-2210-43a5-950c-2f57ff329629', N'Project 2', N'description', N'functionalReq', N'nonfunctionalReq', 0, 0)
GO
INSERT [dbo].[ProjectTeam] ([projectTeamID], [projectID], [teamName], [status], [leaderId]) VALUES (N'7004e602-ab91-482e-90b9-6ec6b2c33a72', N'fd400739-d1ba-4e32-9f6d-36b7e372cdd6', N'G01', 1, N'1b26bbe5-cc0a-4c28-b1d2-235e10f9dc03')
INSERT [dbo].[ProjectTeam] ([projectTeamID], [projectID], [teamName], [status], [leaderId]) VALUES (N'df64628e-6944-4ffe-8bd3-eab0b5c2ab64', N'fcc3feee-3398-448c-afd1-a2977e95b3a6', N'G02', 1, N'bc3e5d3b-d63f-404a-ab3f-2bc23395ab06')
GO
INSERT [dbo].[Role] ([roleID], [roleName]) VALUES (N'be3960be-a4c8-4932-8635-3d5005c5e8dc', N'Student')
INSERT [dbo].[Role] ([roleID], [roleName]) VALUES (N'7008a7fb-4e50-47c4-8378-506c2fcf5c2b', N'University')
INSERT [dbo].[Role] ([roleID], [roleName]) VALUES (N'767fe372-c5a9-4ca0-bdf4-81b288bac864', N'Teacher')
GO
INSERT [dbo].[Semester] ([semesterId], [semeterName], [startTime], [endTime]) VALUES (N'6a9366a3-6c69-402e-b783-53b596ee1c20', N'FA23', CAST(N'2023-09-04T00:00:00.000' AS DateTime), CAST(N'2024-01-18T00:00:00.000' AS DateTime))
GO
INSERT [dbo].[TeamMember] ([teamMemberID], [projectTeamID], [userID]) VALUES (N'0b6c9128-f2d2-4b58-99d5-0c009034415f', N'7004e602-ab91-482e-90b9-6ec6b2c33a72', N'cd62c452-c6e4-460e-a5f9-11c1933dd634')
INSERT [dbo].[TeamMember] ([teamMemberID], [projectTeamID], [userID]) VALUES (N'29f1e9d0-9379-4a09-984b-ddb6b03f62fa', N'7004e602-ab91-482e-90b9-6ec6b2c33a72', N'1b26bbe5-cc0a-4c28-b1d2-235e10f9dc03')
INSERT [dbo].[TeamMember] ([teamMemberID], [projectTeamID], [userID]) VALUES (N'01df0ce6-1eee-416c-afe8-f359c2bf6477', N'df64628e-6944-4ffe-8bd3-eab0b5c2ab64', N'bc3e5d3b-d63f-404a-ab3f-2bc23395ab06')
GO
INSERT [dbo].[User] ([userID], [fullName], [email], [password], [roleID], [isBan], [MSSV]) VALUES (N'cd62c452-c6e4-460e-a5f9-11c1933dd634', N'Sinh Viên 4', N'sinhvien4@gmail.com', N'123456', N'be3960be-a4c8-4932-8635-3d5005c5e8dc', 0, N'SE0005')
INSERT [dbo].[User] ([userID], [fullName], [email], [password], [roleID], [isBan], [MSSV]) VALUES (N'a03b0b85-0143-49d1-89d8-1d7b7f56efc6', N'Nguyễn Thị Minh Khai', N'nguyenthiminhkhai@gmail.com', N'123456', N'767fe372-c5a9-4ca0-bdf4-81b288bac864', 0, NULL)
INSERT [dbo].[User] ([userID], [fullName], [email], [password], [roleID], [isBan], [MSSV]) VALUES (N'1b26bbe5-cc0a-4c28-b1d2-235e10f9dc03', N'Học sinh 1', N'hocsinh1@gmail.com', N'123456', N'be3960be-a4c8-4932-8635-3d5005c5e8dc', 0, N'SE0002')
INSERT [dbo].[User] ([userID], [fullName], [email], [password], [roleID], [isBan], [MSSV]) VALUES (N'bc3e5d3b-d63f-404a-ab3f-2bc23395ab06', N'Học Sinh 3', N'hocsinh3@gmail.com', N'123456', N'be3960be-a4c8-4932-8635-3d5005c5e8dc', 0, N'SE0004')
INSERT [dbo].[User] ([userID], [fullName], [email], [password], [roleID], [isBan], [MSSV]) VALUES (N'0abba85f-2e5e-41ea-a177-525971b4a6db', N'Học sinh 2', N'hocsinh2@gmail.com', N'123456', N'be3960be-a4c8-4932-8635-3d5005c5e8dc', 0, N'SE0003')
INSERT [dbo].[User] ([userID], [fullName], [email], [password], [roleID], [isBan], [MSSV]) VALUES (N'c7b1ce04-c419-4e12-a34b-57d7729fa60f', N'Võ Thị Sáu - University', N'vothisau@gmail.com', N'123456', N'7008a7fb-4e50-47c4-8378-506c2fcf5c2b', 0, NULL)
INSERT [dbo].[User] ([userID], [fullName], [email], [password], [roleID], [isBan], [MSSV]) VALUES (N'14176936-bddc-4f5d-b928-a891bbc7caef', N'Giảng Viên 2', N'giangvien2@gmail.com', N'123456', N'767fe372-c5a9-4ca0-bdf4-81b288bac864', 0, NULL)
INSERT [dbo].[User] ([userID], [fullName], [email], [password], [roleID], [isBan], [MSSV]) VALUES (N'566b1628-b245-42bc-99ab-b6074c4d4acd', N'Lê Văn Việt', N'levanviet@gmail.com', N'123456', N'be3960be-a4c8-4932-8635-3d5005c5e8dc', 0, N'SE0001')
INSERT [dbo].[User] ([userID], [fullName], [email], [password], [roleID], [isBan], [MSSV]) VALUES (N'64344d24-5f60-43de-9933-ba55bd6d2399', N'Teacher 1', N'teacher1@gmail.com', N'123456', N'767fe372-c5a9-4ca0-bdf4-81b288bac864', 0, NULL)
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
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Role] FOREIGN KEY([roleID])
REFERENCES [dbo].[Role] ([roleID])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Role]
GO
USE [master]
GO
ALTER DATABASE [SWP391OnGoingReport] SET  READ_WRITE 
GO
