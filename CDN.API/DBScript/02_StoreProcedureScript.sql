-- UspUserSel
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.UspUserSel') AND type IN ( N'P', N'PC',N'X',N'RF')) 
BEGIN
	DROP PROCEDURE [dbo].[UspUserSel]
END
GO

CREATE PROCEDURE [dbo].[UspUserSel]    
(    
  @PageNumber INT,    
  @PageSize INT    
)    
AS    
SET NOCOUNT ON    
    
 -- Calculate the offset and fetch next rows    
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;    
    
    SELECT COUNT(1) AS TotalRecordCount     
    FROM tblUser WITH(NOLOCK)    
    
    -- Get records for the requested page    
    SELECT *    
    FROM tblUser WITH(NOLOCK)    
    ORDER BY Id -- Add your ordering here if needed    
    OFFSET @Offset ROWS    
    FETCH NEXT @PageSize ROWS ONLY;    
    
SET NOCOUNT OFF  
GO

-- UspUserMaintain
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.UspUserMaintain') AND type IN ( N'P', N'PC',N'X',N'RF')) 
BEGIN
	DROP PROCEDURE [dbo].[UspUserMaintain]
END
GO

CREATE PROCEDURE [dbo].[UspUserMaintain]
(
	@Id [uniqueidentifier],
	@Username [nvarchar](50),
	@Mail [nvarchar](20),
	@PhoneNumber [nvarchar](30),
	@Skillsets [nvarchar](MAX),
	@Hobby [nvarchar](20),
	@Action [int]
)
AS
SET NOCOUNT ON

	/* Action
	   -- 0 = Update
	   -- 1 - Delete
	   -- 2 = Insert
	*/

	IF @Action = 0
	BEGIN
		UPDATE [dbo].[tblUser]
		SET UserName = @Username,
			Mail = @Mail,
			PhoneNumber = @PhoneNumber,
			Skillsets = @Skillsets,
			Hobby = @Hobby,
			UpdateDateTime = GETDATE()
		WHERE Id = @Id
	END
	ELSE IF @Action = 1
	BEGIN
		DELETE FROM [dbo].[tblUser]
		WHERE Id = @Id
	END
	ELSE IF @Action = 2
	BEGIN
		INSERT INTO [dbo].[tblUser] ([Id], [Username], [Mail], [PhoneNumber], [Skillsets], [Hobby], [InserDateTime], [UpdateDateTime])
		VALUES (@Id, @Username, @Mail, @PhoneNumber, @Skillsets, @Hobby, GETDATE(), NULL)
	END

SET NOCOUNT OFF
GO

