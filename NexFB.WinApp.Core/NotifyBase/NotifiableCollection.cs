// --------------------------------------------------------------------------
// <copyright file="NotifiableCollection.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.9.15">Created</change>
// </history>

namespace NexFB.WinApp.Core.NotifyBase
{
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;

    /// <summary>
    /// Collection Changed Base.
    /// </summary>
    /// <typeparam name="T">Generic type.</typeparam>
    public class NotifiableCollection<T> : Collection<T>, INotifyCollectionChanged
    {
        public NotifiableCollection()
        {
        }

        #region INotifyCollectionChanged Members
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        protected void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (CollectionChanged != null)
            {
                CollectionChanged(this, e);
            }
        }
        #endregion

        protected override void InsertItem(int index, T item)
        {
            base.InsertItem(index, item);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
        }

        protected override void RemoveItem(int index)
        {
            T item = base[index];
            base.RemoveItem(index);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
        }

        protected override void ClearItems()
        {
            base.ClearItems();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        protected void Move(int oldIndex, int newIndex)
        {
            T item = base[oldIndex];
            base.SetItem(newIndex, item);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, item, newIndex, oldIndex));
        }

        protected override void SetItem(int index, T item)
        {
            T oldValue = base[index];
            base.SetItem(index, item);

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, item, oldValue));
        }
    }
}
