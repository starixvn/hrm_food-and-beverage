// --------------------------------------------------------------------------
// <copyright file="MenuColor.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.9.12">Created</change>
// </history>

namespace NexFB.Pos.Core.Models
{
    using NexFB.Pos.Core.NotifyBase;

    /// <summary>
    /// Menu Color.
    /// </summary>
    public class MenuColor : PropertyChangedBase
    {
        #region Private Fields

        private string _background;
        private string _foreground;
        private string _fontWeight;

        #endregion

        #region Public Properties

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

        #endregion
    }
}
