ALTER PROCEDURE [dbo].[RegisterMeal_CancelByDate]
    @SysId INT,
    @OnDate DATE,
    @MealId INT,
    @ModifiedBy INT,
    @ModifiedDate DATETIME = NULL,
    @ErrCode INT = 0 OUTPUT,
    @ErrMsg NVARCHAR(200) = '' OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

        IF @ModifiedDate IS NULL
            SET @ModifiedDate = GETDATE();

        DECLARE @RestrictCancelDay INT;
        SET @RestrictCancelDay = [dbo].[func_GetRestrictCancelVegMeal]();

        DECLARE @LogTraderOldData NVARCHAR(MAX) = (
            SELECT *
            FROM EmpRegisterFood
            WHERE SysId = @SysId AND MealID = @MealId AND WorkDate = @OnDate
            FOR JSON AUTO
        );



        UPDATE er
        SET er.IsVegetarianManual = NULL,
            er.IsVegetarian = NULL,
            er.ModifiedBy = @ModifiedBy,
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
            @UserId = @ModifiedBy,
            @SPName = 'RegisterMeal_CancelByDate',
            @LogDate = @ModifiedDate,
            @ObjectId = @SysId,
            @Data = @LogTraderData,
            @OldData = @LogTraderOldData;

        -- Set success
        SET @ErrCode = 0;
        SET @ErrMsg = N'Meal registration cancelled successfully.';

END