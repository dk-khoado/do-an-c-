USE [master]
GO
/****** Object:  Database [DatabaseGameBai]    Script Date: 7/6/2019 9:33:30 AM ******/
CREATE DATABASE [DatabaseGameBai]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'DatabaseGameBai', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS01\MSSQL\DATA\DatabaseGameBai.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'DatabaseGameBai_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS01\MSSQL\DATA\DatabaseGameBai_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [DatabaseGameBai] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [DatabaseGameBai].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [DatabaseGameBai] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [DatabaseGameBai] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [DatabaseGameBai] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [DatabaseGameBai] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [DatabaseGameBai] SET ARITHABORT OFF 
GO
ALTER DATABASE [DatabaseGameBai] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [DatabaseGameBai] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [DatabaseGameBai] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [DatabaseGameBai] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [DatabaseGameBai] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [DatabaseGameBai] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [DatabaseGameBai] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [DatabaseGameBai] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [DatabaseGameBai] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [DatabaseGameBai] SET  DISABLE_BROKER 
GO
ALTER DATABASE [DatabaseGameBai] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [DatabaseGameBai] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [DatabaseGameBai] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [DatabaseGameBai] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [DatabaseGameBai] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [DatabaseGameBai] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [DatabaseGameBai] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [DatabaseGameBai] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [DatabaseGameBai] SET  MULTI_USER 
GO
ALTER DATABASE [DatabaseGameBai] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [DatabaseGameBai] SET DB_CHAINING OFF 
GO
ALTER DATABASE [DatabaseGameBai] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [DatabaseGameBai] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [DatabaseGameBai] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [DatabaseGameBai] SET QUERY_STORE = OFF
GO
USE [DatabaseGameBai]
GO
/****** Object:  Table [dbo].[player]    Script Date: 7/6/2019 9:33:31 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[player](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[username] [varchar](30) NOT NULL,
	[password] [varchar](50) NOT NULL,
	[money] [money] NOT NULL,
	[nickname] [nvarchar](30) NOT NULL,
	[email] [varchar](256) NOT NULL,
 CONSTRAINT [PK_player] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[player_status]    Script Date: 7/6/2019 9:33:31 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[player_status](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[player_id] [int] NOT NULL,
	[status] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[player_rank]    Script Date: 7/6/2019 9:33:31 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[player_rank](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[player_id] [int] NOT NULL,
	[lose] [int] NULL,
	[win] [int] NULL,
	[hightscore] [money] NULL,
 CONSTRAINT [PK_player_rank] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[infoplayer]    Script Date: 7/6/2019 9:33:31 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[infoplayer] AS SELECT
dbo.player.id,
dbo.player.nickname,
dbo.player.money,
dbo.player_status.status,
dbo.player_rank.lose,
dbo.player_rank.win,
dbo.player_rank.hightscore

FROM
dbo.player
INNER JOIN dbo.player_status ON dbo.player_status.player_id = dbo.player.id
INNER JOIN dbo.player_rank ON dbo.player_rank.player_id = dbo.player.id
GO
/****** Object:  Table [dbo].[history_login]    Script Date: 7/6/2019 9:33:31 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[history_login](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[player_id] [int] NOT NULL,
	[login_time] [date] NULL,
	[logout_time] [date] NULL,
 CONSTRAINT [PK_history_login] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[player_chatlist]    Script Date: 7/6/2019 9:33:31 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[player_chatlist](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[player_id_send] [int] NOT NULL,
	[player_id_receive] [int] NOT NULL,
	[isseen] [bit] NOT NULL,
 CONSTRAINT [PK_player_chatlist] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[player_listfriend]    Script Date: 7/6/2019 9:33:31 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[player_listfriend](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[player_id] [int] NOT NULL,
	[friend_id] [int] NOT NULL,
	[isban] [bit] NOT NULL,
 CONSTRAINT [PK_player_listfriend] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[player_message]    Script Date: 7/6/2019 9:33:31 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[player_message](
	[id_chatlist] [int] NOT NULL,
	[message] [text] NOT NULL,
	[send_date] [date] NOT NULL,
	[isdelete] [bit] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[room_list]    Script Date: 7/6/2019 9:33:31 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[room_list](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[owner_id] [int] NOT NULL,
	[limit_player] [int] NOT NULL,
	[password] [nchar](10) NULL,
	[current_player] [int] NULL,
 CONSTRAINT [PK_room_list] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[room_listplayer]    Script Date: 7/6/2019 9:33:31 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[room_listplayer](
	[room_id] [int] NOT NULL,
	[player_id] [int] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[player] ADD  CONSTRAINT [DF_player_money]  DEFAULT ((0)) FOR [money]
GO
ALTER TABLE [dbo].[player_chatlist] ADD  CONSTRAINT [DF_player_chatlist_isseen]  DEFAULT ((0)) FOR [isseen]
GO
ALTER TABLE [dbo].[player_listfriend] ADD  CONSTRAINT [DF_player_listfriend_isban]  DEFAULT ((0)) FOR [isban]
GO
ALTER TABLE [dbo].[player_message] ADD  CONSTRAINT [DF_player_message_isdelete]  DEFAULT ((0)) FOR [isdelete]
GO
ALTER TABLE [dbo].[player_rank] ADD  CONSTRAINT [DF_player_rank_lose]  DEFAULT ((0)) FOR [lose]
GO
ALTER TABLE [dbo].[player_rank] ADD  CONSTRAINT [DF_player_rank_win]  DEFAULT ((0)) FOR [win]
GO
ALTER TABLE [dbo].[player_rank] ADD  CONSTRAINT [DF_player_rank_hightscore]  DEFAULT ((0)) FOR [hightscore]
GO
ALTER TABLE [dbo].[room_list] ADD  CONSTRAINT [DF_room_list_current_player]  DEFAULT ((0)) FOR [current_player]
GO
ALTER TABLE [dbo].[player_chatlist]  WITH CHECK ADD  CONSTRAINT [FK_player_id_receive] FOREIGN KEY([player_id_receive])
REFERENCES [dbo].[player] ([id])
GO
ALTER TABLE [dbo].[player_chatlist] CHECK CONSTRAINT [FK_player_id_receive]
GO
ALTER TABLE [dbo].[player_chatlist]  WITH CHECK ADD  CONSTRAINT [FK_player_id_send] FOREIGN KEY([player_id_send])
REFERENCES [dbo].[player] ([id])
GO
ALTER TABLE [dbo].[player_chatlist] CHECK CONSTRAINT [FK_player_id_send]
GO
ALTER TABLE [dbo].[player_listfriend]  WITH CHECK ADD  CONSTRAINT [FK_player_listfriend_player] FOREIGN KEY([friend_id])
REFERENCES [dbo].[player] ([id])
GO
ALTER TABLE [dbo].[player_listfriend] CHECK CONSTRAINT [FK_player_listfriend_player]
GO
ALTER TABLE [dbo].[player_listfriend]  WITH CHECK ADD  CONSTRAINT [FK_player_listfriend_player_listfriend] FOREIGN KEY([player_id])
REFERENCES [dbo].[player] ([id])
GO
ALTER TABLE [dbo].[player_listfriend] CHECK CONSTRAINT [FK_player_listfriend_player_listfriend]
GO
ALTER TABLE [dbo].[player_message]  WITH CHECK ADD  CONSTRAINT [FK_player_message_player_chatlist] FOREIGN KEY([id_chatlist])
REFERENCES [dbo].[player_chatlist] ([id])
GO
ALTER TABLE [dbo].[player_message] CHECK CONSTRAINT [FK_player_message_player_chatlist]
GO
ALTER TABLE [dbo].[player_rank]  WITH CHECK ADD  CONSTRAINT [FK_player_rank_player] FOREIGN KEY([player_id])
REFERENCES [dbo].[player] ([id])
GO
ALTER TABLE [dbo].[player_rank] CHECK CONSTRAINT [FK_player_rank_player]
GO
ALTER TABLE [dbo].[player_status]  WITH CHECK ADD  CONSTRAINT [FK_player_status_player] FOREIGN KEY([player_id])
REFERENCES [dbo].[player] ([id])
GO
ALTER TABLE [dbo].[player_status] CHECK CONSTRAINT [FK_player_status_player]
GO
ALTER TABLE [dbo].[room_listplayer]  WITH CHECK ADD  CONSTRAINT [FK_room_listplayer_room_list] FOREIGN KEY([room_id])
REFERENCES [dbo].[room_list] ([id])
GO
ALTER TABLE [dbo].[room_listplayer] CHECK CONSTRAINT [FK_room_listplayer_room_list]
GO
/****** Object:  StoredProcedure [dbo].[congTien]    Script Date: 7/6/2019 9:33:31 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[congTien] @amount money, @id int
as
Update DatabaseGameBai.dbo.users set money = money+ @amount where DatabaseGameBai.dbo.users.Id = @id
GO
USE [master]
GO
ALTER DATABASE [DatabaseGameBai] SET  READ_WRITE 
GO
