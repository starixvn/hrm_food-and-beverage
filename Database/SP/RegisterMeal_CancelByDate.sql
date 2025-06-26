
CREATE OR ALTER PROCEDURE [dbo].[RegisterMeal_CancelByDate]
    @SysId INT,
    @OnDate DATE,
    @MealId INT,
    @ErrCode INT = 0 OUTPUT,
    @ErrMsg NVARCHAR(200) = '' OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

	DECLARE @ModifiedDate DATETIME;
	SET @ModifiedDate = GETDATE()	


    -- Check if trying to cancel after cutoff time (14:00 of previous day)
    DECLARE @CutoffTime DATETIME = DATEADD(HOUR, 14, CAST(DATEADD(DAY, -1, @OnDate) AS DATETIME));
    IF GETDATE() > @CutoffTime
    BEGIN
        SET @ErrCode = 1;
        SET @ErrMsg = N'Không thể hủy đăng ký suất ăn sau 14:00.';
        RETURN;
    END

    DECLARE @LogTraderOldData NVARCHAR(MAX) = (
        SELECT *
        FROM EmpRegisterFood
        WHERE SysId = @SysId AND MealID = @MealId AND WorkDate = @OnDate
        FOR JSON AUTO
    );

    UPDATE er
    SET er.IsVegetarianManual = NULL,
        er.IsVegetarian = NULL,
		er.IsOverride = 1,
        er.ModifiedBy = @SysId,
        er.ModifiedDate = @ModifiedDate
    FROM EmpRegisterFood er
    WHERE er.SysId = @SysId
      AND er.MealID = @MealId
      AND er.WorkDate = @OnDate;

    -- Logging
    DECLARE @LogTraderData NVARCHAR(MAX) = (
        SELECT *
        FROM EmpRegisterFood
        WHERE SysId = @SysId AND MealID = @MealId AND WorkDate = @OnDate
        FOR JSON AUTO
    );

    EXEC dbo.SYS_WriteLog_RunSp
        @UserId = @SysId,
        @SPName = 'RegisterMeal_CancelByDate',
        @LogDate = @ModifiedDate,
        @ObjectId = @SysId,
        @Data = @LogTraderData,
        @OldData = @LogTraderOldData;

    -- Set success
    SET @ErrCode = 0;
    SET @ErrMsg = N'Hủy đăng ký suất ăn thành công.';
END