namespace NexFB.WinAppSmartCard.ViewModels.Properties
{
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using NexFB.WinAppSmartCard.Core.Models;

    /// <summary>
    /// Main Dish Model Property.
    /// </summary>
    public class MainDishModelProperty : ViewModelBase
    {
        private ObservableCollection<Employee> _observableListEmployee = new ObservableCollection<Employee>();
        private Employee _orderOwner;
        private Food _selectedFood;
        private string _screenOrderVisible;
        private string _deleteButtonVisible;
        private string _screenNotificationVisible;
        private string _notifyErrorOrder;
        private string _errorEmployees;
        private int _orderNo;
        private int _countDownTimer;
        private bool _mainDishTimerLive;

        public ICommand RemoveEmployeeOrder { get; set; }

        public ICommand EditMenu { get; set; }

        public ICommand ConfirmCommand { get; set; }

        public ICommand CancelCommand { get; set; }

        public Food SelectedFood
        {
            get
            {
                return _selectedFood;
            }

            set
            {
                _selectedFood = value;
                this.OnPropertyChanged("SelectedFood");
            }
        }

        public bool MainDishTimerLive
        {
            get
            {
                return _mainDishTimerLive;
            }

            set
            {
                _mainDishTimerLive = value;
                this.OnPropertyChanged("MainDishTimerLive");
            }
        }

        public int OrderNo
        {
            get
            {
                return _orderNo;
            }

            set
            {
                _orderNo = value;
                this.OnPropertyChanged("OrderNo");
            }
        }

        public int CountDownTimer
        {
            get
            {
                return _countDownTimer;
            }

            set
            {
                _countDownTimer = value;
                this.OnPropertyChanged("CountDownTimer");
            }
        }

        public string ScreenOrderVisible
        {
            get
            {
                return _screenOrderVisible;
            }

            set
            {
                _screenOrderVisible = value;
                OnPropertyChanged("ScreenOrderVisible");
            }
        }

        public string DeleteButtonVisible
        {
            get
            {
                return _deleteButtonVisible;
            }

            set
            {
                _deleteButtonVisible = value;
                OnPropertyChanged("DeleteButtonVisible");
            }
        }

        public string ScreenNotificationVisible
        {
            get
            {
                return _screenNotificationVisible;
            }

            set
            {
                _screenNotificationVisible = value;
                OnPropertyChanged("ScreenNotificationVisible");
            }
        }
        public string NotifyErrorOrder
        {
            get
            {
                return _notifyErrorOrder;
            }

            set
            {
                _notifyErrorOrder = value;
                OnPropertyChanged("NotifyErrorOrder");
            }
        }

        public string ErrorEmployees
        {
            get
            {
                return _errorEmployees;
            }

            set
            {
                _errorEmployees = value;
                OnPropertyChanged("ErrorEmployees");
            }
        }

        public Employee OrderOwner
        {
            get
            {
                return _orderOwner;
            }

            set
            {
                _orderOwner = value;
                OnPropertyChanged("OrderOwner");
            }
        }

        public ObservableCollection<Employee> ObservableListEmployee
        {
            get
            {
                return _observableListEmployee;
            }

            set
            {
                _observableListEmployee = value;
                this.OnPropertyChanged("ObservableListEmployee");
            }
        }
    }
}
