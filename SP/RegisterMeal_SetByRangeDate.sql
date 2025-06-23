USE [FBM]
GO
/****** Object:  StoredProcedure [dbo].[RegisterMeal_SetByRangeDate]    Script Date: 6/23/2025 10:10:24 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

 ALTER PROCEDURE [dbo].[RegisterMeal_SetByRangeDate] @SysId VARCHAR(500)
	,@Meal VARCHAR(max) /*<ROOT><Meal MealID="1" IsVeg="1/><Meal MealID="2" IsVeg="0/><Meal MealID="3" IsVeg="1/><Meal MealID="4" IsVeg="0"/></ROOT>'*/
	,@OnDate VARCHAR(max) /*2014-9-01,2014-9-8,2014-9-15,2014-9-22,2014-9-29*/
	,@ModifiedBy INT
	,@ModifiedDate DATETIME = NULL
	,@ErrCode INT = 0 OUTPUT
	,@ErrMsg NVARCHAR(200) = '' OUTPUT
AS
/*
Created: Tom.Vo@2014-8-25
Task: Set employee calendar by month
DB: [FBM]
Original: 

Revisions:
Param's Explanation:

Exec SP:
	DECLARE
		@ErrCode int = 0  
		,@ErrMsg nvarchar(200) = ''

	EXEC  [dbo].[RegisterMeal_SetByRangeDate]  
		@SysId =25
		,@Meal ='<ROOT><Meal MealID="1" IsVeg="1"/><Meal MealID="2" IsVeg="0"/><Meal MealID="3" IsVeg="1"/><Meal MealID="4" IsVeg="0"/></ROOT>'
		,@OnDate ='2014-9-01,2014-9-8,2014-9-15,2014-9-22,2014-9-29'	
		,@ModifiedBy = 100
		,@ErrCode    = @ErrCode  OUTPUT
		,@ErrMsg = @ErrMsg OUTPUT

	SELECT @ErrCode, @ErrMsg  
*/
BEGIN
	SET NOCOUNT ON

	-- For write log
	DECLARE @StartRun DATETIME
		,@EndRun DATETIME
		,@Duration INT
		,@LogTraderData NVARCHAR(MAX)
		,@LogTraderOldData NVARCHAR(MAX)
		,@Xml XML

	SET @StartRun = GETDATE()	

	DECLARE @RestrictDay INT,@RestrictCancelDay INT

	SET @RestrictDay = dbo.[func_GetRestrictRegisterVegMeal]()

	SET @RestrictCancelDay = [dbo].[func_GetRestrictCancelVegMeal]()

	-- write log before update				
	SET @xml = (
			SELECT @SysId AS '@ei'
				,@OnDate AS '@fd'
				,@Meal AS '@mi'
			FOR XML PATH('z')
				,ROOT('r')
			)
	SET @LogTraderData = REPLACE(CONVERT(VARCHAR(MAX), @xml), '" />', '"/"')

	-- end write log
	IF OBJECT_ID('tempdb..#Meal') IS NOT NULL
	BEGIN
		DROP TABLE #Meal
	END

	CREATE TABLE #Meal (
		MealID INT
		,IsVeg BIT NULL
		)

	DECLARE @DocHandler INT

	BEGIN TRY
		EXEC sp_xml_preparedocument @DocHandler OUTPUT
			,@Meal

		INSERT INTO #Meal (
			MealID
			,IsVeg
			)
		SELECT MealID
			,IsVeg
		FROM OPENXML(@DocHandler, '/ROOT/Meal', 1) WITH (MealID INT, IsVeg BIT) AS Data

		EXEC sp_xml_removedocument @DocHandler
	END TRY

	BEGIN CATCH
		SELECT @ErrCode = 100
			,@ErrMsg = N'Paramter invalid.'

		RETURN
	END CATCH;

	IF OBJECT_ID('tempdb..#Member') IS NOT NULL
	BEGIN
		DROP TABLE #Member
	END

	CREATE TABLE #Member ([SysId] INT)

	INSERT INTO #Member ([SysId])
	SELECT val
	FROM fnc_strSplit(@SysId, ',')

	IF OBJECT_ID('tempdb..#RangeDate') IS NOT NULL
	BEGIN
		DROP TABLE #RangeDate
	END

	CREATE TABLE #RangeDate (WorkDate DATETIME)

	INSERT INTO #RangeDate (WorkDate)
	SELECT DISTINCT val WorkDate
	FROM fnc_strSplit(@OnDate, ',')

	DELETE tmp
	FROM #RangeDate tmp
	WHERE tmp.WorkDate < CONVERT(NVARCHAR(100), GETDATE(), 111)

	DECLARE @LimitMealID INT

	IF EXISTS (
			SELECT TOP 1 1
			FROM #RangeDate tmp
			WHERE tmp.WorkDate = CONVERT(NVARCHAR(100), GETDATE(), 111)
			)
	BEGIN
		SELECT TOP 1 @LimitMealID = MealID
		FROM dbo.Meal WITH (NOLOCK)
		WHERE DATEPART(HH, RegisterTime) < DATEPART(HH, GETDATE())
			OR (
				DATEPART(HH, RegisterTime) = DATEPART(HH, GETDATE())
				AND DATEPART(MINUTE, RegisterTime) < DATEPART(MINUTE, GETDATE())
				)
		ORDER BY MealID DESC
	END

	UPDATE ec
	SET   ec.IsVegetarian = CASE 
			WHEN ec.WorkDate < DATEADD(DAY, @RestrictDay, CONVERT(NVARCHAR(100), GETDATE(), 111))
				AND (
					@LimitMealID >= ec.MealID
					OR @RestrictDay > 0
					)
				AND tmpMeal.IsVeg = 1 THEN ec.IsVegetarian
			WHEN ec.WorkDate < DATEADD(DAY, @RestrictCancelDay, CONVERT(NVARCHAR(100), GETDATE(), 111))
				AND ec.IsVegetarian = 1 THEN ec.IsVegetarian
			WHEN ec.WorkDate = CONVERT(NVARCHAR(100), GETDATE(), 111)
				AND @LimitMealID >= ec.MealID THEN ec.IsVegetarian
			WHEN ((select count(*)
					from dbo.EmpCalendar ecc 
					INNER JOIN dbo.Employee ee
					ON ecc.EmpId = ee.EmpId AND ecc.CompanyID = ee.CompanyID
					WHERE ee.SysID = @SysId AND ecc.WorkDate = tmpDate.WorkDate) = 0) THEN NULL 
			ELSE tmpMeal.IsVeg
			END
		,ec.IsVegetarianManual = CASE 
			WHEN ec.WorkDate < DATEADD(DAY, @RestrictDay, CONVERT(NVARCHAR(100), GETDATE(), 111))
				AND (
					@LimitMealID >= ec.MealID
					OR @RestrictDay > 0
					)
				AND tmpMeal.IsVeg = 1 THEN ec.IsVegetarian
			WHEN ec.WorkDate < DATEADD(DAY, @RestrictCancelDay, CONVERT(NVARCHAR(100), GETDATE(), 111))
				AND ec.IsVegetarian = 1 THEN ec.IsVegetarian
			WHEN ec.WorkDate = CONVERT(NVARCHAR(100), GETDATE(), 111)
				AND @LimitMealID >= ec.MealID THEN ec.IsVegetarian
			ELSE tmpMeal.IsVeg
			END
		,ec.ModifiedBy = @ModifiedBy
		,ec.ModifiedDate = GETDATE()
		,ec.[IsOverride] = 1
	FROM dbo.EmpRegisterFood ec WITH (ROWLOCK, UPDLOCK)
		INNER JOIN #RangeDate tmpDate
		ON tmpDate.WorkDate = ec.WorkDate
		INNER JOIN #Member tmpSys
		ON ec.SysId = tmpSys.[SysId]
		INNER JOIN #Meal tmpMeal
		ON ec.MealID = tmpMeal.MealID


	IF EXISTS(SELECT TOP 1 1 
		FROM dbo.EmpRegisterFood ec 
		INNER JOIN #RangeDate tmp
		ON tmp.WorkDate = ec.WorkDate
		INNER JOIN #Meal tmpMeal
			ON  1 = 1
		WHERE ec.WorkDate <   DATEADD(DAY, @RestrictCancelDay, CONVERT(NVARCHAR(100), GETDATE(), 111))
			AND  ec.SysId = @SysID
			AND  ec.IsVegetarian = 1
			AND  tmpMeal.IsVeg = 0 
		)
	BEGIN
		DELETE #RangeDate
		WHERE WorkDate  <   DATEADD(DAY, @RestrictCancelDay, CONVERT(NVARCHAR(100), GETDATE(), 111))
	END


	INSERT INTO [dbo].EmpRegisterFood (
		[WorkDate]
		,SysId
		,MealID
		,IsVegetarian
		,IsVegetarianManual
		,ModifiedBy
		,ModifiedDate
		,[IsOverride]
		)
	SELECT tmp.WorkDate
		,tmp.[SysID]
		,tmp.MealID
		,CASE 
			WHEN tmp.WorkDate < DATEADD(DAY, @RestrictDay, CONVERT(NVARCHAR(100), GETDATE(), 111))
				AND (
					@LimitMealID >= ec.MealID
					OR @RestrictDay > 0
					)
				AND tmp.IsVeg = 1 THEN ec.IsVegetarian
			ELSE tmp.IsVeg
			END IsVegetarian
		,CASE 
			WHEN tmp.WorkDate < DATEADD(DAY, @RestrictDay, CONVERT(NVARCHAR(100), GETDATE(), 111))
				AND (
					@LimitMealID >= ec.MealID
					OR @RestrictDay > 0
					)
				AND tmp.IsVeg = 1 THEN ec.IsVegetarian
			ELSE tmp.IsVeg
			END IsVegetarian
		,@ModifiedBy ModifiedBy
		,GETDATE() ModifiedDate
		,1 [IsOverride]
	FROM (
		SELECT *
		FROM #RangeDate tmpDate
			,#Member tmpSys
			,#Meal tmpMeal
		) tmp
		LEFT JOIN dbo.EmpRegisterFood ec
		ON tmp.WorkDate = ec.WorkDate
			AND ec.SysId = tmp.[SysId]
			AND ec.MealID = tmp.MealID
	WHERE ec.WorkDate IS NULL
		AND (
			(
				tmp.WorkDate = CONVERT(NVARCHAR(100), GETDATE(), 111)
				AND @LimitMealID < tmp.MealID
				)
			OR tmp.WorkDate > CONVERT(NVARCHAR(100), GETDATE(), 111)
			)
	--AND tmp.IsVeg IS NOT NULL
	--AND DATEPART(DW, ec.WorkDate) NOT IN (1,7)
	ORDER BY tmp.WorkDate
		,tmp.MealID

	IF @@ERROR <> 0
	BEGIN
		SELECT @ErrCode = 101
			,@ErrMsg = N'Meal Registration failed.'

		RETURN
	END
	ELSE
	BEGIN
		SELECT @ErrCode = 0
			,@ErrMsg = ''
	END

	IF OBJECT_ID('tempdb..#RangeDate') IS NOT NULL
	BEGIN
		DROP TABLE #RangeDate
	END

	IF OBJECT_ID('tempdb..#Member') IS NOT NULL
	BEGIN
		DROP TABLE #Member
	END

	SET @EndRun = GETDATE()
	SET @Duration = DATEDIFF(MILLISECOND, @StartRun, @EndRun)

	EXEC dbo.SYS_WriteLog_RunSp @UserId = @ModifiedBy
		,@SPName = 'RegisterMeal_SetByRangeDate'
		,@LogDate = @StartRun
		,@Duration = @Duration
		,@ObjectId = @SysId
		,@Data = @LogTraderData
		,@OldData = @LogTraderOldData
END


