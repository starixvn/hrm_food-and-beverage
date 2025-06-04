namespace NexFB.Models.UserControl
{
    using System.Collections.Generic;

    /// <summary>
    /// The function model.
    /// </summary>
    public class FunctionModel
    {
        public FunctionModel()
        {
            ChildFunctions = new List<FunctionModel>();
        }

        public int FunctionId { get; set; }

        public string FunctionKey { get; set; }

        public string Name { get; set; }

        public int? ParentFunctionId { get; set; }

        public bool IsReadOnly { get; set; }

        public bool IsRead { get; set; }

        public bool IsHost { get; set; }

        public List<FunctionModel> ChildFunctions { get; set; }
    }
}
