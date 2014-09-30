// --------------------------------------------------------------------------
// <copyright file="OrderFoodViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.8.28">Created</change>
// </history>

namespace NexFB.WinApp.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Windows.Input;
    using NexFB.Criteria.Kitchen;
    using NexFB.Models.Kitchen;
    using NexFB.WinApp.Core.Commands;
    using NexFB.WinApp.Core.Constants;

    /// <summary>
    /// Order Food ViewModel.
    /// </summary>
    public class OrderFoodViewModel : ViewModelBase
    {
        #region Fields

        private static ObservableCollection<Order> _waitingMeals1 = new ObservableCollection<Order>();
        private static ObservableCollection<Order> _waitingMeals2 = new ObservableCollection<Order>();
        private string _result;
        private string _borderButtonPickRoom;
        private string _borderButtonChooseDishes;
        private string _borderButtonPickGroup;
        private string _imageCancel;

        #endregion

        #region Constructors

        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Reviewed. Suppression is OK here.")]
        public OrderFoodViewModel()
        {
            ChangeButtonBorder(0);
            NumPadPressed = new RelayCommand((x) => CanExecuteNumPadPressed(), (y) => ExecuteNumPadPressed(y));
            NumPadReleased = new RelayCommand((x) => CanExecuteNumPadReleased(), (y) => ExecuteNumPadPressed(y));
            PickRoomCommand = new RelayCommand((x) => CanExecutePickRoomCommand(), (y) => ExecutePickRoomCommand());
            PickGroupCommand = new RelayCommand((x) => CanExecutePickGroupCommand(), (y) => ExecutePickGroupCommand());
            ChooseDishesCommand = new RelayCommand((x) => CanExecuteChooseDishesCommand(), (y) => ExecuteChooseDishesCommand());
            DefaulMenuSelected = new RelayCommand((x) => CanExecuteMealMenuSelected(), (y) => ExecuteMealMenuSelected(y));
            DefaultEmployeeSelected = new RelayCommand((x) => CanExecuteEmployeeInGroupSelected(), (y) => ExecuteEmployeeInGroupSelected(y));

            WaitingMeals1 = this.GetWaitingMealOrder;
            WaitingMeals2 = this.GetWaitingMealOrder;
        }

        #endregion

        #region Properties

        public ObservableCollection<Order> GetWaitingMealOrder
        {
            get
            {
                var criteria = new GetWaitingMealOrderCriteria();
                List<Order> waitingMeals = KitchenService.GetWaitngMealOrder(criteria);

                ObservableCollection<Order> observableWaitingMeals = new ObservableCollection<Order>(waitingMeals);
                return observableWaitingMeals;
            }
        }

        public ObservableCollection<Order> WaitingMeals1
        {
            get
            {
                return _waitingMeals1;
            }

            set
            {
                if (_waitingMeals1 == value)
                {
                    return;
                }

                _waitingMeals1 = value;
                OnPropertyChanged("WaitingMeals1");
            }
        }

        public ObservableCollection<Order> WaitingMeals2
        {
            get
            {
                return _waitingMeals2;
            }

            set
            {
                if (_waitingMeals2 == value)
                {
                    return;
                }

                _waitingMeals2 = value;
                OnPropertyChanged("WaitingMeals2");
            }
        }

        public string Result
        {
            get
            {
                return _result;
            }

            set
            {
                _result = value;
                if (this._result.Length == 5)
                {
                    ////MessageBox.Show("123");
                }

                this.OnPropertyChanged("Result");
            }
        }

        public string BorderButtonPickRoom
        {
            get
            {
                return _borderButtonPickRoom;
            }

            set
            {
                _borderButtonPickRoom = value;
                this.OnPropertyChanged("BorderButtonPickRoom");
            }
        }

        public string BorderButtonChooseDishes
        {
            get
            {
                return _borderButtonChooseDishes;
            }

            set
            {
                _borderButtonChooseDishes = value;
                this.OnPropertyChanged("BorderButtonChooseDishes");
            }
        }

        public string BorderButtonPickGroup
        {
            get
            {
                return _borderButtonPickGroup;
            }

            set
            {
                _borderButtonPickGroup = value;
                this.OnPropertyChanged("BorderButtonPickGroup");
            }
        }

        public string ImageCancel
        {
            get
            {
                return _imageCancel;
            }

            set
            {
                _imageCancel = value;
                this.OnPropertyChanged("ImageCancel");
            }
        }

        public ICommand NumPadPressed { get; set; }

        public ICommand NumPadReleased { get; set; }

        public ICommand PickGroupCommand { get; set; }

        public ICommand PickRoomCommand { get; set; }

        public ICommand ChooseDishesCommand { get; set; }

        public ICommand DefaulMenuSelected { get; set; }

        public ICommand DefaultEmployeeSelected { get; set; }

        #endregion

        #region RelayCommands

        // Can Execute Command.
        public bool CanExecutePickRoomCommand()
        {
            return true;
        }

        public bool CanExecutePickGroupCommand()
        {
            return true;
        }

        public bool CanExecuteChooseDishesCommand()
        {
            return true;
        }

        public bool CanExecuteNumPadPressed()
        {
            if (Result != null && Result.Length == 5)
            {
                return false;
            }

            return true;
        }

        public bool CanExecuteMealMenuSelected()
        {
            return true;
        }

        public bool CanExecuteEmployeeInGroupSelected()
        {
            return true;
        }

        /// <summary>
        /// Action for Keypad Released.
        /// </summary>
        /// <returns>Boolean to check.</returns>
        public bool CanExecuteNumPadReleased()
        {
            if (string.IsNullOrEmpty(Result))
            {
                ImageCancel = Constant.CancelImageDisabled;
                return false;
            }
            else
            {
                ImageCancel = Constant.CancelImageActive;
                return true;
            }
        }

        // Execute Command
        public void ExecuteChooseDishesCommand()
        {
            ChangeButtonBorder(3);
        }

        public void ExecutePickRoomCommand()
        {
            ChangeButtonBorder(1);
        }

        public void ExecutePickGroupCommand()
        {
            ChangeButtonBorder(2);
        }

        /// <summary>
        /// Method Press virtual key pad.
        /// </summary>
        /// <param name="value">Object parameter.</param>
        public void ExecuteNumPadPressed(object value)
        {
            var keyPadParameter = value.ToString();
            switch (keyPadParameter)
            {
                case "ESC":
                    break;

                case "RETURN":
                    break;

                case "BACK":
                    if (Result.Length > 0)
                    {
                        Result = Result.Remove(Result.Length - 1);
                    }

                    break;

                default:
                    string keypadValue = keyPadParameter.Substring(keyPadParameter.Length - 1, 1);
                    Result += keypadValue;
                    break;
            }
        }

        /// <summary>
        /// Execute Meal Menu Selected.
        /// </summary>
        /// <param name="value">Value pass from user interface.</param>
        public void ExecuteMealMenuSelected(object value)
        {
            if (value != null)
            {
                var orderId = (int)value;
                WaitingMeals1.Where(x => x.OrderId != orderId).ToList().ForEach(y => y.IsSelectedMealMenu = false);
                WaitingMeals1.Where(x => x.OrderId == orderId).FirstOrDefault().IsSelectedMealMenu = true;
            }

            WaitingMeals1 = new ObservableCollection<Order>(this.WaitingMeals1);
        }

        public void ExecuteEmployeeInGroupSelected(object value)
        {
            if (value != null)
            {
                var orderId = (int)value;
                WaitingMeals2.Where(x => x.OrderId != orderId).ToList().ForEach(y => y.IsSelectedMemberInGroup = false);
                WaitingMeals2.Where(x => x.OrderId == orderId).FirstOrDefault().IsSelectedMemberInGroup = true;
            }

            WaitingMeals2 = new ObservableCollection<Order>(this.WaitingMeals2);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Change button focused color.
        /// </summary>
        /// <param name="currentButton">Value of current button.</param>
        public void ChangeButtonBorder(int currentButton)
        {
            string red = Constant.Red;
            string defaultColor = Constant.DefaultButtonOrderBorderColor;
            switch (currentButton)
            {
                case 1:
                    BorderButtonPickRoom = red;
                    BorderButtonPickGroup = defaultColor;
                    BorderButtonChooseDishes = defaultColor;
                    break;
                case 2:
                    BorderButtonPickRoom = defaultColor;
                    BorderButtonPickGroup = red;
                    BorderButtonChooseDishes = defaultColor;
                    break;
                case 3:
                    BorderButtonPickRoom = defaultColor;
                    BorderButtonPickGroup = defaultColor;
                    BorderButtonChooseDishes = red;
                    break;
                default:
                    BorderButtonPickRoom = defaultColor;
                    BorderButtonPickGroup = defaultColor;
                    BorderButtonChooseDishes = defaultColor;
                    break;
            }
        }

        #endregion
    }
}
