namespace NexFB.WorkerService.Models;

public class Team
{
    public int TeamId { get; set; }

    public string Code { get; set; }

    public string Name { get; set; }

    public string DepartmentId { get; set; }

    public string ModifiedDate { get; set; }

    public string IsDeleted { get; set; }
}