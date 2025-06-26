ALTER PROCEDURE [dbo].[DBA_Task_RegisterMealOnDate]
	@SysID VARCHAR(MAX) = NULL
	,@FromDate DATE = NULL
	,@IsTest BIT = 0
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @FromDateRun DATE = DATEADD(DD, -1, @FromDate)
		,@ToDateRun DATE = DATEADD(DD, 1, @FromDate)
		,@TmpDate DATE = DATEADD(DD, -1, @FromDate)
	
	SELECT @SysID = ISNULL(@SysID, '')

	IF @SysID = ''
	BEGIN
		SELECT @SysID = COALESCE(@SysID + ',', '') + convert(NVARCHAR(50), ISNULL(tbl.SysID, ''))
		FROM dbo.Employee tbl WITH (NOLOCK)
		WHERE ISNULL(isDeleted, 0) = 0
	END

	IF OBJECT_ID('tempdb..#tmpSys') IS NOT NULL
		DROP TABLE #tmpSys

	CREATE TABLE #tmpSys ([SysId] INT)

	INSERT INTO #tmpSys ([SysId])
	SELECT val
	FROM fnc_strSplit(@SysID, ',')
	
	DELETE #tmpSys WHERE [SysId] <1 OR [SysId] IS NULL
	
	IF OBJECT_ID('tempdb..#tmpDate') IS NOT NULL
	BEGIN
		DROP TABLE #tmpDate
	END
	CREATE TABLE #tmpDate (WorkDate DATE)
	WHILE (@TmpDate <= @ToDateRun)
	BEGIN
		INSERT INTO #tmpDate (WorkDate) VALUES (@TmpDate)
		SET @TmpDate = DATEADD(DAY, 1, @TmpDate)
	END

	IF OBJECT_ID('tempdb..#EmpRegisterFood') IS NOT NULL
		DROP TABLE #EmpRegisterFood

	CREATE TABLE #EmpRegisterFood (
		[SysId] [int] NULL
		,[WorkDate] [datetime] NULL
		,[MealID] [smallint] NULL
		,[IsVegetarian] [bit] NULL
		,[IsVegetarianOriginal] [bit] NULL
		,[IsOverride] [bit] NULL
		,[IsVegetarianManual] [bit] NULL
		)

	INSERT INTO #EmpRegisterFood(
		[SysId]
		,[WorkDate]
		,[MealID]
		,[IsVegetarian] 
		,[IsVegetarianOriginal] 
		,[IsOverride] 
		,[IsVegetarianManual]
	)
	SELECT a.SysId
		,b.WorkDate
		,me.MealID
		,CASE WHEN ISNULL(reg.IsOverride, 0) = 1 
		THEN 
			-- manual
			CASE WHEN ((me.MealID = 1 AND (ISNULL(es1.Meal_B_1, 0) = 1 OR ISNULL(es.Meal_B, 0) = 1))
					OR (me.MealID = 2 AND (ISNULL(es1.Meal_L_1, 0) = 1 OR ISNULL(es.Meal_L, 0) = 1))
					OR (me.MealID = 3 AND (ISNULL(es1.Meal_D_1, 0) = 1 OR ISNULL(es.Meal_D, 0) = 1))
					OR (me.MealID = 4 AND (ISNULL(es.Meal_S, 0) = 1 OR ISNULL(es2.Meal_S_0, 0) = 1)))
				THEN
					reg.IsVegetarianManual
				ELSE NULL
			END
		ELSE
			-- not yet manual
			CASE WHEN ((me.MealID = 1 AND (ISNULL(es1.Meal_B_1, 0) = 1 OR ISNULL(es.Meal_B, 0) = 1))
					OR (me.MealID = 2 AND (ISNULL(es1.Meal_L_1, 0) = 1 OR ISNULL(es.Meal_L, 0) = 1))
					OR (me.MealID = 3 AND (ISNULL(es1.Meal_D_1, 0) = 1 OR ISNULL(es.Meal_D, 0) = 1))
					OR (me.MealID = 4 AND (ISNULL(es.Meal_S, 0) = 1 OR ISNULL(es2.Meal_S_0, 0) = 1)))
				THEN
					0
				ELSE NULL
				END
		END AS New_IsVegetarian
		,CASE WHEN ((me.MealID = 1 AND (ISNULL(es1.Meal_B_1, 0) = 1 OR ISNULL(es.Meal_B, 0) = 1))
					OR (me.MealID = 2 AND (ISNULL(es1.Meal_L_1, 0) = 1 OR ISNULL(es.Meal_L, 0) = 1))
					OR (me.MealID = 3 AND (ISNULL(es1.Meal_D_1, 0) = 1 OR ISNULL(es.Meal_D, 0) = 1))
					OR (me.MealID = 4 AND (ISNULL(es.Meal_S, 0) = 1 OR ISNULL(es2.Meal_S_0, 0) = 1)))
				THEN
					ISNULL(reg.IsVegetarianManual, 0)
				ELSE NULL
				END 
		AS New_IsVegetarianOriginal
		,ISNULL(reg.IsOverride, 0) AS New_IsOverride
		,IsVegetarianManual AS New_IsVegetarianManual
	FROM #tmpSys AS a
	INNER JOIN dbo.Employee AS e ON a.SysId = e.SysID AND e.isDeleted = 0 AND e.Enabled = 1
	INNER JOIN #tmpDate AS b ON 1=1
	INNER JOIN dbo.Meal AS me WITH (NOLOCK) ON 1=1
	LEFT JOIN 
	(SELECT WorkDate, EmpId, CompanyID, (CASE WHEN LeaveDay = 1 THEN NULL ELSE ShiftId END) AS ShiftId FROM
		dbo.EmpCalendar WITH (NOLOCK) 
		) 
	
	AS cl ON e.EmpId = cl.EmpId AND e.CompanyID = cl.CompanyID AND b.WorkDate = cl.WorkDate
	LEFT JOIN dbo.EmpShift AS es WITH (NOLOCK) ON cl.ShiftId = es.ShiftID AND cl.CompanyID = es.CompanyID
	LEFT JOIN dbo.EmpCalendar AS pre WITH (NOLOCK) ON e.EmpId = pre.EmpId AND e.CompanyID = pre.CompanyID AND DATEADD(DD, -1, b.WorkDate) = pre.WorkDate
	LEFT JOIN dbo.EmpShift AS es1 WITH (NOLOCK) ON pre.ShiftId = es1.ShiftID AND pre.CompanyID = es1.CompanyID
	LEFT JOIN dbo.EmpCalendar AS nex WITH (NOLOCK) ON e.EmpId = nex.EmpId AND e.CompanyID = nex.CompanyID AND DATEADD(DD, 1, b.WorkDate) = nex.WorkDate
	LEFT JOIN dbo.EmpShift AS es2 WITH (NOLOCK) ON nex.ShiftId = es2.ShiftID AND nex.CompanyID = es2.CompanyID
	LEFT JOIN dbo.EmpRegisterFood AS reg WITH (NOLOCK) ON a.SysId = reg.SysId AND b.WorkDate = reg.WorkDate AND me.MealID = reg.MealID
	ORDER BY a.SysId, b.WorkDate

	select * from #EmpRegisterFood;

	UPDATE reg 
	SET reg.IsVegetarian = t_reg.IsVegetarian
		,reg.IsVegetarianOriginal = t_reg.IsVegetarianOriginal
		,reg.IsOverride = t_reg.IsOverride
		,reg.ModifiedDate = GETDATE()
	FROM dbo.EmpRegisterFood AS reg WITH (ROWLOCK, UPDLOCK)
	INNER JOIN #EmpRegisterFood AS t_reg ON reg.SysId = t_reg.SysId
		AND reg.WorkDate = t_reg.WorkDate
		AND reg.MealID = t_reg.MealID
	INNER JOIN dbo.Employee AS emp ON reg.SysId = emp.SysID
	WHERE (emp.CompanyID IN (1, 4) AND GETDATE() <= DATEADD(HOUR, 14, CAST(DATEADD(DAY, -1, reg.WorkDate) AS DATETIME)))
		OR emp.CompanyID NOT IN (1, 4)

	INSERT INTO dbo.EmpRegisterFood(
		SysId
		,WorkDate
		,MealID
		,IsVegetarian
		,IsVegetarianOriginal
		,IsOverride
		,ModifiedDate
	)
	SELECT SysId
		,WorkDate
		,MealID
		,IsVegetarian
		,IsVegetarianOriginal
		,IsOverride
		,GETDATE()
	FROM #EmpRegisterFood AS t_reg
	WHERE NOT EXISTS (
		SELECT 1 
		FROM dbo.EmpRegisterFood AS reg WITH (NOLOCK)
		WHERE reg.SysId = t_reg.SysId 
			AND reg.WorkDate = t_reg.WorkDate
			AND reg.MealID = t_reg.MealID)

	IF OBJECT_ID('tempdb..#EmpRegisterFood') IS NOT NULL
		DROP TABLE #EmpRegisterFood
	IF OBJECT_ID('tempdb..#tmpDate') IS NOT NULL
		DROP TABLE #tmpDate
	IF OBJECT_ID('tempdb..#tmpSys') IS NOT NULL
		DROP TABLE #tmpSys 
END

