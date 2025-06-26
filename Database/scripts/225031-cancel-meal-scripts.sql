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

GO

ALTER PROCEDURE [dbo].[MealOrder_Edit_RequestByWeb]
	@OrderID INT
	,@Remark nvarchar(500)	=''
	,@RemarkVNM nvarchar(500)=''
	,@LocationID SmallINT=NULL
	,@phone nvarchar(500)=''
	
	,@OrderTime DateTime=NULL
	,@ListDetail varchar(MAX)=NULL
	,@IsSaveOrder BIT=0
	,@TakeAway BIT=0

	,@ErrCode INT = 0 OUTPUT
	,@ErrMsg nvarchar(2000) = '' OUTPUT
	
	,@ModifiedDate datetime=NULL
	,@ModifiedBy INT
AS
/*
Created: Tom.Vo@2014-8-25
Task: Get Menu On Date By EmpID
DB: FBM
Original: 

Revisions:    
Param's Explanation:
Execute SP:
DECLARE @p2 INT
DECLARE @p3 INT
DECLARE @p8 INT

DECLARE @p9 NVARCHAR(2000)
				EXEC [MealOrder_Edit_RequestByWeb] 
		@LocationID = 31
		,@OrderID = @p3 OUTPUT
		,@Phone = N'123456'
		,@Remark = N''
		,@IsSaveOrder = 1
		,@@ModifiedDate = '2014-10-09 14:47:32.110'
		,@ErrCode = @p8 OUTPUT
		,@ErrMsg = @p9 OUTPUT
		,@@ModifiedBy = 1019
		,@ListDetail = '<ROOT>
		<User SysId=''1019'' MenuID=''17'' IsMainMenu=''0'' IsOrder=''True''></User>
		<User SysId=''884''  MenuID=''17'' IsMainMenu=''0'' IIsOrder=''False''></User>
		<User SysId=''895''  MenuID=''17'' IsMainMenu=''0''  IsOrder=''True''></User>
		<User SysId=''918''  MenuID=''17'' IsMainMenu=''0''  IsOrder=''True''></User>
		<User SysId=''998''  MenuID=''17'' IsMainMenu=''0''  IsOrder=''True''></User></ROOT>'

	SELECT @p2,@p3,@p8,@p9
*/
BEGIN
	SET NOCOUNT ON	 
	-- For write log
	DECLARE 
		@StartRun					DATETIME,
		@EndRun						DATETIME,
		@Duration					INT,
		@LogTraderData				NVARCHAR(MAX),	
		@LogTraderOldData			NVARCHAR(MAX),	
		@Xml						XML		 

	SET @StartRun = GETDATE()
	
	DECLARE @MealID INT	
	
	IF @LocationID IS NULL 
	BEGIN	
		SELECT @ErrCode = 105
		
		SELECT @ErrMsg = N'Location invalid.'
		
		RETURN
	END
	
	IF NOT EXISTS(SELECT TOP 1 1 FROM dbo.Location l WITH(NOLOCK) WHERE LocationID = @LocationID AND l.IsActive = 1)
	BEGIN
		SELECT @ErrCode = 106
		
		SELECT @ErrMsg = N'This Delivery Area does not exist. Please reload this page.'
		
		RETURN
	END

	IF OBJECT_ID('tempdb..#EmpRegister') IS NOT NULL
		BEGIN
			DROP TABLE #EmpRegister
		END

	CREATE TABLE #EmpRegister (
		SysId INT
		,QtyVeg INT
		,QtyNonVeg INT
		,IsOrder BIT
		,IsMainMenu BIT
		,MenuID SMALLINT
		,RetrictOrder BIT DEFAULT(0)
		,MaxOrderPerDay TINYINT DEFAULT(0)
		)
 
	DECLARE @DocHandler INT

	SELECT @ListDetail = ISNULL(@ListDetail,'')

	BEGIN TRY
		EXEC sp_xml_preparedocument @DocHandler OUTPUT
			,@ListDetail

		INSERT INTO #EmpRegister (SysID,IsOrder,IsMainMenu,MenuID)
		SELECT DISTINCT SysId,IsOrder,ISNULL(IsMainMenu,1) IsMainMenu,ISNULL(MenuID,0) MenuID
		FROM OPENXML(@DocHandler, '/ROOT/User', 1) WITH (SysId INT,IsOrder BIT,IsMainMenu BIT,MenuID SMALLINT) AS Data

		EXEC sp_xml_removedocument @DocHandler
	END TRY

	BEGIN CATCH
		SELECT @ErrCode = 100
			,@ErrMsg = N'Paramter invalid.'

		RETURN
	END CATCH;

	UPDATE tmp
	SET tmp.RetrictOrder = ecm.RetrictOrder 
		,tmp.MaxOrderPerDay = CASE 
			WHEN ecm.RetrictOrder = 1 THEN ecm.MaxOrderPerDay
			ELSE 1
			END
	FROM #EmpRegister tmp
		INNER JOIN [dbo].[Employee] ee WITH (NOLOCK)
		ON ee.SysId = tmp.SysId
		INNER JOIN [dbo].[EmpCategoryMeal] ecm WITH (NOLOCK)
		ON ee.EmpCategoryID = ecm.EmpCategoryID
	WHERE tmp.IsOrder = 1

	DECLARE @OnDate DATE
		,@OrderNo INT=NULL
		,@CurrentStatusID INT		
		,@OwerID INT
		,@Member INT
		,@OrderFrom VARCHAR(10)
		,@OrderTime_Old DATETIME

	SELECT TOP 1 @OnDate = ord.OnDate
		,@CurrentStatusID=[CurrentStatusID]
		,@MealID =ord.MealID
		,@OwerID = ord.SysId
		,@OrderFrom = ord.OrderFrom 
		,@OrderTime_Old = ord.OrderTime
	FROM [dbo].[Order] ord WITH(NOLOCK) 
	WHERE ord.OrderID  = @OrderID

	-- Check for users who haven't registered food in EmpRegisterFood
	IF EXISTS (
		SELECT 1 
		FROM #EmpRegister er
		INNER JOIN dbo.Employee e WITH (NOLOCK) ON er.SysId = e.SysID 
		WHERE er.IsOrder = 1
		AND NOT EXISTS (
			SELECT 1 
			FROM dbo.EmpRegisterFood erf WITH (NOLOCK)
			WHERE erf.SysId = er.SysId
			AND erf.WorkDate = @OnDate 
			AND erf.MealID = @MealID
			AND erf.IsVegetarian IS NOT NULL
		)
	)
	BEGIN
		SELECT @ErrCode = 100,
			   @ErrMsg = N'Users have not registered for meals on this date. Please register meals first.'
		RETURN
	END

	IF @CurrentStatusID > 3
	BEGIN
		SELECT @ErrCode = 102
		
		SELECT @ErrMsg = N'Only "NEW" or "RECEIVED" status of an order can be edited.'
		
		RETURN
	END

	IF ((@OrderFrom <> 'W'))
	BEGIN
		SELECT @ErrCode = 103
		
		SELECT @ErrMsg = N'Cannot cancel this order. Please contact the owner of this order.'
		
		RETURN
	END
	
	IF (@OwerID <> @ModifiedBy )
	BEGIN
		IF  (SELECT COUNT(tmp.SysID) FROM #EmpRegister tmp WHERE tmp.IsOrder = 1 AND tmp.[SysId] <> @ModifiedBy) 
			<>  
			(
				SELECT 
				COUNT(DISTINCT Members)  Members
				FROM (
					SELECT ordd.SysId Members
					FROM [dbo].[Order] ord WITH (NOLOCK)
						INNER JOIN [dbo].OrderDetail ordd WITH (NOLOCK)
						ON ord.OrderID = ordd.OrderID
							AND ordd.IsDeleted = 0
							AND ordd.IsCancel = 0
						WHERE ord.OrderID = @OrderID AND ordd.[SysId] <> @ModifiedBy
					UNION ALL
					SELECT ordA.SysId Members
					FROM [dbo].[Order] ord WITH (NOLOCK)
						INNER JOIN [dbo].OrderAlternative ordA WITH (NOLOCK)
						ON ord.OrderID = ordA.OrderID
							AND ordA.IsDeleted = 0
							AND ordA.IsCancel = 0
						WHERE ord.OrderID = @OrderID  AND ordA.[SysId] <> @ModifiedBy
					) AA
			)
		OR EXISTS(
			SELECT TOP 1 1
			FROM [dbo].OrderDetail ordo WITH(NOLOCK)
				INNER JOIN #EmpRegister tmp
				ON tmp.IsOrder = 1
					AND tmp.IsMainMenu = 0
					AND tmp.[SysId] = ordo.[SysId]
					AND ordo.IsCancel = 0
					AND ordo.IsDeleted = 0
			WHERE ordo.OrderID = @OrderID 
				AND ordo.[SysId] <> @ModifiedBy
			)
		OR EXISTS(
		SELECT TOP 1 1
			FROM #EmpRegister tmp
				INNER JOIN [dbo].[OrderAlternative] ordo  WITH(NOLOCK)
				ON tmp.IsOrder = 1
					AND tmp.[SysId] = ordo.[SysId]
					AND ordo.OrderID = @OrderID
					AND tmp.IsMainMenu = 1
					AND ordo.IsCancel = 0
					AND ordo.IsDeleted = 0
				WHERE tmp.[SysId] <> @ModifiedBy					
		)
		BEGIN
			SELECT @ErrCode = 104
		
			SELECT @ErrMsg = N'Cannot cancel this order. Please contact the owner of this order.'
		
			RETURN
		END
	END

IF OBJECT_ID('tempdb..#tmpOrder') IS NOT NULL
	BEGIN
		DROP TABLE #tmpOrder
	END

	CREATE TABLE #tmpOrder (
		SysId INT
		,OrderPerDay TINYINT DEFAULT(0)
		);

	WITH CTE (
		OrderID
		,SysId
		,CurrentStatusID
		,Qty
		)
	AS (
		SELECT ord.OrderID
			,orda.SysId
			,ord.CurrentStatusID
			,orda.Qty
		FROM [dbo].[Order] ord WITH (NOLOCK)
				INNER JOIN [dbo].OrderAlternative orda WITH (NOLOCK)
				ON ord.OrderID = orda.OrderID
				INNER JOIN #EmpRegister tmp WITH (NOLOCK)
				ON orda.SysId = tmp.SysId AND tmp.IsOrder = 1
		WHERE ord.OnDate = @OnDate
			AND (tmp.RetrictOrder = 1 OR
				(tmp.RetrictOrder = 0 AND ord.MealID =@MealID)
					)
			AND ord.OrderID <> @OrderID
			AND ord.CurrentStatusID NOT IN (6, 99, 100)
			AND (
				orda.IsDeleted = 0
				AND orda.IsCancel = 0
				)
		UNION ALL
		SELECT ord.OrderID
			,ordd.SysId
			,ord.CurrentStatusID
			,ordd.QtyNonVeg+ordd.QtyVeg AS Qty
		FROM [dbo].[Order] ord WITH (NOLOCK)
			INNER JOIN [dbo].OrderDetail ordd WITH (NOLOCK)
			ON ord.OrderID = ordd.OrderID
			INNER JOIN #EmpRegister tmp WITH (NOLOCK)
			ON ordd.SysId = tmp.SysId
		WHERE ord.OnDate = @OnDate
			AND (tmp.RetrictOrder = 1 OR
				(tmp.RetrictOrder = 0 AND ord.MealID =@MealID)
					)
			AND ord.OrderID <> @OrderID
			AND ord.CurrentStatusID NOT IN (6, 99, 100)
			AND (
				ordd.IsDeleted = 0
				AND ordd.IsCancel = 0
				)
		)
	INSERT INTO #tmpOrder (
		SysId
		,OrderPerDay
		)
	SELECT SysId
		,SUM(CASE WHEN CTE.CurrentStatusID IN (1,2,3) THEN 100 ELSE CTE.Qty END) OrderPerDay
	FROM CTE
	WHERE OrderID <> @OrderID
	GROUP BY SysId
	
	SELECT TOP 1 @OrderNo = tmpEmp.SysId 
	FROM #EmpRegister tmpEmp
	INNER JOIN #tmpOrder tmpOrd
		ON tmpEmp.SysId = tmpOrd.SysId
	WHERE tmpOrd.OrderPerDay >= tmpEmp.MaxOrderPerDay AND tmpEmp.RetrictOrder = 0 
	
	DECLARE @NickName Nvarchar(500)=''

	IF @OrderNo IS NOT NULL
	BEGIN	
		SELECT @ErrCode = 102
		
		SELECT @NickName = COALESCE(@NickName + '<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;', '') + convert(NVARCHAR(50),ISNULL(ee.NickName,'')) 
		FROM (
			SELECT DISTINCT ee.NickName
			FROM #EmpRegister tmpEmp
				INNER JOIN #tmpOrder tmpOrd
					ON tmpEmp.SysId = tmpOrd.SysId
				INNER JOIN  [dbo].[Employee] ee WITH (NOLOCK)
								ON ee.SysID = tmpEmp.SysID	
			WHERE tmpOrd.OrderPerDay >= tmpEmp.MaxOrderPerDay AND tmpEmp.RetrictOrder = 0 
			) ee

		SELECT @ErrMsg = N'This member has reached his meal limit or included in another order:' + @NickName

		RETURN
	END

	SET @OrderNo = NULL

	SELECT TOP 1 @OrderNo = tmpEmp.SysId 
	FROM #EmpRegister tmpEmp
	INNER JOIN #tmpOrder tmpOrd
		ON tmpEmp.SysId = tmpOrd.SysId
	WHERE tmpOrd.OrderPerDay >= tmpEmp.MaxOrderPerDay AND tmpEmp.RetrictOrder = 1 
	
	IF @OrderNo IS NOT NULL
	BEGIN	
		SELECT @ErrCode = 102
		
		SELECT @NickName = COALESCE(@NickName + '<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;', '') + convert(NVARCHAR(50),ISNULL(ee.NickName,'')) 
		FROM (
			SELECT DISTINCT ee.NickName
			FROM #EmpRegister tmpEmp
				INNER JOIN #tmpOrder tmpOrd
					ON tmpEmp.SysId = tmpOrd.SysId
				INNER JOIN  [dbo].[Employee] ee WITH (NOLOCK)
								ON ee.SysID = tmpEmp.SysID	
			WHERE tmpOrd.OrderPerDay >= tmpEmp.MaxOrderPerDay AND tmpEmp.RetrictOrder = 1 
			) ee

		SELECT @ErrMsg = N'This member has reached his meal limit or included in another order:' + @NickName
		
		RETURN
	END
	
	IF OBJECT_ID('tempdb..#tmpOrder') IS NOT NULL
	BEGIN
		DROP TABLE #tmpOrder
	END 

	IF EXISTS (SELECT TOP 1 1 FROM #EmpRegister tmp)
	BEGIN
		IF (SELECT COUNT(tmp.SysID) FROM #EmpRegister tmp WHERE tmp.IsOrder = 1 ) --AND tmp.IsMainMenu = 1) 
			<>  
			(SELECT COUNT(DISTINCT tmp.SysID)
				FROM 
				#EmpRegister AS tmp  WITH(NOLOCK)
				INNER JOIN [dbo].[Employee] AS ee WITH (NOLOCK) ON tmp.SysId = ee.SysID
				LEFT JOIN [dbo].[EmpRegisterFood] AS reg WITH (NOLOCK) ON ee.SysID = reg.SysId AND reg.WorkDate = @OnDate AND reg.MealID = @MealID
				INNER JOIN [dbo].[HisEmpMenuActive] AS his WITH (NOLOCK)
					ON ee.EmpCategoryID = his.EmpCategoryID
						AND his.MealID = @MealID
						AND ISNULL(reg.IsVegetarian, 0) = his.IsVegetarian
						AND (his.EffectedToDate IS NULL OR his.EffectedToDate >= @OnDate)  AND his.EffectedFromDate <= @OnDate
				INNER JOIN [dbo].[Meal] AS ml WITH (NOLOCK) ON his.MealID = ml.MealID
				INNER JOIN dbo.EmpMenu AS eMenu WITH(NOLOCK)
					ON eMenu.EmpMenuID = his.EmpMenuID
				INNER JOIN [dbo].[FoodMenu] fmn WITH(NOLOCK)
					ON fmn.EmpMenuID = his.EmpMenuID
						AND fmn.OnDate = @OnDate
						AND fmn.IsActive = 1
			WHERE tmp.IsOrder = 1
				)
		BEGIN				
			SELECT @NickName = COALESCE(@NickName + '<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;', '') + convert(NVARCHAR(50),ISNULL(ee.NickName,'')) 
			FROM #EmpRegister tmp
				INNER JOIN [dbo].[Employee] ee WITH (NOLOCK)
							ON ee.SysID = tmp.SysID
				WHERE tmp.IsOrder = 1 AND tmp.IsMainMenu = 1  AND tmp.SysID NOT IN 
						(
							SELECT DISTINCT tmp.SysID
							FROM 
								#EmpRegister AS tmp  WITH(NOLOCK)
								INNER JOIN [dbo].[Employee] AS ee WITH (NOLOCK) ON tmp.SysId = ee.SysID
								LEFT JOIN [dbo].[EmpRegisterFood] AS reg WITH (NOLOCK) ON ee.SysID = reg.SysId AND reg.WorkDate = @OnDate AND reg.MealID = @MealID
								INNER JOIN [dbo].[HisEmpMenuActive] AS his WITH (NOLOCK)
									ON ee.EmpCategoryID = his.EmpCategoryID
										AND his.MealID = @MealID
										AND ISNULL(reg.IsVegetarian, 0) = his.IsVegetarian
										AND (his.EffectedToDate IS NULL OR his.EffectedToDate >= @OnDate)  AND his.EffectedFromDate <= @OnDate
								INNER JOIN [dbo].[Meal] AS ml WITH (NOLOCK) ON his.MealID = ml.MealID
								INNER JOIN dbo.EmpMenu AS eMenu WITH(NOLOCK)
									ON eMenu.EmpMenuID = his.EmpMenuID
								INNER JOIN [dbo].[FoodMenu] fmn WITH(NOLOCK)
									ON fmn.EmpMenuID = his.EmpMenuID
										AND fmn.OnDate = @OnDate
										AND fmn.IsActive = 1
							WHERE tmp.IsOrder = 1
						)  

			SELECT @ErrCode = 104
			SELECT @ErrMsg = N'There is no menu available for:' + @NickName
		
			RETURN
		END
	END

	IF @CurrentStatusID > 1
	BEGIN
		IF EXISTS (SELECT TOP 1 1
				FROM #EmpRegister tmp
				INNER JOIN dbo.[OrderAlternative] ordA WITH(NOLOCK)
					ON ordA.OrderID = @OrderID AND ordA.SysId = tmp.SysId
						AND ordA.AltMenuID <> tmp.MenuID
						AND tmp.IsMainMenu = 0
						AND ordA.IsCancel = 0
						AND ordA.IsDeleted = 0
						AND tmp.IsOrder = 1
				)
			OR EXISTS (SELECT TOP 1 1
				FROM dbo.[OrderAlternative] ordA WITH(NOLOCK)
				LEFT JOIN #EmpRegister tmp
					ON ordA.SysId = tmp.SysId
						AND ((ordA.AltMenuID <> tmp.MenuID AND tmp.IsMainMenu = 0)
							OR tmp.IsMainMenu = 1)
						AND ordA.IsCancel = 0
						AND ordA.IsDeleted = 0
						AND tmp.IsOrder = 1
				 WHERE ordA.OrderID = @OrderID  AND tmp.IsOrder IS NOT NULL
				)
		BEGIN
			SELECT @ErrCode = 105
			SELECT @ErrMsg = N'Only "New" order can be edit menu!' + @NickName
		
			RETURN
		END
	END

	UPDATE tmp
	SET [QtyVeg] = CASE WHEN erf.IsVegetarian = 1 AND tmp.IsMainMenu = 1 THEN 1 ELSE 0 END
		,[QtyNonVeg] = CASE WHEN erf.IsVegetarian = 1 THEN 0 ELSE 1 END
	FROM #EmpRegister tmp
		LEFT JOIN [dbo].[EmpRegisterFood] erf WITH(NOLOCK)
			ON erf.WorkDate = @OnDate
					AND erf.MealID = @MealID
					AND erf.SysId = tmp.SysId
	WHERE  tmp.IsOrder = 1

	IF ISDATE(@OrderTime)=0 OR @OrderTime IS NULL
	BEGIN
		SELECT @OrderTime=ISNULL(@OrderTime,GETDATE())
	END
	
	IF @OrderTime_Old <> @OrderTime
	BEGIN
		IF CONVERT(NVARCHAR(10),@OrderTime,112) < CONVERT(NVARCHAR(10),GETDATE(),112)
		BEGIN
			SELECT @OrderTime = CONVERT(DATETIME,CONVERT(NVARCHAR(10),GETDATE(),101)+' '+ CONVERT(NVARCHAR(20),@OrderTime,114))
		END

		IF DATEPART(HH,@OrderTime) < DATEPART(HH,GETDATE())-1
		BEGIN
			SET @OrderTime=DATEADD(DAY,1,@OrderTime)
		END
	END

	UPDATE [dbo].[Order] WITH(ROWLOCK,UPDLOCK)
     SET  [iRework] = [iRework]+1
           ,[LocationID] = @LocationID
           ,[CurrentStatusID] = CASE WHEN [CurrentStatusID] = 1 THEN 1 WHEN [CurrentStatusID] = 2 THEN 3 WHEN [CurrentStatusID] = 3 THEN 3 END
		   ,MealID = ISNULL(@MealID ,MealID )
           ,[Remark] = @Remark
           ,[phone] = @phone
		   ,ModifiedBy = @ModifiedBy
		   ,ModifiedDate = GETDATE()
		   ,OrderTime=@OrderTime
		   ,TakeAway=@TakeAway
		   ,RemarkVNM=@RemarkVNM
     WHERE OrderID  = @OrderID
	 
	IF @CurrentStatusID = 1 
	BEGIN
		DELETE ordd	
		FROM  [dbo].[OrderDetail] ordd WITH(UPDLOCK,ROWLOCK)
		 LEFT JOIN #EmpRegister tmp
		 ON ordd.SysId = tmp.SysId  AND tmp.IsMainMenu = 1
		 WHERE ordd.[OrderID] = @OrderID AND (tmp.SysId IS NULL OR tmp.IsOrder = 0)
		 
		DELETE ordd	
		FROM  [dbo].OrderAlternative ordd WITH(UPDLOCK,ROWLOCK)
		 LEFT JOIN #EmpRegister tmp
		 ON ordd.SysId = tmp.SysId  AND tmp.IsMainMenu = 0
		 WHERE ordd.[OrderID] = @OrderID AND (tmp.SysId IS NULL OR tmp.IsOrder = 0)
	END

	DELETE ordo
	FROM dbo.OrderOptional ordo WITH(UPDLOCK,ROWLOCK)
		LEFT JOIN #EmpRegister tmp ON ordo.SysId = tmp.SysId
	WHERE  [OrderID] = @OrderID AND tmp.SysId IS NULL
	
	UPDATE ordd
	SET ordd.IsCancel = CASE WHEN ISNULL(tmp.IsOrder,0) = 0 OR tmp.IsMainMenu = 0 THEN 1 ELSE 0 END
		,ordd.ModifiedBy = @ModifiedBy
		,ordd.ModifiedDate = GETDATE()
	FROM  [dbo].[OrderDetail] ordd WITH(UPDLOCK,ROWLOCK)
	 LEFT JOIN #EmpRegister tmp
	 ON ordd.SysId = tmp.SysId 
	 WHERE ordd.[OrderID] = @OrderID

	INSERT INTO [dbo].[OrderDetail]
			([OrderID]
			,[SysId]
			,[MenuID]
			,[QtyVeg]
			,[QtyNonVeg]
			,[Price]
			,[ModifiedBy]
			,[ModifiedDate])

	SELECT DISTINCT
			@OrderID OrderID 
			,tmp.[SysId]
			,fmn.MenuID
			,tmp.QtyVeg
			,tmp.QtyNonVeg
			,fmn.Price
			,@ModifiedBy ModifiedBy
			,GETDATE() ModifiedDate 
	FROM #EmpRegister tmp
		LEFT JOIN [dbo].[OrderDetail] ordd
			ON ordd.[OrderID] = @OrderID AND ordd.SysId = tmp.SysId
		INNER JOIN [dbo].[Employee] AS ee WITH (NOLOCK) ON tmp.SysId = ee.SysID
		LEFT JOIN [dbo].[EmpRegisterFood] AS reg WITH (NOLOCK) ON ee.SysID = reg.SysId AND reg.WorkDate = @OnDate AND reg.MealID = @MealID
		INNER JOIN [dbo].[HisEmpMenuActive] AS his WITH (NOLOCK)
			ON ee.EmpCategoryID = his.EmpCategoryID
				AND his.MealID = @MealID
				AND ISNULL(reg.IsVegetarian, 0) = his.IsVegetarian
				AND (his.EffectedToDate IS NULL OR his.EffectedToDate >= @OnDate)  AND his.EffectedFromDate <= @OnDate
		INNER JOIN [dbo].[Meal] AS ml  WITH (NOLOCK) ON his.MealID = ml.MealID
		INNER JOIN dbo.EmpMenu AS eMenu WITH(NOLOCK)
			ON eMenu.EmpMenuID = his.EmpMenuID
		INNER JOIN [dbo].[FoodMenu] fmn WITH(NOLOCK)
			ON fmn.EmpMenuID = his.EmpMenuID
				AND fmn.OnDate = @OnDate
				AND fmn.IsActive = 1		
	WHERE  tmp.IsOrder = 1  AND tmp.IsMainMenu = 1 AND ordd.SysId IS NULL
	
	UPDATE ordd
	SET ordd.IsCancel = CASE WHEN ISNULL(tmp.IsOrder,0) = 0 OR tmp.IsMainMenu = 1 THEN 1 ELSE 0 END
		,ordd.AltMenuID =  ISNULL(tmp.MenuID,ordd.AltMenuID)
		,ordd.ModifiedBy = @ModifiedBy
		,ordd.ModifiedDate = GETDATE()
	FROM  [dbo].OrderAlternative ordd WITH(UPDLOCK,ROWLOCK)
		 LEFT JOIN #EmpRegister tmp
		 ON ordd.SysId = tmp.SysId 
		 WHERE ordd.[OrderID] = @OrderID

	INSERT INTO [dbo].OrderAlternative (
		[OrderID]
		,[SysId]
		,AltMenuID
		,MenuID
		,[Qty]
		,[Price]
		,[ModifiedBy]
		,[ModifiedDate]
		,IsDeleted
		,IsCancel
		)
	SELECT DISTINCT @OrderID [OrderID]
		,tmp.[SysId]
		,tmp.MenuID AltMenuID
		,fmn.MenuID MenuID
		,1 [Qty]
		,fmn.[Price]
		,@ModifiedBy ModifiedBy
		,GETDATE() ModifiedDate		
		,0 IsDeleted
		,0 IsCancel 
	FROM #EmpRegister tmp  WITH(NOLOCK)
		INNER JOIN [dbo].[Employee] AS ee WITH (NOLOCK) ON tmp.SysId = ee.SysID
		LEFT JOIN [dbo].[EmpRegisterFood] AS reg WITH (NOLOCK) ON ee.SysID = reg.SysId AND reg.WorkDate = @OnDate AND reg.MealID = @MealID
		INNER JOIN [dbo].[HisEmpMenuActive] AS his WITH (NOLOCK)
			ON ee.EmpCategoryID = his.EmpCategoryID
				AND his.MealID = @MealID
				AND ISNULL(reg.IsVegetarian, 0) = his.IsVegetarian
				AND (his.EffectedToDate IS NULL OR his.EffectedToDate >= @OnDate)  AND his.EffectedFromDate <= @OnDate
		INNER JOIN [dbo].[Meal] AS ml WITH (NOLOCK) ON his.MealID = ml.MealID
		INNER JOIN dbo.EmpMenu AS eMenu WITH(NOLOCK)
			ON eMenu.EmpMenuID = his.EmpMenuID
		INNER JOIN [dbo].[FoodMenu] fmn WITH(NOLOCK)
			ON fmn.EmpMenuID = his.EmpMenuID
				AND fmn.OnDate = @OnDate
				AND fmn.IsActive = 1
		LEFT JOIN [dbo].OrderAlternative ordd WITH (NOLOCK)
			ON ordd.[OrderID] = @OrderID AND ordd.SysId = tmp.SysId
	WHERE tmp.IsOrder = 1 
		AND tmp.IsMainMenu = 0 
		AND ordd.AltMenuID IS NULL

	IF @CurrentStatusID = 1 
	BEGIN
		IF NOT EXISTS(SELECT TOP 1 1 FROM  [dbo].[OrderDetail] ordd WITH(NOLOCK) WHERE ordd.[OrderID] = @OrderID AND ordd.IsCancel = 0 AND ordd.IsDeleted = 0 )
			AND NOT EXISTS(SELECT TOP 1 1 FROM  [dbo].OrderAlternative ordd WITH(NOLOCK) WHERE ordd.[OrderID] = @OrderID AND ordd.IsCancel = 0 AND ordd.IsDeleted = 0 )
		BEGIN
			UPDATE [dbo].[Order] WITH(ROWLOCK,UPDLOCK)
			SET [CurrentStatusID] = 99		
				,ModifiedBy = @ModifiedBy
				,ModifiedDate = GETDATE()
			WHERE [OrderID] = @OrderID 
		END
	END

	UPDATE [dbo].[Employee]
	SET Phone = @Phone
	WHERE SysId = @ModifiedBy

	IF @IsSaveOrder = 1 
	BEGIN
		IF EXISTS(SELECT TOP 1 1 FROM [dbo].[EmpLocation]el WITH(NOLOCK) WHERE el.SysId = @ModifiedBy)
			BEGIN
				UPDATE el 
					SET el.LocationID = @LocationID
				FROM [dbo].[EmpLocation] el
				   WHERE el.SysId = @ModifiedBy
			END
		ELSE
			BEGIN
				INSERT INTO [dbo].[EmpLocation] (SysId,LocationID)
				SELECT @ModifiedBy SysId,@LocationID LocationID
			END
	END

 	SELECT @ErrCode = 0
		,@ErrMsg = ''
	
	IF OBJECT_ID('tempdb..#EmpRegister') IS NOT NULL
	BEGIN
		DROP TABLE #EmpRegister
	END
	
	SET @EndRun = GETDATE()
	SET @Duration = DATEDIFF(MILLISECOND, @StartRun, @EndRun) 
	
	EXEC	dbo.SYS_WriteLog_RunSp
			@UserId		= @ModifiedBy,
			@SPName		= 'MealOrder_Edit_RequestByWeb',
			@LogDate	= @StartRun,
			@Duration	= @Duration,
			@ObjectId	= @OrderID,
			@Data		= @LogTraderData,
			@OldData	= @LogTraderOldData
END

GO

ALTER PROCEDURE [dbo].[MealOrder_Insert_RequestByWeb]
	@Remark nvarchar(500)=''
	,@RemarkVNM nvarchar(500)=''
	,@LocationID SmallINT=NULL
	,@Phone nvarchar(500)=''
	,@OrderTime DateTime=NULL
	,@ListDetail varchar(MAX)=NULL
	,@OrderNo INT=NULL OUTPUT
	,@OrderID INT=NULL OUTPUT
	,@TakeAway BIT=0
	,@ErrCode INT = 0 OUTPUT
	,@ErrMsg nvarchar(2000) = '' OUTPUT
	,@IsSaveOrder BIT=0
	,@CreatedDate datetime=NULL
	,@CreatedBy INT
AS
/*
Created: Tom.Vo@2014-8-25
Task: Get Menu On Date By EmpID
DB: FBM
Original: 

Revisions:    
Param's Explanation:

Execute SP:
DECLARE @p2 INT
DECLARE @p3 INT
DECLARE @p8 INT

DECLARE @p9 NVARCHAR(2000)
		EXEC MealOrder_Insert_RequestByWeb_xtest 
		@LocationID = 31
		,@OrderNo = @p2 OUTPUT
		,@OrderID = @p3 OUTPUT
		,@Phone = N'123456'
		,@Remark = N''
		,@IsSaveOrder = 1
		,@CreatedDate = '2014-10-09 14:47:32.110'
		,@ErrCode = @p8 OUTPUT
		,@ErrMsg = @p9 OUTPUT
		,@CreatedBy = 1019
		,@ListDetail = '<ROOT>
		<User SysId=''1019'' MenuID=''17'' IsMainMenu=''0'' IsOrder=''True''></User>
		<User SysId=''884''  MenuID=''17'' IsMainMenu=''0'' IIsOrder=''False''></User>
		<User SysId=''895''  MenuID=''17'' IsMainMenu=''0''  IsOrder=''True''></User>
		<User SysId=''918''  MenuID=''17'' IsMainMenu=''0''  IsOrder=''True''></User>
		<User SysId=''998''  MenuID=''17'' IsMainMenu=''0''  IsOrder=''True''></User></ROOT>'

	SELECT @p2,@p3,@p8,@p9

*/
BEGIN
	SET NOCOUNT ON
	DECLARE 
		@StartRun					DATETIME,
		@EndRun						DATETIME,
		@Duration					INT,
		@LogTraderData				NVARCHAR(MAX),	
		@LogTraderOldData			NVARCHAR(MAX),	
		@Xml						XML		 

	SET @StartRun = GETDATE()
		
	IF @LocationID IS NULL 
	BEGIN	
		SELECT @ErrCode = 105
		
		SELECT @ErrMsg = N'Location invalid.'
		
		RETURN
	END
	
	IF NOT EXISTS(SELECT TOP 1 1 FROM dbo.Location l WITH(NOLOCK) WHERE LocationID = @LocationID AND l.IsActive = 1)
	BEGIN
		SELECT @ErrCode = 106
		
		SELECT @ErrMsg = N'This Delivery Area does not exist. Please reload this page.'
		
		RETURN
	END

	DECLARE @OnDate DATE
		,@MealID INT

	SET @OnDate = dbo.func_GetRegisterOrderDate()

	SELECT @MealID = dbo.func_GetRegisterOrderMeal()

	IF @MealID IS NULL
	BEGIN	
		SELECT @ErrCode = 100
			,@ErrMsg = N'Please order meal on time .'

		RETURN
	END	

	IF OBJECT_ID('tempdb..#EmpRegister') IS NOT NULL
	BEGIN
		DROP TABLE #EmpRegister
	END

	CREATE TABLE #EmpRegister (
		SysId INT
		,QtyVeg INT
		,QtyNonVeg INT
		,IsOrder BIT
		,IsMainMenu BIT
		,MenuID SMALLINT
		,RetrictOrder BIT DEFAULT(0)
		,MaxOrderPerDay TINYINT DEFAULT(0)
		)
 
	DECLARE @DocHandler INT

	SELECT @ListDetail = ISNULL(@ListDetail,'')

	BEGIN TRY
		EXEC sp_xml_preparedocument @DocHandler OUTPUT
			,@ListDetail

		INSERT INTO #EmpRegister (SysID,IsOrder,IsMainMenu,MenuID)
		SELECT DISTINCT SysId,IsOrder,ISNULL(IsMainMenu,1) IsMainMenu,ISNULL(MenuID,0) MenuID
		FROM OPENXML(@DocHandler, '/ROOT/User', 1) WITH (SysId INT,IsOrder BIT,IsMainMenu BIT,MenuID SMALLINT) AS Data

		EXEC sp_xml_removedocument @DocHandler
	END TRY

	BEGIN CATCH
		SELECT @ErrCode = 100
			,@ErrMsg = N'Paramter invalid.'

		RETURN
	END CATCH;


	-- Check for users who haven't registered food in EmpRegisterFood
	IF EXISTS (
		SELECT 1 
		FROM #EmpRegister er
		INNER JOIN dbo.Employee e WITH (NOLOCK) ON er.SysId = e.SysID 
		WHERE er.IsOrder = 1
		AND NOT EXISTS (
			SELECT 1 
			FROM dbo.EmpRegisterFood erf WITH (NOLOCK)
			WHERE erf.SysId = er.SysId
			AND erf.WorkDate = @OnDate 
			AND erf.MealID = @MealID
			AND erf.IsVegetarian IS NOT NULL
		)
	)
	BEGIN
		SELECT @ErrCode = 100,
			   @ErrMsg = N'Some users have not registered for meals on this date. Please register meals first.'
		RETURN
	END

	UPDATE tmp
	SET tmp.RetrictOrder = ecm.RetrictOrder 
		,tmp.MaxOrderPerDay = CASE 
			WHEN ecm.RetrictOrder = 1 THEN ecm.MaxOrderPerDay
			ELSE 1
			END
	FROM #EmpRegister tmp
		INNER JOIN [dbo].[Employee] ee WITH (NOLOCK)
		ON ee.SysId = tmp.SysId
		INNER JOIN [dbo].[EmpCategoryMeal] ecm WITH (NOLOCK)
		ON ee.EmpCategoryID = ecm.EmpCategoryID
	WHERE tmp.IsOrder = 1

	SET @OrderNo = NULL

	IF OBJECT_ID('tempdb..#tmpOrder') IS NOT NULL
	BEGIN
		DROP TABLE #tmpOrder
	END

	CREATE TABLE #tmpOrder (
		SysId INT
		,OrderPerDay TINYINT DEFAULT(0)
		);

	WITH CTE (
		OrderID
		,SysId
		,CurrentStatusID
		,Qty
		)
	AS (
		SELECT ord.OrderID
			,orda.SysId
			,ord.CurrentStatusID
			,orda.Qty
		FROM [dbo].[Order] ord WITH (NOLOCK)
				INNER JOIN [dbo].OrderAlternative orda WITH (NOLOCK)
				ON ord.OrderID = orda.OrderID
				INNER JOIN #EmpRegister tmp WITH (NOLOCK)
				ON orda.SysId = tmp.SysId AND tmp.IsOrder = 1
		WHERE ord.OnDate = @OnDate
			AND (tmp.RetrictOrder = 1 OR
				(tmp.RetrictOrder = 0 AND ord.MealID =@MealID)
					)
			AND ord.CurrentStatusID NOT IN (6, 99, 100)
			AND (
				orda.IsDeleted = 0
				AND orda.IsCancel = 0
				) 
	
		UNION ALL
		SELECT ord.OrderID
			,ordd.SysId
			,ord.CurrentStatusID
			,ordd.QtyNonVeg+ordd.QtyVeg AS Qty
		FROM [dbo].[Order] ord WITH (NOLOCK)
			INNER JOIN [dbo].OrderDetail ordd WITH (NOLOCK)
			ON ord.OrderID = ordd.OrderID
			INNER JOIN #EmpRegister tmp WITH (NOLOCK)
			ON ordd.SysId = tmp.SysId
		WHERE ord.OnDate = @OnDate
			AND (tmp.RetrictOrder = 1 OR
				(tmp.RetrictOrder = 0 AND ord.MealID =@MealID)
					)
			AND ord.CurrentStatusID NOT IN (6, 99, 100)
			AND (
				ordd.IsDeleted = 0
				AND ordd.IsCancel = 0
				)
			AND EXISTS (
				SELECT TOP 1 1
				FROM #EmpRegister tmp
				WHERE ordd.SysId = tmp.SysId
					AND tmp.IsOrder = 1
				)
		)
	INSERT INTO #tmpOrder (
		SysId
		,OrderPerDay
		)
	SELECT SysId
		,SUM(CASE WHEN CTE.CurrentStatusID IN (1,2,3) THEN 100 ELSE CTE.Qty END) OrderPerDay
	FROM CTE
	GROUP BY SysId
	
	SELECT TOP 1 @OrderNo = tmpEmp.SysId 
	FROM #EmpRegister tmpEmp
	INNER JOIN #tmpOrder tmpOrd
		ON tmpEmp.SysId = tmpOrd.SysId
	WHERE tmpOrd.OrderPerDay >= tmpEmp.MaxOrderPerDay AND tmpEmp.RetrictOrder = 0 
	
	DECLARE @NickName Nvarchar(500)=''

	IF @OrderNo IS NOT NULL
	BEGIN	
		SELECT @ErrCode = 102
		
		SELECT @NickName = COALESCE(@NickName + '<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;', '') + convert(NVARCHAR(50),ISNULL(ee.NickName,'')) 
		FROM (
			SELECT DISTINCT ee.NickName
			FROM #EmpRegister tmpEmp
				INNER JOIN #tmpOrder tmpOrd
					ON tmpEmp.SysId = tmpOrd.SysId
				INNER JOIN  [dbo].[Employee] ee WITH (NOLOCK)
								ON ee.SysID = tmpEmp.SysID	
			WHERE tmpOrd.OrderPerDay >= tmpEmp.MaxOrderPerDay AND tmpEmp.RetrictOrder = 0 
			) ee

		SELECT @ErrMsg = N'This member has reached his meal limit or included in another order:' + @NickName

		RETURN
	END

	SET @OrderNo = NULL

	SELECT TOP 1 @OrderNo = tmpEmp.SysId 
	FROM #EmpRegister tmpEmp
	INNER JOIN #tmpOrder tmpOrd
		ON tmpEmp.SysId = tmpOrd.SysId
	WHERE tmpOrd.OrderPerDay >= tmpEmp.MaxOrderPerDay AND tmpEmp.RetrictOrder = 1 
	
	IF @OrderNo IS NOT NULL
	BEGIN	
		SELECT @ErrCode = 103
		
		SELECT @NickName = COALESCE(@NickName + '<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;', '') + convert(NVARCHAR(50),ISNULL(ee.NickName,'')) 
		FROM (
			SELECT DISTINCT ee.NickName
			FROM #EmpRegister tmpEmp
				INNER JOIN #tmpOrder tmpOrd
					ON tmpEmp.SysId = tmpOrd.SysId
				INNER JOIN  [dbo].[Employee] ee WITH (NOLOCK)
								ON ee.SysID = tmpEmp.SysID	
			WHERE tmpOrd.OrderPerDay >= tmpEmp.MaxOrderPerDay AND tmpEmp.RetrictOrder = 1 
			) ee

		SELECT @ErrMsg = N'This member has reached his meal limit or included in another order:' + @NickName
		
		RETURN
	END
	
	IF OBJECT_ID('tempdb..#tmpOrder') IS NOT NULL
	BEGIN
		DROP TABLE #tmpOrder
	END

	IF (SELECT COUNT(tmp.SysID) FROM #EmpRegister tmp WHERE tmp.IsOrder = 1)
			<>  
			(SELECT COUNT(DISTINCT tmp.SysID)
			FROM 
				#EmpRegister AS tmp  WITH(NOLOCK)
				INNER JOIN [dbo].[Employee] AS ee WITH (NOLOCK) ON tmp.SysId = ee.SysID
				LEFT JOIN [dbo].[EmpRegisterFood] AS reg WITH (NOLOCK) ON ee.SysID = reg.SysId AND reg.WorkDate = @OnDate AND reg.MealID = @MealID
				INNER JOIN [dbo].[HisEmpMenuActive] AS his WITH (NOLOCK)
					ON ee.EmpCategoryID = his.EmpCategoryID
						AND his.MealID = @MealID
						AND ISNULL(reg.IsVegetarian, 0) = his.IsVegetarian
						AND (his.EffectedToDate IS NULL OR his.EffectedToDate >= @OnDate)  AND his.EffectedFromDate <= @OnDate
				INNER JOIN [dbo].[Meal] AS ml WITH (NOLOCK) ON his.MealID = ml.MealID
				INNER JOIN dbo.EmpMenu AS eMenu WITH(NOLOCK)
					ON eMenu.EmpMenuID = his.EmpMenuID
				INNER JOIN [dbo].[FoodMenu] fmn WITH(NOLOCK)
					ON fmn.EmpMenuID = his.EmpMenuID
						AND fmn.OnDate = @OnDate
						AND fmn.IsActive = 1
			WHERE tmp.IsOrder = 1)
	BEGIN
			SELECT @NickName = COALESCE(@NickName + '<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;', '') + convert(NVARCHAR(50),ISNULL(ee.NickName,'')) 
			FROM #EmpRegister tmp
				INNER JOIN [dbo].[Employee] ee WITH (NOLOCK)
							ON ee.SysID = tmp.SysID
				WHERE tmp.IsOrder = 1  AND tmp.IsMainMenu = 1 AND tmp.SysID NOT IN 
						(
							SELECT DISTINCT tmp.SysID
							FROM 
								#EmpRegister AS tmp  WITH(NOLOCK)
								INNER JOIN [dbo].[Employee] AS ee WITH (NOLOCK) ON tmp.SysId = ee.SysID
								LEFT JOIN [dbo].[EmpRegisterFood] AS reg WITH (NOLOCK) ON ee.SysID = reg.SysId AND reg.WorkDate = @OnDate AND reg.MealID = @MealID
								INNER JOIN [dbo].[HisEmpMenuActive] AS his WITH (NOLOCK)
									ON ee.EmpCategoryID = his.EmpCategoryID
										AND his.MealID = @MealID
										AND ISNULL(reg.IsVegetarian, 0) = his.IsVegetarian
										AND (his.EffectedToDate IS NULL OR his.EffectedToDate >= @OnDate)  AND his.EffectedFromDate <= @OnDate
								INNER JOIN [dbo].[Meal] AS ml WITH (NOLOCK) ON his.MealID = ml.MealID
								INNER JOIN dbo.EmpMenu AS eMenu WITH(NOLOCK)
									ON eMenu.EmpMenuID = his.EmpMenuID
								INNER JOIN [dbo].[FoodMenu] fmn WITH(NOLOCK)
									ON fmn.EmpMenuID = his.EmpMenuID
										AND fmn.OnDate = @OnDate
										AND fmn.IsActive = 1
							WHERE tmp.IsOrder = 1
						)  
				
			SELECT @ErrCode = 103
			SELECT @ErrMsg = N'There is no menu available for:' + @NickName
		
		RETURN
	END
	
	UPDATE tmp
	SET [QtyVeg] = CASE WHEN erf.IsVegetarian = 1 AND tmp.IsMainMenu = 1 THEN 1 ELSE 0 END
		,[QtyNonVeg] = CASE WHEN erf.IsVegetarian = 1 THEN 0 ELSE 1 END
	FROM #EmpRegister tmp
		LEFT JOIN [dbo].[EmpRegisterFood] erf WITH(NOLOCK)
			ON erf.WorkDate = @OnDate
					AND erf.MealID = @MealID
					AND erf.SysId = tmp.SysId
		WHERE  tmp.IsOrder = 1	
	
	SELECT @OrderNo = MAX([OrderNo])
	FROM dbo.[Order] tbl WITH(NOLOCK)
	WHERE tbl.[OnDate] = @OnDate
	
	SELECT @OrderNo=ISNULL(@OrderNo,0) + 1

	DECLARE @OrderNo_Max NVARCHAR(10) 

	SELECT @OrderNo_Max=[Value]  FROM [dbo].[LookupData] WITH(NOLOCK) WHERE LookupId = 6

	IF ISNUMERIC(@OrderNo_Max)=0 
	BEGIN
		SELECT @OrderNo_Max = 999	
	END

	IF @OrderNo > CONVERT(INT,@OrderNo_Max)
	BEGIN
		SELECT @OrderNo = 1
	END
	
	IF ISDATE(@OrderTime)=0 OR @OrderTime IS NULL
	BEGIN
		SELECT @OrderTime=ISNULL(@OrderTime,GETDATE())
	END
	
	IF CONVERT(NVARCHAR(10),@OrderTime,112) < CONVERT(NVARCHAR(10),GETDATE(),112)
	BEGIN
		SELECT @OrderTime = CONVERT(DATETIME,CONVERT(NVARCHAR(10),GETDATE(),101)+' '+ CONVERT(NVARCHAR(20),@OrderTime,114))
	END	

	IF DATEPART(HH,@OrderTime) < DATEPART(HH,GETDATE())-1
	BEGIN
		SET @OrderTime=DATEADD(DAY,1,@OrderTime)
	END

	IF(EXISTS(SELECT 1 FROM dbo.OrderDetail AS odt WITH (NOLOCK)
		INNER JOIN dbo.[Order] AS od WITH (NOLOCK) ON od.OrderID = odt.OrderID 
			AND ISNULL(odt.IsDeleted, 0) = 0 
			AND od.CurrentStatusID NOT IN (6, 99, 100)
			AND od.OnDate = @OnDate
			AND od.MealID = @MealID
		INNER JOIN #EmpRegister AS emp ON odt.SysId = emp.SysId
		WHERE DATEDIFF(SS, od.OrderTime, @OrderTime) < 5
	))
	BEGIN
		SELECT @ErrCode = 100
			,@ErrMsg = N'Duplicated.'

		RETURN
	END

	INSERT INTO [dbo].[Order]
           ([OnDate]
           ,[MealID]
           ,[OrderNo]
           ,[iRework]
           ,[SysId]
           ,[LocationID]
           ,[CurrentStatusID]
           ,[RegisterDate]
           ,[RegisterBy]
           ,[Price]
           ,[IsSpecial]
           ,[Remark]
           ,[phone]
		   ,OrderFrom
		   ,OrderTime
		   ,TakeAway
		   ,RemarkVNM)
     SELECT TOP 1
           @OnDate OnDate
           ,@MealID MealID
           ,@OrderNo OrderNo
           ,0 iRework
           ,@CreatedBy SysId
           ,@LocationID LocationID
           ,1 CurrentStatusID
           ,GETDATE() RegisterDate
           ,@CreatedBy RegisterBy
		   ,0 [Price]
           ,0 IsSpecial
           ,@Remark Remark
           ,@phone phone
		   ,'W' OrderFrom 
		   ,@OrderTime OrderTime
		   ,@TakeAway TakeAway
		   ,@RemarkVNM RemarkVNM
	SELECT @OrderID = CAST(SCOPE_IDENTITY() AS INT)

	INSERT INTO [dbo].[OrderDetail]
			([OrderID]
			,SysId
			,[MenuID]
			,[QtyVeg]
			,[QtyNonVeg]
			,[Price]
			,[ModifiedBy]
			,[ModifiedDate]					
			,IsDeleted
			,IsCancel)
	SELECT DISTINCT
			@OrderID OrderID 
			,tmp.SysId
			,fmn.MenuID
			,QtyVeg
			,QtyNonVeg
			,fmn.Price Price
			,@CreatedBy ModifiedBy
			,GETDATE() ModifiedDate 					
			,0 IsDeleted
			,0 IsCancel 
	FROM #EmpRegister AS tmp  WITH(NOLOCK)
		INNER JOIN [dbo].[Employee] AS ee WITH (NOLOCK) ON tmp.SysId = ee.SysID
		LEFT JOIN [dbo].[EmpRegisterFood] AS reg WITH (NOLOCK) ON ee.SysID = reg.SysId AND reg.WorkDate = @OnDate AND reg.MealID = @MealID
		INNER JOIN [dbo].[HisEmpMenuActive] AS his WITH (NOLOCK)
			ON ee.EmpCategoryID = his.EmpCategoryID
				AND his.MealID = @MealID
				AND ISNULL(reg.IsVegetarian, 0) = his.IsVegetarian
				AND (his.EffectedToDate IS NULL OR his.EffectedToDate >= @OnDate)  AND his.EffectedFromDate <= @OnDate
		INNER JOIN [dbo].[Meal] AS ml WITH (NOLOCK) ON his.MealID = ml.MealID
		INNER JOIN dbo.EmpMenu AS eMenu WITH(NOLOCK)
			ON eMenu.EmpMenuID = his.EmpMenuID
		INNER JOIN [dbo].[FoodMenu] fmn WITH(NOLOCK)
			ON fmn.EmpMenuID = his.EmpMenuID
				AND fmn.OnDate = @OnDate
				AND fmn.IsActive = 1			
		WHERE  tmp.IsOrder = 1 AND tmp.IsMainMenu = 1

	INSERT INTO [dbo].OrderAlternative (
		[OrderID]
		,[SysId]
		,AltMenuID
		,MenuID
		,[Qty]
		,[Price]
		,[ModifiedBy]
		,[ModifiedDate]
		,IsDeleted
		,IsCancel
		)
	SELECT @OrderID [OrderID]
		,tmp.[SysId]
		,tmp.MenuID AltMenuID
		,fmn.MenuID MenuID
		,1 [Qty]
		,fmn.[Price]
		,@CreatedBy ModifiedBy
		,GETDATE() ModifiedDate		
		,0 IsDeleted
		,0 IsCancel 
	FROM 
		#EmpRegister AS tmp  WITH(NOLOCK)
		INNER JOIN [dbo].[Employee] AS ee WITH (NOLOCK) ON tmp.SysId = ee.SysID
		LEFT JOIN [dbo].[EmpRegisterFood] AS reg WITH (NOLOCK) ON ee.SysID = reg.SysId AND reg.WorkDate = @OnDate AND reg.MealID = @MealID
		INNER JOIN [dbo].[HisEmpMenuActive] AS his WITH (NOLOCK)
			ON ee.EmpCategoryID = his.EmpCategoryID
				AND his.MealID = @MealID
				AND ISNULL(reg.IsVegetarian, 0) = his.IsVegetarian
				AND (his.EffectedToDate IS NULL OR his.EffectedToDate >= @OnDate)  AND his.EffectedFromDate <= @OnDate
		INNER JOIN [dbo].[Meal] AS ml WITH (NOLOCK) ON his.MealID = ml.MealID
		INNER JOIN dbo.EmpMenu AS eMenu WITH(NOLOCK)
			ON eMenu.EmpMenuID = his.EmpMenuID
		INNER JOIN [dbo].[FoodMenu] fmn WITH(NOLOCK)
			ON fmn.EmpMenuID = his.EmpMenuID
				AND fmn.OnDate = @OnDate
				AND fmn.IsActive = 1
	WHERE tmp.IsOrder = 1 AND tmp.IsMainMenu = 0
	
	UPDATE [dbo].[Employee] WITH (ROWLOCK, UPDLOCK)
	SET Phone = @Phone
	WHERE SysId = @CreatedBy

	IF @IsSaveOrder = 1 
	BEGIN
		IF EXISTS(SELECT TOP 1 1 FROM [dbo].[EmpLocation]el WITH(NOLOCK) WHERE el.SysId = @CreatedBy)
			BEGIN
				UPDATE el 
					SET el.LocationID = @LocationID
				FROM [dbo].[EmpLocation] el
				   WHERE el.SysId = @CreatedBy
			END
		ELSE
			BEGIN
				INSERT INTO [dbo].[EmpLocation] (SysId,LocationID)
				SELECT @CreatedBy SysId,@LocationID LocationID
			END
	END
	
 	SELECT @ErrCode = 0
		,@ErrMsg = ''
	
	IF OBJECT_ID('tempdb..#EmpRegister') IS NOT NULL
	BEGIN
		DROP TABLE #EmpRegister
	END
		
	SET @EndRun = GETDATE()
	SET @Duration = DATEDIFF(MILLISECOND, @StartRun, @EndRun) 
	
	EXEC	dbo.SYS_WriteLog_RunSp
			@UserId		= @CreatedBy,
			@SPName		= 'MealOrder_Insert_RequestByWeb',
			@LogDate	= @StartRun,
			@Duration	= @Duration,
			@ObjectId	= @OrderID,
			@Data		= @LogTraderData,
			@OldData	= @LogTraderOldData
END

GO

ALTER PROCEDURE [dbo].[POS_Edit_OrderMealByUser]
	@OrderID INT

	,@ListDetail varchar(MAX)=NULL

	,@ErrCode INT = 0 OUTPUT
	,@ErrMsg nvarchar(2000) = '' OUTPUT
	
	,@ModifiedDate datetime=NULL
	,@ModifiedBy INT
AS
/*
Created: Tom.Vo@2014-8-25
Task: Get Menu On Date By EmpID
DB: FBM
Original: 

Revisions:    
Param's Explanation:
Execute SP:
DECLARE @OrderID INT
DECLARE @ErrCode INT
DECLARE @ErrMsg NVARCHAR(2000)

	EXEC [POS_Edit_OrderMealByUser] 
		,@OrderID = @OrderID
		,@@ModifiedDate = '2014-10-09 14:47:32.110'
		,@ErrCode = @ErrCode OUTPUT
		,@ErrMsg = @ErrMsg OUTPUT
		,@@ModifiedBy = 1019
		,@ListDetail = '<ROOT>
		<User SysId=''1019'' MenuID=''17'' IsMainMenu=''0'' ></User>
		<User SysId=''884''  MenuID=''17'' IsMainMenu=''0'' ></User>
		<User SysId=''895''  MenuID=''17'' IsMainMenu=''0'' ></User>
		<User SysId=''918''  MenuID=''17'' IsMainMenu=''0'' ></User>
		<User SysId=''998''  MenuID=''17'' IsMainMenu=''0'' ></User></ROOT>'

	SELECT @OrderID,@ErrCode,@ErrMsg
*/
BEGIN
	SET NOCOUNT ON	 
	-- For write log
	DECLARE 
		@StartRun					DATETIME,
		@EndRun						DATETIME,
		@Duration					INT,
		@LogTraderData				NVARCHAR(MAX),	
		@LogTraderOldData			NVARCHAR(MAX),	
		@Xml						XML		 

	SET @StartRun = GETDATE()
	
	DECLARE @MealID INT	
		,@Remark nvarchar(500)	=''
		,@LocationID SmallINT=NULL
		,@phone nvarchar(500)=''

	IF OBJECT_ID('tempdb..#EmpRegister') IS NOT NULL
		BEGIN
			DROP TABLE #EmpRegister
		END

	CREATE TABLE #EmpRegister (SysId INT,QtyVeg INT,QtyNonVeg INT,IsMainMenu BIT,MenuID SMALLINT)
 
	DECLARE @DocHandler INT

	SELECT @ListDetail = ISNULL(@ListDetail,'')

	BEGIN TRY
		EXEC sp_xml_preparedocument @DocHandler OUTPUT
			,@ListDetail

		INSERT INTO #EmpRegister (SysID,IsMainMenu,MenuID)
		SELECT DISTINCT SysId,ISNULL(IsMainMenu,1) IsMainMenu,ISNULL(MenuID,0) MenuID
		FROM OPENXML(@DocHandler, '/ROOT/User', 1) WITH (SysId INT,IsMainMenu BIT,MenuID SMALLINT) AS Data

		EXEC sp_xml_removedocument @DocHandler
	END TRY

	BEGIN CATCH
		SELECT @ErrCode = 100
			,@ErrMsg = N'Paramter invalid.'

		RETURN
	END CATCH;

	DECLARE @OnDate DATE
		,@OrderNo INT=NULL
		,@CurrentStatusID INT		
		,@OwerID INT
		,@Member INT
		,@OrderFrom VARCHAR(10)
	
	SELECT TOP 1 @OnDate = ord.OnDate
		,@CurrentStatusID=[CurrentStatusID]
		,@MealID =ord.MealID
		,@OwerID = ord.SysId
		,@OrderFrom = OrderFrom 
	FROM [dbo].[Order] ord WITH(NOLOCK) 
	WHERE ord.OrderID  = @OrderID

	-- Check for users who haven't registered food in EmpRegisterFood
	IF EXISTS (
		SELECT 1 
		FROM #EmpRegister er
		INNER JOIN dbo.Employee e WITH (NOLOCK) ON er.SysId = e.SysID 
		WHERE NOT EXISTS (
			SELECT 1 
			FROM dbo.EmpRegisterFood erf WITH (NOLOCK)
			WHERE erf.SysId = er.SysId
			AND erf.WorkDate = @OnDate 
			AND erf.MealID = @MealID
			AND erf.IsVegetarian IS NOT NULL
		)
	)
	BEGIN
		SELECT @ErrCode = 100,
			   @ErrMsg = N'Some users have not registered for meals on this date. Please register meals first.'
		RETURN
	END

	IF @CurrentStatusID > 3
	BEGIN
		SELECT @ErrCode = 102
		
		SELECT @ErrMsg = N'Only "NEW" or "RECEIVED" status of an order can be edited.'
		
		RETURN
	END

	IF (@OwerID <> @ModifiedBy OR (@OrderFrom <> 'W'))
	BEGIN
		SELECT @ErrCode = 103
		
		SELECT @ErrMsg = N'Cannot cancel this order. Please contact the owner of this order.'
		
		RETURN
	END

	SELECT TOP 1 @OrderNo = ord.OrderID,@Member = ordd.SysID   FROM [dbo].[Order] ord WITH(NOLOCK)
		INNER JOIN [dbo].OrderDetail ordd WITH(NOLOCK)
			ON ord.OrderID = ordd.OrderID
		WHERE ord.OrderID  <> @OrderID
			AND ord.OnDate = @OnDate
			AND ord.MealID = @MealID
			AND ord.CurrentStatusID < 99
			AND ordd.IsCancel = 0
			AND ordd.IsDeleted = 0
			AND EXISTS(SELECT TOP 1 1 FROM #EmpRegister tmp WHERE ordd.SysID = tmp.SysID)

	DECLARE @NickName Nvarchar(500)=''

	IF @OrderNo IS NOT NULL
	BEGIN	
		SELECT @ErrCode = 103
		
		SELECT @NickName = COALESCE(@NickName + '<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;11', '') + convert(NVARCHAR(50),ISNULL(ee.NickName,'')) 
		FROM (
			SELECT DISTINCT ee.NickName
			FROM [dbo].[Order] ord WITH(NOLOCK)
			INNER JOIN [dbo].OrderDetail ordd WITH(NOLOCK)
				ON ord.OrderID = ordd.OrderID
			INNER JOIN  [dbo].[Employee] ee WITH (NOLOCK)
							ON ee.SysID = ordd.SysID	
			WHERE ord.OrderID  <> @OrderID
				AND ord.OnDate = @OnDate
				AND ord.MealID = @MealID
				AND ord.CurrentStatusID < 99
				AND ordd.IsCancel = 0
				AND ordd.IsDeleted = 0
				AND EXISTS(SELECT TOP 1 1 FROM #EmpRegister tmp WHERE ordd.SysID = tmp.SysID)
			) ee

		SELECT @ErrMsg = N'The selected member is already included in another order.' + @NickName

		RETURN
	END						

	SELECT TOP 1 @OrderNo = ord.OrderID 
		FROM [dbo].[Order] ord WITH(NOLOCK)
			INNER JOIN [dbo].OrderAlternative ordd WITH(NOLOCK)
				ON ord.OrderID = ordd.OrderID
			WHERE ord.OrderID  <> @OrderID
				AND ord.OnDate = @OnDate 
				AND ord.MealID = @MealID
				AND ord.CurrentStatusID < 99
				AND ordd.IsDeleted = 0 AND ordd.IsCancel = 0
				AND EXISTS(SELECT TOP 1 1 FROM #EmpRegister tmp WHERE ordd.SysId = tmp.SysId)
	
	IF @OrderNo IS NOT NULL
	BEGIN	
		SELECT @ErrCode = 106
		
		SELECT @NickName = COALESCE(@NickName + '<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;', '') + convert(NVARCHAR(50),ISNULL(ee.NickName,'')) 
		FROM (
			SELECT DISTINCT ee.NickName
			FROM [dbo].[Order] ord WITH(NOLOCK)
			INNER JOIN [dbo].OrderAlternative ordd WITH(NOLOCK)
				ON ord.OrderID = ordd.OrderID
			INNER JOIN  [dbo].[Employee] ee WITH (NOLOCK)
							ON ee.SysID = ordd.SysID	
			WHERE ord.OrderID  <> @OrderID
				AND ord.OnDate = @OnDate
				AND ord.MealID = @MealID
				AND ord.CurrentStatusID < 99
				AND ordd.IsCancel = 0
				AND ordd.IsDeleted = 0
				AND EXISTS(SELECT TOP 1 1 FROM #EmpRegister tmp WHERE ordd.SysID = tmp.SysID)
			) ee

		SELECT @ErrMsg = N'The selected member is already included in another order.' + @NickName

		RETURN
	END
			
	IF EXISTS (SELECT TOP 1 1 FROM #EmpRegister tmp)
	BEGIN
		IF (SELECT COUNT(tmp.SysID) FROM #EmpRegister tmp WHERE tmp.IsMainMenu = 1) 
			<>  
			(SELECT COUNT(tmp.SysID)
				FROM #EmpRegister tmp
					INNER JOIN [dbo].[Employee] ee WITH (NOLOCK)
						ON ee.SysID = tmp.SysID	
					INNER JOIN [dbo].[EmpMenuSub] emus WITH(NOLOCK)
						ON ee.EmpCategoryID = emus.EmpCategoryID	
					INNER JOIN [dbo].[EmpMeal] ems WITH(NOLOCK)
						ON ems.EmpMenuID = emus.EmpMenuID
						AND ems.MealID = @MealID
						AND ems.IsActive = 1
					INNER JOIN [dbo].[FoodMenu] fm WITH(NOLOCK)
						ON ems.EmpMenuID = fm.EmpMenuID
							AND fm.OnDate = @OnDate
							AND fm.IsActive = 1
					INNER JOIN dbo.EmpMenu eMu WITH(NOLOCK)
						ON ems.EmpMenuID = eMu.EmpMenuID
							AND eMu.IsActive = 1
					LEFT JOIN [dbo].[EmpRegisterFood] erf WITH(NOLOCK)
						ON erf.WorkDate = @OnDate
							AND erf.MealID = @MealID
							AND erf.SysID = tmp.SysID
					WHERE tmp.IsMainMenu = 1 AND ISNULL(erf.IsVegetarian,0) = eMu.IsVegetarian
				)
		BEGIN				
			SELECT @NickName = COALESCE(@NickName + '<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;', '') + convert(NVARCHAR(50),ISNULL(ee.NickName,'')) 
			FROM #EmpRegister tmp
				INNER JOIN [dbo].[Employee] ee WITH (NOLOCK)
							ON ee.SysID = tmp.SysID
				WHERE tmp.IsMainMenu = 1  AND tmp.SysID NOT IN 
						(
							SELECT DISTINCT tmp.SysID
							FROM #EmpRegister tmp							
								INNER JOIN [dbo].[Employee] ee WITH (NOLOCK)
									ON ee.SysID = tmp.SysID
								INNER JOIN [dbo].[EmpMenuSub] emus WITH(NOLOCK)
									ON ee.EmpCategoryID = emus.EmpCategoryID	
								INNER JOIN [dbo].[EmpMeal] ems WITH(NOLOCK)
									ON ems.EmpMenuID = emus.EmpMenuID
									AND ems.MealID = @MealID
									AND ems.IsActive = 1
								INNER JOIN [dbo].[FoodMenu] fm WITH(NOLOCK)
									ON ems.EmpMenuID = fm.EmpMenuID
										AND fm.OnDate = @OnDate
										AND fm.IsActive = 1
								INNER JOIN dbo.EmpMenu eMu WITH(NOLOCK)
									ON ems.EmpMenuID = eMu.EmpMenuID
										AND eMu.IsActive = 1
								LEFT JOIN [dbo].[EmpRegisterFood] erf WITH(NOLOCK)
									ON erf.WorkDate = @OnDate
										AND erf.MealID = @MealID
										AND erf.SysID = tmp.SysID
								WHERE tmp.IsMainMenu = 1 AND ISNULL(erf.IsVegetarian,0) = eMu.IsVegetarian
						)  

			SELECT @ErrCode = 104
			SELECT @ErrMsg = N'No menu available!' + @NickName
		
			RETURN
		END
	END

	IF @CurrentStatusID > 1
	BEGIN
		IF EXISTS (SELECT TOP 1 1
				FROM #EmpRegister tmp
				INNER JOIN dbo.[OrderAlternative] ordA WITH(NOLOCK)
					ON ordA.OrderID = @OrderID AND ordA.SysId = tmp.SysId
						AND ordA.AltMenuID <> tmp.MenuID
						AND tmp.IsMainMenu = 0
						AND ordA.IsCancel = 0
						AND ordA.IsDeleted = 0
						
				)
			OR EXISTS (SELECT TOP 1 1
				FROM dbo.[OrderAlternative] ordA WITH(NOLOCK)
				LEFT JOIN #EmpRegister tmp
					ON ordA.SysId = tmp.SysId
						AND ((ordA.AltMenuID <> tmp.MenuID AND tmp.IsMainMenu = 0)
							OR tmp.IsMainMenu = 1)
						AND ordA.IsCancel = 0
						AND ordA.IsDeleted = 0
						
				 WHERE ordA.OrderID = @OrderID
				)
		BEGIN
			SELECT @ErrCode = 105
			SELECT @ErrMsg = N'Only "New" order can be edit menu!' + @NickName
		
			RETURN
		END
	END

	UPDATE tmp
	SET [QtyVeg] = CASE WHEN erf.IsVegetarian = 1 AND tmp.IsMainMenu = 1 THEN 1 ELSE 0 END
		,[QtyNonVeg] = CASE WHEN erf.IsVegetarian = 1 THEN 0 ELSE 1 END
	FROM #EmpRegister tmp
		LEFT JOIN [dbo].[EmpRegisterFood] erf WITH(NOLOCK)
			ON erf.WorkDate = @OnDate
					AND erf.MealID = @MealID
					AND erf.SysId = tmp.SysId

	UPDATE [dbo].[Order] WITH(ROWLOCK,UPDLOCK)
     SET  [iRework] = [iRework]+1
           ,[LocationID] = @LocationID
           ,[CurrentStatusID] = CASE WHEN [CurrentStatusID] = 1 THEN 1 WHEN [CurrentStatusID] = 2 THEN 3 WHEN [CurrentStatusID] = 3 THEN 3 END
		   ,MealID = ISNULL(@MealID ,MealID )
           ,[Remark] = @Remark
           ,[phone] = @phone
		   ,ModifiedBy = @ModifiedBy
		   ,ModifiedDate = GETDATE()
     WHERE OrderID  = @OrderID
	 
	IF @CurrentStatusID = 1 
	BEGIN
		DELETE ordd	
		FROM  [dbo].[OrderDetail] ordd WITH(UPDLOCK,ROWLOCK)
		 LEFT JOIN #EmpRegister tmp
		 ON ordd.SysId = tmp.SysId  AND tmp.IsMainMenu = 1
		 WHERE ordd.[OrderID] = @OrderID AND (tmp.SysId IS NULL)
		 
		DELETE ordd	
		FROM  [dbo].OrderAlternative ordd WITH(UPDLOCK,ROWLOCK)
		 LEFT JOIN #EmpRegister tmp
		 ON ordd.SysId = tmp.SysId  AND tmp.IsMainMenu = 0
		 WHERE ordd.[OrderID] = @OrderID AND (tmp.SysId IS NULL)
	END

	DELETE ordo
	FROM dbo.OrderOptional ordo WITH(UPDLOCK,ROWLOCK)
		LEFT JOIN #EmpRegister tmp ON ordo.SysId = tmp.SysId
	WHERE  [OrderID] = @OrderID AND tmp.SysId IS NULL
	
	UPDATE ordd
	SET ordd.IsCancel = CASE WHEN ISNULL(tmp.SysId,0) = 0 OR tmp.IsMainMenu = 0 THEN 1 ELSE 0 END
		,ordd.ModifiedBy = @ModifiedBy
		,ordd.ModifiedDate = GETDATE()
	FROM  [dbo].[OrderDetail] ordd WITH(UPDLOCK,ROWLOCK)
	 LEFT JOIN #EmpRegister tmp
	 ON ordd.SysId = tmp.SysId 
	 WHERE ordd.[OrderID] = @OrderID

	INSERT INTO [dbo].[OrderDetail]
			([OrderID]
			,[SysId]
			,[MenuID]
			,[QtyVeg]
			,[QtyNonVeg]
			,[Price]
			,[ModifiedBy]
			,[ModifiedDate])

	SELECT DISTINCT
			@OrderID OrderID 
			,tmp.[SysId]
			,fm.MenuID
			,tmp.QtyVeg
			,tmp.QtyNonVeg
			,0 Price
			,@ModifiedBy ModifiedBy
			,GETDATE() ModifiedDate 
	FROM #EmpRegister tmp
		LEFT JOIN [dbo].[OrderDetail] ordd
			ON ordd.[OrderID] = @OrderID AND ordd.SysId = tmp.SysId
		INNER JOIN [dbo].[Employee] ee WITH (NOLOCK)
			ON ee.[SysId] = tmp.[SysId]	
		INNER JOIN [dbo].[EmpMenuSub] emus WITH(NOLOCK)
			ON ee.EmpCategoryID = emus.EmpCategoryID	
		INNER JOIN [dbo].[EmpMeal] ems WITH(NOLOCK)
			ON ems.EmpMenuID = emus.EmpMenuID
				AND ems.MealID = @MealID
				AND ems.IsActive = 1
		INNER JOIN [dbo].[FoodMenu] fm WITH(NOLOCK)
			ON ems.EmpMenuID = fm.EmpMenuID
				AND fm.OnDate = @OnDate				
				AND fm.IsActive = 1
		INNER JOIN dbo.EmpMenu eMu WITH(NOLOCK)
			ON ems.EmpMenuID = eMu.EmpMenuID
				AND eMu.IsActive = 1
		LEFT JOIN [dbo].[EmpRegisterFood] erf WITH(NOLOCK)
			ON erf.WorkDate = @OnDate
				AND erf.MealID = @MealID
				AND erf.[SysId] = tmp.[SysId]				
	WHERE tmp.IsMainMenu = 1 AND ordd.SysId IS NULL AND ISNULL(erf.IsVegetarian,0) = eMu.IsVegetarian  
	
	UPDATE ordd
	SET ordd.IsCancel = CASE WHEN ISNULL(tmp.SysId,0) = 0 OR tmp.IsMainMenu = 1 THEN 1 ELSE 0 END
		,ordd.AltMenuID =  ISNULL(tmp.MenuID,ordd.AltMenuID)
		,ordd.ModifiedBy = @ModifiedBy
		,ordd.ModifiedDate = GETDATE()
	FROM  [dbo].OrderAlternative ordd WITH(UPDLOCK,ROWLOCK)
		 LEFT JOIN #EmpRegister tmp
		 ON ordd.SysId = tmp.SysId 
		 WHERE ordd.[OrderID] = @OrderID

	INSERT INTO [dbo].OrderAlternative (
		[OrderID]
		,[SysId]
		,AltMenuID
		,[Qty]
		,[Price]
		,[ModifiedBy]
		,[ModifiedDate]
		,IsDeleted
		,IsCancel
		)
	SELECT @OrderID [OrderID]
		,tmp.[SysId]
		,tmp.MenuID AltMenuID
		,1 [Qty]
		,0 [Price]
		,@ModifiedBy ModifiedBy
		,GETDATE() ModifiedDate		
		,0 IsDeleted
		,0 IsCancel 
	FROM #EmpRegister tmp  WITH(NOLOCK)		
		LEFT JOIN [dbo].OrderAlternative ordd
			ON ordd.[OrderID] = @OrderID AND ordd.SysId = tmp.SysId
	WHERE tmp.IsMainMenu = 0 AND ordd.AltMenuID IS NULL

	IF @CurrentStatusID = 1 
	BEGIN
		IF NOT EXISTS(SELECT TOP 1 1 FROM  [dbo].[OrderDetail] ordd WITH(NOLOCK) WHERE ordd.[OrderID] = @OrderID AND ordd.IsCancel = 0 AND ordd.IsDeleted = 0 )
			AND NOT EXISTS(SELECT TOP 1 1 FROM  [dbo].OrderAlternative ordd WITH(NOLOCK) WHERE ordd.[OrderID] = @OrderID AND ordd.IsCancel = 0 AND ordd.IsDeleted = 0 )
		BEGIN
			UPDATE [dbo].[Order] WITH(ROWLOCK,UPDLOCK)
			SET [CurrentStatusID] = 99		
				,ModifiedBy = @ModifiedBy
				,ModifiedDate = GETDATE()
			WHERE [OrderID] = @OrderID 
		END
	END

	UPDATE [dbo].[Employee]
	SET Phone = @Phone
	WHERE SysId = @ModifiedBy

 	SELECT @ErrCode = 0
		,@ErrMsg = ''
	
	IF OBJECT_ID('tempdb..#EmpRegister') IS NOT NULL
	BEGIN
		DROP TABLE #EmpRegister
	END
	
	SET @EndRun = GETDATE()
	SET @Duration = DATEDIFF(MILLISECOND, @StartRun, @EndRun) 
	
	EXEC	dbo.SYS_WriteLog_RunSp
			@UserId		= @ModifiedBy,
			@SPName		= 'POS_Edit_OrderMealByUser',
			@LogDate	= @StartRun,
			@Duration	= @Duration,
			@ObjectId	= @OrderID,
			@Data		= @LogTraderData,
			@OldData	= @LogTraderOldData
END

GO

ALTER PROCEDURE [dbo].[POS_Insert_OrderMealByUser]
	@ListDetail varchar(MAX)=NULL
	,@Foods varchar(MAX)=NULL
	,@OrderNo INT=NULL OUTPUT
	,@OrderID INT=NULL OUTPUT
	,@TakeAway BIT=0

	,@ErrCode INT = 0 OUTPUT
	,@ErrMsg nvarchar(2000) = '' OUTPUT

	,@CreatedDate DATETIME=NULL
	,@CreatedBy INT
AS
/*
Created: Tom.Vo@2014-8-25
Task: Get Menu On Date By EmpID
DB: FBM
Original: 

Revisions:    
Param's Explanation:

Execute SP:
DECLARE @OrderNo INT
DECLARE @OrderID INT
DECLARE @ErrCode INT

DECLARE @ErrMsg NVARCHAR(2000)
		EXEC POS_Insert_OrderMealByUser_xtest 
		
		,@OrderNo = @OrderNo OUTPUT
		,@OrderID = @OrderID OUTPUT
		
		,@CreatedDate = '2014-10-09 14:47:32.110'
		,@ErrCode = @ErrCode OUTPUT
		,@ErrMsg = @ErrMsg OUTPUT
		,@CreatedBy = 1019
		,@ListDetail = '<ROOT>
			<User SysId=''1019'' MenuId=''17'' IsMainMenu=''0'' ></User>
			<User SysId=''884''  MenuId=''17'' IsMainMenu=''0'' ></User>
			<User SysId=''895''  MenuId=''17'' IsMainMenu=''0'' ></User>
			<User SysId=''918''  MenuId=''17'' IsMainMenu=''0'' ></User>
			<User SysId=''998''  MenuId=''17'' IsMainMenu=''0'' ></User></ROOT>'
		,@Foods='<ROOT><User SysId=''225'' FoodId=''1'' Qty=''2''></User><User SysId=''230'' FoodId=''3'' Qty=''2''></User></ROOT>'

	SELECT @OrderNo,@OrderID,@ErrCode,@ErrMsg

*/
BEGIN
	SET NOCOUNT ON
	DECLARE 
		@StartRun					DATETIME,
		@EndRun						DATETIME,
		@Duration					INT,
		@LogTraderData				NVARCHAR(MAX),	
		@LogTraderOldData			NVARCHAR(MAX),	
		@Xml						XML		 

	SET @StartRun = GETDATE()
		
	DECLARE @OnDate DATE
		,@MealID INT
		,@LocationID SmallINT=NULL
		,@Phone nvarchar(500)=''
		,@Remark nvarchar(500)=''

	SET @OnDate = dbo.func_GetRegisterOrderDate()

	SELECT @MealID = dbo.func_GetRegisterOrderMeal_xtest()

	IF @MealID IS NULL
	BEGIN	
		SELECT @ErrCode = 100
			,@ErrMsg = N'Please order meal on time .'

		RETURN
	END
	
	IF OBJECT_ID('tempdb..#EmpRegister') IS NOT NULL
		BEGIN
			DROP TABLE #EmpRegister
		END

	CREATE TABLE #EmpRegister (
		SysId INT
		,QtyVeg INT
		,QtyNonVeg INT
		,IsMainMenu BIT
		,MenuID SMALLINT
		,RetrictOrder BIT DEFAULT(0)
		,MaxOrderPerDay TINYINT DEFAULT(0)
		)
 
	DECLARE @DocHandler INT

	SELECT @ListDetail = ISNULL(@ListDetail,'')

	BEGIN TRY
		EXEC sp_xml_preparedocument @DocHandler OUTPUT
			,@ListDetail

		INSERT INTO #EmpRegister (SysID,IsMainMenu,MenuId)
		SELECT DISTINCT SysId,ISNULL(IsMainMenu,1) IsMainMenu,ISNULL(MenuId,0) MenuId
		FROM OPENXML(@DocHandler, '/ROOT/User', 1) WITH (SysId INT,IsMainMenu BIT,MenuId SMALLINT) AS Data

		EXEC sp_xml_removedocument @DocHandler
	END TRY

	BEGIN CATCH
		SELECT @ErrCode = 100
			,@ErrMsg = N'Paramter invalid.'

		RETURN
	END CATCH;
	
	
	IF OBJECT_ID('tempdb..#EmpFood') IS NOT NULL
		BEGIN
			DROP TABLE #EmpFood
		END

	CREATE TABLE #EmpFood (SysId INT,FoodId INT, Qty NUMERIC(8,2))
		
	SELECT @Foods = ISNULL(@Foods,'')
	IF @Foods <> ''
	BEGIN
		BEGIN TRY
			EXEC sp_xml_preparedocument @DocHandler OUTPUT
				,@Foods

			INSERT INTO #EmpFood (SysId,FoodId,Qty)
			SELECT DISTINCT SysId,FoodId,Qty
			FROM OPENXML(@DocHandler, '/ROOT/User', 1) WITH (SysId INT,FoodId INT,Qty NUMERIC(8,2)) AS Data

			EXEC sp_xml_removedocument @DocHandler
		END TRY

		BEGIN CATCH
			SELECT @ErrCode = 100
				,@ErrMsg = N'Paramter invalid.'

			RETURN
		END CATCH;
	END	

	-- Check for users who haven't registered food in EmpRegisterFood
	IF EXISTS (
		SELECT 1 
		FROM #EmpRegister er
		INNER JOIN dbo.Employee e WITH (NOLOCK) ON er.SysId = e.SysID 
		WHERE NOT EXISTS (
			SELECT 1 
			FROM dbo.EmpRegisterFood erf WITH (NOLOCK)
			WHERE erf.SysId = er.SysId
			AND erf.WorkDate = @OnDate 
			AND erf.MealID = @MealID
			AND erf.IsVegetarian IS NOT NULL
		)
	)
	BEGIN
		SELECT @ErrCode = 100,
			   @ErrMsg = N'Some Users have not registered for meals on this date. Please register meals first.'
		RETURN
	END



	UPDATE tmp
	SET tmp.RetrictOrder = ecm.RetrictOrder 
		,tmp.MaxOrderPerDay = CASE 
			WHEN ecm.RetrictOrder = 1 THEN ecm.MaxOrderPerDay
			ELSE 1
			END
	FROM #EmpRegister tmp
		INNER JOIN [dbo].[Employee] ee WITH (NOLOCK)
		ON ee.SysId = tmp.SysId
		INNER JOIN [dbo].[EmpCategoryMeal] ecm WITH (NOLOCK)
		ON ee.EmpCategoryID = ecm.EmpCategoryID

	IF OBJECT_ID('tempdb..#tmpOrder') IS NOT NULL
	BEGIN
		DROP TABLE #tmpOrder
	END

	CREATE TABLE #tmpOrder (
		SysId INT
		,OrderPerDay TINYINT DEFAULT(0)
		);
	WITH CTE (
			OrderID
			,SysId
			,CurrentStatusID
			,Qty
			)
		AS (
			SELECT ord.OrderID
				,orda.SysId
				,ord.CurrentStatusID
				,orda.Qty
			FROM [dbo].[Order] ord WITH (NOLOCK)
				INNER JOIN [dbo].OrderAlternative orda WITH (NOLOCK)
				ON ord.OrderID = orda.OrderID
				INNER JOIN #EmpRegister tmp WITH (NOLOCK)
				ON orda.SysId = tmp.SysId
			WHERE ord.OnDate = @OnDate
				AND (tmp.RetrictOrder = 1 OR
					(tmp.RetrictOrder = 0 AND ord.MealID =@MealID)
					 )
				AND ord.CurrentStatusID NOT IN (6, 99, 100)
				AND (
					orda.IsDeleted = 0
					AND orda.IsCancel = 0
					)
			UNION ALL
			SELECT ord.OrderID
				,ordd.SysId
				,ord.CurrentStatusID
			,ordd.QtyNonVeg+ordd.QtyVeg AS Qty
			FROM [dbo].[Order] ord WITH (NOLOCK)
				INNER JOIN [dbo].OrderDetail ordd WITH (NOLOCK)
				ON ord.OrderID = ordd.OrderID
				INNER JOIN #EmpRegister tmp WITH (NOLOCK)
				ON ordd.SysId = tmp.SysId
			WHERE ord.OnDate = @OnDate
				AND ord.MealID = @MealID
				AND ord.CurrentStatusID NOT IN (6, 99, 100)
				AND (
					ordd.IsDeleted = 0
					AND ordd.IsCancel = 0
					)
				AND (tmp.RetrictOrder = 1 OR
					(tmp.RetrictOrder = 0 AND ord.MealID =@MealID)
					 )
			)
		INSERT INTO #tmpOrder (
			SysId
			,OrderPerDay
			)
		SELECT SysId
			,SUM(CASE WHEN CTE.CurrentStatusID IN (1,2,3) THEN 100 ELSE CTE.Qty END) OrderPerDay
		FROM CTE
		GROUP BY SysId

	SET @OrderNo = NULL

	SELECT TOP 1 @OrderNo = tmpEmp.SysId 
	FROM #EmpRegister tmpEmp
	INNER JOIN #tmpOrder tmpOrd
		ON tmpEmp.SysId = tmpOrd.SysId
	WHERE tmpOrd.OrderPerDay >= tmpEmp.MaxOrderPerDay AND tmpEmp.RetrictOrder = 0 
	
	DECLARE @NickName Nvarchar(500)=''

	IF @OrderNo IS NOT NULL
	BEGIN	
		SELECT @ErrCode = 102
		
		--SELECT @NickName = COALESCE(@NickName + '<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;', '') + convert(NVARCHAR(50),ISNULL(ee.NickName,'')) 
		SELECT @NickName = COALESCE(@NickName + ',', '') + convert(NVARCHAR(50),ISNULL(ee.NickName,'')) 
		FROM (
			SELECT DISTINCT ee.NickName
			FROM #EmpRegister tmpEmp
				INNER JOIN #tmpOrder tmpOrd
					ON tmpEmp.SysId = tmpOrd.SysId
				INNER JOIN  [dbo].[Employee] ee WITH (NOLOCK)
								ON ee.SysID = tmpEmp.SysID	
			WHERE tmpOrd.OrderPerDay >= tmpEmp.MaxOrderPerDay AND tmpEmp.RetrictOrder = 0 
			) ee

		IF LEN(ISNULL(@NickName,''))>0
			SELECT @NickName=RIGHT(@NickName,LEN(@NickName)-1)

		SELECT @ErrMsg = N'This member has reached his meal limit or included in another order:' + @NickName

		RETURN
	END

	SET @OrderNo = NULL

	SELECT TOP 1 @OrderNo = tmpEmp.SysId 
	FROM #EmpRegister tmpEmp
	INNER JOIN #tmpOrder tmpOrd
		ON tmpEmp.SysId = tmpOrd.SysId
	WHERE tmpOrd.OrderPerDay >= tmpEmp.MaxOrderPerDay AND tmpEmp.RetrictOrder = 1 
	
	IF @OrderNo IS NOT NULL
	BEGIN	
		SELECT @ErrCode = 102
		
		SELECT @NickName = COALESCE(@NickName + ',', '') + convert(NVARCHAR(50),ISNULL(ee.NickName,'')) 
		FROM (
			SELECT DISTINCT ee.NickName
			FROM #EmpRegister tmpEmp
				INNER JOIN #tmpOrder tmpOrd
					ON tmpEmp.SysId = tmpOrd.SysId
				INNER JOIN  [dbo].[Employee] ee WITH (NOLOCK)
								ON ee.SysID = tmpEmp.SysID	
			WHERE tmpOrd.OrderPerDay >= tmpEmp.MaxOrderPerDay AND tmpEmp.RetrictOrder = 1 
			) ee

		IF LEN(ISNULL(@NickName,''))>0
			SELECT @NickName=RIGHT(@NickName,LEN(@NickName)-1)

		SELECT @ErrMsg = N'This member has reached his meal limit or included in another order:' + @NickName
		
		RETURN
	END

	IF (SELECT COUNT(tmp.SysID) FROM #EmpRegister tmp WHERE tmp.IsMainMenu = 1) 
			<>  
			(SELECT COUNT(DISTINCT tmp.SysID)
			FROM #EmpRegister tmp
				INNER JOIN [dbo].[Employee] ee WITH (NOLOCK)
					ON ee.SysId = tmp.SysId
				LEFT JOIN [dbo].[EmpRegisterFood] AS reg WITH (NOLOCK) ON ee.SysID = reg.SysId AND reg.WorkDate = @OnDate AND reg.MealID = @MealID
				INNER JOIN [dbo].[HisEmpMenuActive] AS his WITH (NOLOCK)
					ON ee.EmpCategoryID = his.EmpCategoryID
						AND his.MealID = @MealID
						AND ISNULL(reg.IsVegetarian, 0) = his.IsVegetarian
						AND (his.EffectedToDate IS NULL OR his.EffectedToDate >= @OnDate)  AND his.EffectedFromDate <= @OnDate
				INNER JOIN [dbo].[Meal] AS ml WITH (NOLOCK) ON his.MealID = ml.MealID
				INNER JOIN dbo.EmpMenu AS eMenu WITH(NOLOCK)
					ON eMenu.EmpMenuID = his.EmpMenuID
				INNER JOIN [dbo].[FoodMenu] fmn WITH(NOLOCK)
					ON fmn.EmpMenuID = his.EmpMenuID
						AND fmn.OnDate = @OnDate
						AND fmn.IsActive = 1
				WHERE tmp.IsMainMenu = 1)
	BEGIN
			SELECT @NickName = COALESCE(@NickName + ',', '') + convert(NVARCHAR(50),ISNULL(ee.NickName,'')) 
			FROM #EmpRegister tmp
				INNER JOIN [dbo].[Employee] ee WITH (NOLOCK)
							ON ee.SysID = tmp.SysID
				WHERE tmp.IsMainMenu = 1 AND tmp.SysID NOT IN 
						(
							SELECT DISTINCT tmp.SysID
							FROM #EmpRegister tmp							
								INNER JOIN [dbo].[Employee] ee WITH (NOLOCK)
									ON ee.SysID = tmp.SysID
								LEFT JOIN [dbo].[EmpRegisterFood] AS reg WITH (NOLOCK) ON ee.SysID = reg.SysId AND reg.WorkDate = @OnDate AND reg.MealID = @MealID
								INNER JOIN [dbo].[HisEmpMenuActive] AS his WITH (NOLOCK)
									ON ee.EmpCategoryID = his.EmpCategoryID
										AND his.MealID = @MealID
										AND ISNULL(reg.IsVegetarian, 0) = his.IsVegetarian
										AND (his.EffectedToDate IS NULL OR his.EffectedToDate >= @OnDate)  AND his.EffectedFromDate <= @OnDate
								INNER JOIN [dbo].[Meal] AS ml WITH (NOLOCK) ON his.MealID = ml.MealID
								INNER JOIN dbo.EmpMenu AS eMenu WITH(NOLOCK)
									ON eMenu.EmpMenuID = his.EmpMenuID
								INNER JOIN [dbo].[FoodMenu] fmn WITH(NOLOCK)
									ON fmn.EmpMenuID = his.EmpMenuID
										AND fmn.OnDate = @OnDate
										AND fmn.IsActive = 1
								WHERE tmp.IsMainMenu = 1
						)  

			IF LEN(ISNULL(@NickName,''))>0
				SELECT @NickName=RIGHT(@NickName,LEN(@NickName)-1)
				
			SELECT @ErrCode = 103
			SELECT @ErrMsg = N'There is no menu available for ' + @NickName
		
		RETURN
	END
	
	UPDATE tmp
	SET [QtyVeg] = CASE WHEN erf.IsVegetarian = 1 AND tmp.IsMainMenu = 1 THEN 1 ELSE 0 END
		,[QtyNonVeg] = CASE WHEN erf.IsVegetarian = 1 THEN 0 ELSE 1 END
	FROM #EmpRegister tmp
		LEFT JOIN [dbo].[EmpRegisterFood] erf WITH(NOLOCK)
			ON erf.WorkDate = @OnDate
					AND erf.MealID = @MealID
					AND erf.SysId = tmp.SysId	
	
	SELECT @OrderNo = MAX([OrderNo])
	FROM dbo.[Order] tbl WITH(NOLOCK)
	WHERE tbl.[OnDate] = @OnDate
	
	SELECT @OrderNo=ISNULL(@OrderNo,0) + 1

	DECLARE @OrderNo_Max NVARCHAR(10) 

	SELECT @OrderNo_Max=[Value]  FROM [dbo].[LookupData] WITH(NOLOCK) WHERE LookupId = 6

	IF ISNUMERIC(@OrderNo_Max)=0 
	BEGIN
		SELECT @OrderNo_Max = 999	
	END

	IF @OrderNo > CONVERT(INT,@OrderNo_Max)
	BEGIN
		SELECT @OrderNo = 1
	END
	
	SELECT TOP 1 @LocationID =  [LocationID]
	  FROM [dbo].[Location] l (NOLOCK)
	ORDER BY IsDefault DESC

	INSERT INTO [dbo].[Order]
           ([OnDate]
           ,[MealID]
           ,[OrderNo]
           ,[iRework]
           ,[SysId]
           ,[LocationID]
           ,[CurrentStatusID]
           ,[RegisterDate]
           ,[RegisterBy]
           ,[Price]
           ,[IsSpecial]
           ,[Remark]
		   ,[RemarkVNM]
           ,[phone]
		   ,OrderFrom
		   ,OrderTime
		   ,TakeAway)
     SELECT TOP 1
           @OnDate OnDate
           ,@MealID MealID
           ,@OrderNo OrderNo
           ,0 iRework
           ,@CreatedBy SysId
           ,@LocationID LocationID
           ,1 CurrentStatusID
           ,GETDATE() RegisterDate
           ,@CreatedBy RegisterBy
		   ,0 [Price]
           ,0 IsSpecial
           ,@Remark Remark
		   ,@Remark RemarkVNM
           ,@phone phone
		   ,'P-U' OrderFrom 
		   ,GETDATE() OrderTime
		   ,@TakeAway TakeAway

	SELECT @OrderID = CAST(SCOPE_IDENTITY() AS INT)

	INSERT INTO [dbo].[OrderDetail]
			([OrderID]
			,SysId
			,[MenuId]
			,[QtyVeg]
			,[QtyNonVeg]
			,[Price]
			,[ModifiedBy]
			,[ModifiedDate]					
			,IsDeleted
			,IsCancel)
	SELECT DISTINCT
			@OrderID OrderID 
			,tmp.SysId
			,fmn.MenuId
			,QtyVeg
			,QtyNonVeg
			,fmn.Price
			,@CreatedBy ModifiedBy
			,GETDATE() ModifiedDate 					
			,0 IsDeleted
			,0 IsCancel 
	FROM #EmpRegister tmp
		INNER JOIN [dbo].[Employee] ee WITH (NOLOCK)
			ON ee.SysId = tmp.SysId	
		LEFT JOIN [dbo].[EmpRegisterFood] AS reg WITH (NOLOCK) ON ee.SysID = reg.SysId AND reg.WorkDate = @OnDate AND reg.MealID = @MealID
		INNER JOIN [dbo].[HisEmpMenuActive] AS his WITH (NOLOCK)
			ON ee.EmpCategoryID = his.EmpCategoryID
				AND his.MealID = @MealID
				AND ISNULL(reg.IsVegetarian, 0) = his.IsVegetarian
				AND (his.EffectedToDate IS NULL OR his.EffectedToDate >= @OnDate)  AND his.EffectedFromDate <= @OnDate
		INNER JOIN [dbo].[Meal] AS ml WITH (NOLOCK) ON his.MealID = ml.MealID
		INNER JOIN dbo.EmpMenu AS eMenu WITH(NOLOCK)
			ON eMenu.EmpMenuID = his.EmpMenuID
		INNER JOIN [dbo].[FoodMenu] fmn WITH(NOLOCK)
			ON fmn.EmpMenuID = his.EmpMenuID
				AND fmn.OnDate = @OnDate
				AND fmn.IsActive = 1			
		WHERE  tmp.IsMainMenu = 1

	INSERT INTO [dbo].OrderAlternative (
		[OrderID]
		,[SysId]
		,AltMenuId
		,MenuId
		,[Qty]
		,[Price]
		,[ModifiedBy]
		,[ModifiedDate]
		,IsDeleted
		,IsCancel
		)
	SELECT @OrderID [OrderID]
		,tmp.[SysId]
		,tmp.MenuId AltMenuId
		,fmn.MenuId MenuId
		,1 [Qty]
		,fmn.[Price]
		,@CreatedBy ModifiedBy
		,GETDATE() ModifiedDate		
		,0 IsDeleted
		,0 IsCancel 
	FROM #EmpRegister tmp  WITH(NOLOCK)
		INNER JOIN [dbo].[Employee] ee WITH (NOLOCK)
			ON ee.SysId = tmp.SysId	
		LEFT JOIN [dbo].[EmpRegisterFood] AS reg WITH (NOLOCK) ON ee.SysID = reg.SysId AND reg.WorkDate = @OnDate AND reg.MealID = @MealID
				INNER JOIN [dbo].[HisEmpMenuActive] AS his WITH (NOLOCK)
					ON ee.EmpCategoryID = his.EmpCategoryID
						AND his.MealID = @MealID
						AND ISNULL(reg.IsVegetarian, 0) = his.IsVegetarian
						AND (his.EffectedToDate IS NULL OR his.EffectedToDate >= @OnDate)  AND his.EffectedFromDate <= @OnDate
				INNER JOIN [dbo].[Meal] AS ml WITH (NOLOCK) ON his.MealID = ml.MealID
				INNER JOIN dbo.EmpMenu AS eMenu WITH(NOLOCK)
					ON eMenu.EmpMenuID = his.EmpMenuID
				INNER JOIN [dbo].[FoodMenu] fmn WITH(NOLOCK)
					ON fmn.EmpMenuID = his.EmpMenuID
						AND fmn.OnDate = @OnDate
						AND fmn.IsActive = 1	
		WHERE  tmp.IsMainMenu = 0

	INSERT INTO [dbo].[OrderOptional] (
		[OrderID]
		  ,[SysId]
		  ,[FoodId]
		  ,[Qty]
		  ,[Price]
		  ,[ModifiedBy]
		  ,[ModifiedDate]
		  ,IsDeleted
	)
	SELECT @OrderID [OrderID]
		,tmp.[SysId]
		,tmp.[FoodId]
		,tmp.[Qty]
		,f.[Price]
		,@CreatedBy ModifiedBy
		,GETDATE() ModifiedDate
		,0 IsDeleted
	FROM #EmpFood tmp  WITH(NOLOCK)
		LEFT JOIN dbo.Food f  WITH(NOLOCK)
			ON tmp.FoodId = f.FoodId

 	SELECT @ErrCode = 0
		,@ErrMsg = ''
	
	IF OBJECT_ID('tempdb..#EmpRegister') IS NOT NULL
	BEGIN
		DROP TABLE #EmpRegister
	END
		
	SET @EndRun = GETDATE()
	SET @Duration = DATEDIFF(MILLISECOND, @StartRun, @EndRun) 
	
	EXEC	dbo.SYS_WriteLog_RunSp
			@UserId		= @CreatedBy,
			@SPName		= 'POS_Insert_OrderMealByUser',
			@LogDate	= @StartRun,
			@Duration	= @Duration,
			@ObjectId	= @OrderID,
			@Data		= @LogTraderData,
			@OldData	= @LogTraderOldData
END

GO

CREATE OR ALTER   PROCEDURE [dbo].[RegisterMeal_CancelByDate]
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

GO

CREATE OR ALTER   PROCEDURE [dbo].[RegisterMeal_GetEmployeeRegisterByDate]
    @OnDate DATE
AS
BEGIN
    SET NOCOUNT ON;

    SELECT e.EmpId, e.SysID, e.FullName, e.NickName, e.DomainUsername, e.Code, e.CompanyID
	FROM dbo.Employee e
	WHERE e.CompanyID IN (1, 4)
	  AND ISNULL(e.[Enabled], 1) = 1
	  AND ISNULL(e.isDeleted, 0) = 0
	  AND EXISTS (
		  SELECT 1 
		  FROM EmpRegisterFood er
		  WHERE er.SysID = e.SysID
			AND er.WorkDate = @OnDate
			AND er.MealID = 2
			AND er.IsVegetarian IS NOT NULL
	  )

END
GO

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

		-- Check if trying to register after cutoff time (14:00 of previous day)
		DECLARE @CutoffTime DATETIME = DATEADD(HOUR, 14, CAST(DATEADD(DAY, -1, @OnDate) AS DATETIME))
		IF GETDATE() > @CutoffTime
		BEGIN
			SET @ErrCode = 4
			SET @ErrMsg = N'Không thể đăng ký suất ăn chay sau 14:00.'
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

GO


