namespace NexFB.Models.SideDishManagement
{
    /// <summary>
    /// The unit model.
    /// </summary>
    public class UnitModel
    {
        public int UnitId { get; set; }

        public string UnitEngName { get; set; }

        public string UnitVnmName { get; set; }

        public string UnitName
        {
            get
            {
                return UnitEngName;
            }
        }
    }
}
