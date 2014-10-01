// --------------------------------------------------------------------------
// <copyright file="EmployeeSelectionViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.10.1">Created</change>
// </history>

namespace NexFB.WinApp.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using NexFB.Resources;
    using NexFB.WinApp.Core.Models;

    /// <summary>
    /// Employee Selection ViewModel.
    /// </summary>
    public class EmployeeSelectionViewModel : ViewModelBase
    {
        private static ObservableCollection<KitchenEmployeeInfo> _employeeInfo = new ObservableCollection<KitchenEmployeeInfo>();
        private string _messageHeader;

        public EmployeeSelectionViewModel(List<KitchenEmployeeInfo> employeeInfo)
        {
            EmployeeInfo = new ObservableCollection<KitchenEmployeeInfo>(employeeInfo);
            MessageHeader = string.Format(WinFormMainResource.DuplicateEmployeeMessage, EmployeeInfo.Count);
        }

        public ObservableCollection<KitchenEmployeeInfo> EmployeeInfo
        {
            get
            {
                return _employeeInfo;
            }

            set
            {
                if (_employeeInfo == value)
                {
                    return;
                }

                _employeeInfo = value;
                OnPropertyChanged("EmployeeInfo");
            }
        }

        public string MessageHeader
        {
            get
            {
                return _messageHeader;
            }

            set
            {
                if (_messageHeader == value)
                {
                    return;
                }

                _messageHeader = value;
                OnPropertyChanged("MessageHeader");
            }
        }
    }
}
