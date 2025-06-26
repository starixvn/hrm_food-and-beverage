namespace NexFB.Pos.Core.NotifyBase
{
    using System.ComponentModel;

    /// <summary>
    /// Property Changed Base.
    /// </summary>
    public abstract class PropertyChangedBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
