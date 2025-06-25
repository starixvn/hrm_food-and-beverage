/****** Object:  Stored Procedure dbo.MealOrder_Edit_RequestByWeb    Script Date: 4/13/2015 9:50:19 AM ******/
/****** Object:  Stored Procedure dbo.MealOrder_Edit_RequestByWeb    Script Date: 3/27/2015 3:36:46 PM ******/
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

