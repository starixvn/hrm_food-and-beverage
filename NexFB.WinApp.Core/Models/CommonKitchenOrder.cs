// --------------------------------------------------------------------------
// <copyright file="CommonKitchenOrder.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.9.30">Created</change>
// </history>

namespace NexFB.WinApp.Core.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using NexFB.WinApp.Core.NotifyBase;

    /// <summary>
    /// Common Kitchen Order.
    /// </summary>
    public class CommonKitchenOrder : PropertyChangedBase
    {
        private int _itemId;
        private string _itemName;
        private bool _isSelectedMealMenu;

        public bool IsSelectedMealMenu
        {
            get
            {
                return _isSelectedMealMenu;
            }

            set
            {
                _isSelectedMealMenu = value;
                this.OnPropertyChanged("IsSelectedMealMenu");
            }
        }

        public int ItemId
        {
            get
            {
                return _itemId;
            }

            set
            {
                _itemId = value;
                this.OnPropertyChanged("ItemId");
            }
        }

        public string ItemName
        {
            get
            {
                return _itemName;
            }

            set
            {
                _itemName = value;
                this.OnPropertyChanged("ItemName");
            }
        }
    }
}
