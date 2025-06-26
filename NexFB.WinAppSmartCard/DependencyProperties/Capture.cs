namespace NexFB.WinAppSmartCard.DependencyProperties
{
    using System;
    using System.Collections.Specialized;
    using System.Windows.Controls;

    /// <summary>
    /// Class Capture.
    /// </summary>
    public class Capture : IDisposable
    {
        public Capture(ListBox listBox)
        {
            this.ListBox = listBox;
            ListBoxChange = listBox.ItemsSource as INotifyCollectionChanged;
            if (ListBoxChange != null)
            {
                ListBoxChange.CollectionChanged +=
                    new NotifyCollectionChangedEventHandler(ListBoxCollectionChanged);
            }
        }

        public ListBox ListBox { get; set; }

        public INotifyCollectionChanged ListBoxChange { get; set; }

        public void Dispose()
        {
            if (ListBoxChange != null)
            {
                ListBoxChange.CollectionChanged -= ListBoxCollectionChanged;
            }
        }

        public void ListBoxCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                ListBox.ScrollIntoView(e.NewItems[0]);
                ListBox.SelectedItem = e.NewItems[0];
            }
        }
    }
}
