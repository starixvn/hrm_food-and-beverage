// --------------------------------------------------------------------------
// <copyright file="ViewModelBase.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.10.16">Created</change>
// </history>

namespace NexFB.WinAppSmartCard.ViewModels
{
    using NexFB.Services.Implement;
    using NexFB.Services.Interface;
    using NexFB.WinAppSmartCard.Core.NotifyBase;

    /// <summary>
    /// ViewModel Base.
    /// </summary>
    public class ViewModelBase : PropertyChangedBase
    {
        private IKitchenService _kitchenService;
        private ISmartCardOrderService _smartCardOrderService;

        public ViewModelBase(IKitchenService kitchenService, ISmartCardOrderService smartCardOrderService)
        {
            _kitchenService = kitchenService ?? new KitchenService();
            _smartCardOrderService = smartCardOrderService ?? new SmartCardOrderService();
        }

        public ViewModelBase()
            : this(null, null)
        {
        }

        #region Properties

        public IKitchenService KitchenService
        {
            get
            {
                return _kitchenService;
            }

            set
            {
                if (_kitchenService == value)
                {
                    return;
                }

                _kitchenService = value;
                OnPropertyChanged("KitchenService");
            }
        }

        public ISmartCardOrderService SmartCardOrderService
        {
            get
            {
                return _smartCardOrderService;
            }

            set
            {
                if (_smartCardOrderService == value)
                {
                    return;
                }

                _smartCardOrderService = value;
                OnPropertyChanged("SmartCardOrderService");
            }
        }

        #endregion
    }
}
