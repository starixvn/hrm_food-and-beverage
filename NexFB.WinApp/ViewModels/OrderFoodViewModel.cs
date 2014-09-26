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
    using System.Windows.Input;
    using NexFB.WinApp.Core.Commands;
    using NexFB.WinApp.Core.Constants;

    /// <summary>
    /// Order Food ViewModel.
    /// </summary>
    public class OrderFoodViewModel : ViewModelBase
    {
        #region Fields

        private string _result;
        private string _borderButtonPickRoom;
        private string _borderButtonChooseDishes;
        private string _borderButtonPickGroup;
        private string _imageCancel;

        #endregion

        #region Constructors

        public OrderFoodViewModel()
        {
            NumPadPressed = new RelayCommand((x) => CanExecuteNumPadPressed(), (y) => ExecuteNumPadPressed(y));
            NumPadReleased = new RelayCommand((x) => CanExecuteNumPadReleased(), (y) => ExecuteNumPadPressed(y));
            PickRoomCommand = new RelayCommand((x) => CanExecutePickRoomCommand(), (y) => ExecutePickRoomCommand());
            PickGroupCommand = new RelayCommand((x) => CanExecutePickGroupCommand(), (y) => ExecutePickGroupCommand());
            ChooseDishesCommand = new RelayCommand((x) => CanExecuteChooseDishesCommand(), (y) => ExecuteChooseDishesCommand());
        }

        #endregion

        #region Properties

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
        #endregion

        #region Methods

        /// <summary>
        /// Change button focused color.
        /// </summary>
        /// <param name="currentButton">Value of current button.</param>
        public void ChangeButtonBorder(int currentButton)
        {
            string red = Constant.Red;
            string defaultColor = Constant.DefaultMenuColor;
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
