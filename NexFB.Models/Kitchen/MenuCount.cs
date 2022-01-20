namespace NexFB.Models.Kitchen
{
    public class MenuCount
    {
        public int MenuId { get; set; }

        public int Quantity { get; set; }

        public string MenuVnmName { get; set; }

        public string MenuEngName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S109:Magic numbers should not be used", Justification = "<Pending>")]
        public int DisplayRowHeight
        {
            get
            {
                //// Grater than 25 and less than 45 letter: set Height = 40, 
                //// Less than 25 letter Height = 28
                //// Grater than 45 letter Height = 52
                var textLength = MenuVnmName.Length;
                if (textLength <= 45 && textLength > 25)
                {
                    return 40;
                }
                else
                {
                    return textLength > 45 ? 65 : 28;
                }
            }
        }
    }
}
