CREATE TABLE [dbo].[DatabaseVersions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Version] [int] NOT NULL,
	[DateTime] [datetime2](7) NOT NULL
 CONSTRAINT [PK_DatabaseVersions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)
) ON [PRIMARY] 

ALTER TABLE [dbo].[DatabaseVersions] ADD  DEFAULT (getdate()) FOR [DateTime]



