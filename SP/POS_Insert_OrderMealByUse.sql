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