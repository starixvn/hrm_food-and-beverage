// --------------------------------------------------------------------------
// <copyright file="PropertyChangedBase.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.8.21">Created</change>
// </history>

namespace NexFB.WinApp.Core.NotifyBase
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
