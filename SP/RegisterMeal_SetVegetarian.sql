ALTER PROCEDURE [dbo].[RegisterMeal_SetVegetarian] @EmpId INT
	,@CompanyId INT
	,@OnDate DATE
	,@ErrCode INT = 0 OUTPUT
	,@ErrMsg NVARCHAR(200) = '' OUTPUT
AS
/*
Created: Quinn.Vo@20230423
Task: Set Vegetarian by date
DB: [FBM]
Original: 

Revisions:
Param's Explanation:
*/
BEGIN
	SET NOCOUNT ON

	DECLARE @SysId INT

	SELECT @SysId = SysID
	FROM dbo.Employee WITH (NOLOCK)
	WHERE EmpId = @EmpId
		AND CompanyID = @CompanyId

	-- For write log
	DECLARE @StartRun DATETIME
		,@EndRun DATETIME
		,@Duration INT
		,@LogTraderData NVARCHAR(MAX)
		,@Xml XML

	SET @StartRun = GETDATE()

	-- write log before update				
	SET @xml = (
			SELECT @SysId AS '@sy'
				,@OnDate AS '@on'
			FOR XML PATH('z')
				,ROOT('r')
			)
	SET @LogTraderData = REPLACE(CONVERT(VARCHAR(MAX), @xml), '" />', '"/"')
	-- end write log
	

	IF (@StartRun < @OnDate)
	BEGIN
		IF NOT EXISTS (SELECT 1 FROM dbo.VegetarianManagement WITH (NOLOCK) WHERE WorkDate = @OnDate)
		BEGIN
			SET @ErrCode = 2
			SET @ErrMsg = 'Đăng ký thất bại. Xin vui lòng liên hệ Admin.'
			RETURN
		END

		-- Kiểm tra đã đăng ký cơm chưa
        IF NOT EXISTS (
            SELECT 1 
            FROM dbo.EmpRegisterFood WITH (NOLOCK)
            WHERE SysId = @SysId AND WorkDate = @OnDate AND IsVegetarian IS NOT NULL
        )
        BEGIN
            SET @ErrCode = 3
            SET @ErrMsg = N'Bạn chưa đăng ký cơm ngày này. Vui lòng lên trang web F&B để đăng ký.'
            RETURN
        END

		UPDATE dbo.EmpRegisterFood WITH(ROWLOCK,UPDLOCK)
		SET IsVegetarian = 1, IsVegetarianManual = 1, ModifiedBy = @SysId 
			,IsOverride = 1
			,ModifiedDate = GETDATE()
		WHERE SysId = @SysId 
			AND WorkDate = @OnDate
			AND IsVegetarian IS NOT NULL

		SET @EndRun = GETDATE()
		SET @Duration = DATEDIFF(MILLISECOND, @StartRun, @EndRun)

		EXEC dbo.SYS_WriteLog_RunSp @UserId = @SysId
			,@SPName = 'RegisterMeal_SetVegetarian'
			,@LogDate = @StartRun
			,@Duration = @Duration
			,@ObjectId = @SysId
			,@Data = @LogTraderData
			,@OldData = ''

			SET @ErrCode = 0
	END
	ELSE
	BEGIN
		SET @ErrCode = 1
		SET @ErrMsg = 'Vui lòng đăng ký trước 1 ngày để Bếp chuẩn bị.'
	END
	
END