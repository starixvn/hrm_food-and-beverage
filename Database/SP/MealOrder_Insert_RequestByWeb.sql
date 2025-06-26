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