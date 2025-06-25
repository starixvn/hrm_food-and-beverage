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