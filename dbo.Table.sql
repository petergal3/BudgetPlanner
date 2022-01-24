CREATE TABLE [dbo].[saving_goal]
(
	
	[id] INT IDENTITY (0, 1) NOT NULL, 
	[goal] INT NOT NULL , 
    [date] DATE NOT NULL, 
    
    CONSTRAINT [PK_Table] PRIMARY KEY ([id])
)
