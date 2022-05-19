CREATE TABLE [dbo].[History](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Operation] [nvarchar](50) NOT NULL,	
	[EntityName] [nvarchar](50) NOT NULL,	
	[Message] [nvarchar](max) NOT NULL,	
	[TimeStamp] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_History] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[History] ADD  DEFAULT (getdate()) FOR [TimeStamp]