CREATE TABLE [dbo].Payments(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeId] [int] NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,	
	[DateTime] [datetime2](7) NOT NULL	
 CONSTRAINT [PK_Payments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Payments] ADD  DEFAULT (getdate()) FOR [DateTime]
GO

ALTER TABLE [dbo].Payments  WITH CHECK ADD  CONSTRAINT [FK_Payments_Employees_EmployeeId] FOREIGN KEY([EmployeeId])
REFERENCES [dbo].[Employees] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].Payments CHECK CONSTRAINT [FK_Payments_Employees_EmployeeId]
GO