ALTER PROCEDURE [dbo].[Rpt_Get_MonthlyLunchAndDinnerMealsAmount] @reportDate DATE
	,@CompanyId NVARCHAR(100) = NULL
AS
/*
Created: Oliver.Dang@2018-04-16
Task: get lunch and dinner meals amount report
DB: [FBM]

Revisions:
Param's Explanation:
*/
BEGIN
	SET NOCOUNT ON;

	SELECT @CompanyId = ISNULL(@CompanyId, '')

	CREATE TABLE #tmpCompany (CompanyId SMALLINT)
    CREATE TABLE #tmpOrderQty (OnDate DATE, MealId TINYINT, Qty INT, Price MONEY)

	INSERT INTO #tmpCompany (CompanyId)
	SELECT val
	FROM fnc_strSplit(@CompanyId, ',') AS fnc
		INNER JOIN [dbo].[Company] AS dp WITH (NOLOCK) ON fnc.val = dp.CompanyId

	INSERT INTO #tmpOrderQty (OnDate, MealId, Qty, Price)
    SELECT ord.OnDate
		,ord.MealId
        ,QtyNonVeg + QtyVeg
        ,od.Price
	FROM dbo.[Order] AS ord WITH (NOLOCK)
		INNER JOIN dbo.OrderDetail AS od WITH (NOLOCK) ON od.OrderId = ord.OrderId
		INNER JOIN dbo.Employee AS emp WITH (NOLOCK) ON od.SysId = emp.SysId
		INNER JOIN #tmpCompany AS cp WITH (NOLOCK) ON emp.CompanyId = cp.CompanyId
	WHERE MONTH(ord.OnDate) = MONTH(@reportDate)
		AND YEAR(ord.OnDate) = YEAR(@reportDate)
        AND ord.CurrentStatusID IN (4, 5)
        AND ord.MealId IN (2, 3)
        AND od.IsCancel = 0
        AND od.IsDeleted = 0
	ORDER BY ord.OnDate

    INSERT INTO #tmpOrderQty (OnDate, MealId, Qty, Price)
    SELECT ord.OnDate
		,ord.MealId
        ,orda.Qty
		,orda.Price
    FROM dbo.[Order] AS ord WITH (NOLOCK)
        INNER JOIN dbo.OrderAlternative AS orda WITH (NOLOCK) ON ord.OrderId = orda.OrderId
		INNER JOIN dbo.Employee AS emp WITH (NOLOCK) ON orda.SysId = emp.SysId
		INNER JOIN #tmpCompany AS cp WITH (NOLOCK) ON emp.CompanyId = cp.CompanyId
    WHERE MONTH(ord.OnDate) = MONTH(@reportDate)
		AND YEAR(ord.OnDate) = YEAR(@reportDate)
        AND ord.CurrentStatusID IN (4, 5)
        AND ord.MealId IN (2, 3)
        AND orda.IsCancel = 0
        AND orda.IsDeleted = 0
	ORDER BY ord.OnDate

    SELECT OnDate        
        ,SUM(CASE WHEN MealId = 2 THEN Qty END) AS LunchMealQuantity
		,SUM(CASE WHEN MealId = 2 THEN Qty * Price END) AS LunchAmount
		,SUM(CASE WHEN MealId = 3 THEN Qty END) AS DinnerMealQuantity
		,SUM(CASE WHEN MealId = 3 THEN Qty * Price END) AS DinnerAmount
    FROM #tmpOrderQty
    GROUP BY OnDate
    ORDER BY OnDate

	DROP TABLE #tmpCompany
    DROP TABLE #tmpOrderQty
END