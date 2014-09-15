// --------------------------------------------------------------------------
// <copyright file="MenuColor.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.9.12">Created</change>
// </history>

namespace NexFB.WinApp.Core.Models
{
    using NexFB.WinApp.Core.NotifyBase;

    /// <summary>
    /// Menu Color.
    /// </summary>
    public class MenuColor : PropertyChangedBase
    {
        private string _background;
        private string _foreground;
        private string _fontWeight;

        public string BackgroundColor
        {
            get
            {
                return _background;
            }

            set
            {
                _background = value;
                this.OnPropertyChanged("BackgroundColor");
            }
        }

        public string ForegroundColor
        {
            get
            {
                return _foreground;
            }

            set
            {
                _foreground = value;
                this.OnPropertyChanged("ForegroundColor");
            }
        }

        public string FontWeight
        {
            get
            {
                return _fontWeight;
            }

            set
            {
                _fontWeight = value;
                this.OnPropertyChanged("FontWeight");
            }
        }
    }
}
