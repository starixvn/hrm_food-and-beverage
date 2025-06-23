CREATE OR ALTER PROCEDURE [dbo].[RegisterMeal_GetEmployeeRegisterByDate]
    @OnDate DATE
AS
BEGIN
    SET NOCOUNT ON;

    SELECT e.SysID
	FROM dbo.Employee e
	WHERE e.CompanyID IN (2)
	  AND ISNULL(e.[Enabled], 1) = 1
	  AND ISNULL(e.isDeleted, 0) = 0
	  AND EXISTS (
		  SELECT 1 
		  FROM EmpRegisterFood er
		  WHERE er.SysID = e.SysID
			AND er.WorkDate = @OnDate
			AND er.MealID = 2
			AND er.IsVegetarianManual IS NOT NULL
	  )

END