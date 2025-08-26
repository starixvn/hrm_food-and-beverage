using System;

namespace NexFB.WorkerService.Models;

public class Section
{
    public int SectionId { get; set; }

    public string Code { get; set; }

    public string Name { get; set; }

    public string DepartmentId { get; set; }

    public bool IsActive { get; set; }

    public DateTime ModifiedDate { get; set; }

    public bool IsDeleted { get; set; }
}