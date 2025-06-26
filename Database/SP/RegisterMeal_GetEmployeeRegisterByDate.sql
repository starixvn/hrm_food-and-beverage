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