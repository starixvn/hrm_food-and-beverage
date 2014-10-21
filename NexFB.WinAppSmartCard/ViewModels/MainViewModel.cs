// --------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.10.17">Created</change>
// </history>

namespace NexFB.WinAppSmartCard.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Input;
    using AutoMapper;
    using NexFB.Criteria.SmartCardOrder;
    using NexFB.Models.SmartCardOrder;
    using NexFB.WinAppSmartCard.Core.Commands;
    using NexFB.WinAppSmartCard.Core.Constants;
    using NexFB.WinAppSmartCard.Core.Models;
    using NexFB.WinAppSmartCard.GlobalResources.Services;

    /// <summary>
    /// Main View Model.
    /// </summary>
    public partial class MainViewModel : ViewModelBase
    {
        private static double _screenWidth = SystemParameters.PrimaryScreenWidth;
        private static double _screeHeight = SystemParameters.PrimaryScreenHeight;

        private ViewModelBase _currentViewModel;
        private string _currentTime;
        private int _currentCulture;
        private string _mainScreenVisible;
        private string _standbyScreenVisible;
        private string _currentCardNumber;
        private MenuColor _mainDishesMenu = new MenuColor();
        private MenuColor _sideDishesMenu = new MenuColor();

        public MainViewModel()
        {
            ExecuteMainDishesCommand();
            MainDishesCommand = new RelayCommand((x) => CanExecuteMainDishesCommand(), (y) => ExecuteMainDishesCommand());
            SideDishesCommand = new RelayCommand((x) => CanExecuteSideDishesCommand(), (y) => ExecuteSideDishesCommand());
            ChangeLanguageCommand = new RelayCommand((x) => CanExecuteChangeLanguageCommand(), (y) => ExecuteChangeLanguageCommand(y));
            ChangeCulture();
            CheckCurrentScreen();
        }

        #region Properties

        public ICommand MainDishesCommand { get; set; }

        public ICommand SideDishesCommand { get; set; }

        public ICommand ChangeLanguageCommand { get; set; }

        public string CurrentTime
        {
            get
            {
                return _currentTime;
            }

            set
            {
                _currentTime = value;
                OnPropertyChanged("CurrentTime");
            }
        }

        public ViewModelBase CurrentViewModel
        {
            get
            {
                return _currentViewModel;
            }

            set
            {
                if (_currentViewModel == value)
                {
                    return;
                }

                _currentViewModel = value;
                OnPropertyChanged("CurrentViewModel");
            }
        }

        public int CurrentCulture
        {
            get
            {
                return _currentCulture;
            }

            set
            {
                _currentCulture = value;
                this.OnPropertyChanged("CurrentCulture");
            }
        }

        public string MainScreenVisible
        {
            get
            {
                return _mainScreenVisible;
            }

            set
            {
                _mainScreenVisible = value;
                this.OnPropertyChanged("MainScreenVisible");
            }
        }

        public string StandbyScreenVisible
        {
            get
            {
                return _standbyScreenVisible;
            }

            set
            {
                _standbyScreenVisible = value;
                this.OnPropertyChanged("StandbyScreenVisible");
            }
        }

        public string CurrentCardNumber
        {
            get
            {
                return _currentCardNumber;
            }

            set
            {
                _currentCardNumber = value;
                this.OnPropertyChanged("CurrentCardNumber");
            }
        }

        public MenuColor MainDishesMenu
        {
            get
            {
                return _mainDishesMenu;
            }

            set
            {
                _mainDishesMenu = value;
                this.OnPropertyChanged("MainDishesMenu");
            }
        }

        public MenuColor SideDishesMenu
        {
            get
            {
                return _sideDishesMenu;
            }

            set
            {
                _sideDishesMenu = value;
                this.OnPropertyChanged("SideDishesMenu");
            }
        }

        public double ScreenWidth
        {
            get
            {
                return _screenWidth;
            }

            set
            {
                _screenWidth = value;
                this.OnPropertyChanged("ScreenWidth");
            }
        }

        public double ScreenHeight
        {
            get
            {
                return _screeHeight;
            }

            set
            {
                _screeHeight = value;
                this.OnPropertyChanged("ScreenHeight");
            }
        }

        #endregion
        #region Public Methods

        /// <summary>
        /// On change Card Number.
        /// </summary>
        /// <param name="newCardNumber">New Card number.</param>
        public void OnSmartCardNumberchanged(string newCardNumber)
        {
            if (!string.IsNullOrEmpty(newCardNumber))
            {
                this.CurrentCardNumber = newCardNumber;
                string currentViewModel = this.CurrentViewModel.GetType().Name.ToString();
                switch (currentViewModel)
                {
                    case ConstantValue.MainDishesViewModel:
                        ((MainDishesViewModel)CurrentViewModel).AddEmployeeOrderToList(newCardNumber);

                        break;
                    case ConstantValue.SideDishesViewModel:
                        break;
                    default:
                        break;
                }

                CheckCurrentScreen();
            }
        }

        #endregion

        #region Commands

        private bool CanExecuteMainDishesCommand()
        {
            return true;
        }

        private bool CanExecuteSideDishesCommand()
        {
            return true;
        }

        private bool CanExecuteChangeLanguageCommand()
        {
            return true;
        }

        /// <summary>
        /// Execute Command.
        /// </summary>
        private void ExecuteMainDishesCommand()
        {
            CurrentViewModel = new MainDishesViewModel();
            CheckCurrentViewModel();
        }

        private void ExecuteSideDishesCommand()
        {
            CurrentViewModel = new SideDishesViewModel();
            CheckCurrentViewModel();
        }

        private void ExecuteChangeLanguageCommand(object value)
        {
            if (value != null)
            {
                CurrentCulture = int.Parse((string)value);
                ChangeCulture();
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Set Default Menu Color.
        /// </summary>
        private void DefaulMenuColor()
        {
            MainDishesMenu.BackgroundColor = ConstantValue.MainDishesMenuColor;
            MainDishesMenu.ForegroundColor = ConstantValue.DefaultMenuColor;
            MainDishesMenu.FontWeight = ConstantValue.FontWeightNormal;

            SideDishesMenu.BackgroundColor = ConstantValue.SideDishesMenuColor;
            SideDishesMenu.ForegroundColor = ConstantValue.DefaultMenuColor;
            SideDishesMenu.FontWeight = ConstantValue.FontWeightNormal;
        }

        /// <summary>
        /// Check Current View Model.
        /// </summary>
        private void CheckCurrentViewModel()
        {
            DefaulMenuColor();
            string currentViewModel = this.CurrentViewModel.GetType().Name.ToString();
            switch (currentViewModel)
            {
                case ConstantValue.MainDishesViewModel:
                    MainDishesMenu.BackgroundColor = ConstantValue.DefaultMenuColor;
                    MainDishesMenu.ForegroundColor = ConstantValue.MainDishesMenuColor;
                    MainDishesMenu.FontWeight = ConstantValue.FontWeightBold;
                    break;
                case ConstantValue.SideDishesViewModel:
                    SideDishesMenu.BackgroundColor = ConstantValue.DefaultMenuColor;
                    SideDishesMenu.ForegroundColor = ConstantValue.SideDishesMenuColor;
                    SideDishesMenu.FontWeight = ConstantValue.FontWeightBold;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Change Culture.
        /// </summary>
        private void ChangeCulture()
        {
            switch (CurrentCulture)
            {
                case ConstantValue.EnglishCulture:
                    ResourceManagerService.ChangeLocale("en-GB");
                    CurrentCulture = ConstantValue.EnglishCulture;
                    break;
                case ConstantValue.VietNamCulture:
                    ResourceManagerService.ChangeLocale("vi-VN");
                    CurrentCulture = ConstantValue.VietNamCulture;
                    break;
                case ConstantValue.ChinaCulture:
                    ResourceManagerService.ChangeLocale("zh-CN");
                    CurrentCulture = ConstantValue.ChinaCulture;
                    break;
                default:
                    CurrentCulture = ConstantValue.EnglishCulture;
                    ResourceManagerService.ChangeLocale("vi-VN");
                    break;
            }
        }

        /// <summary>
        /// Show main Screen or Standby screen.
        /// </summary>
        private void CheckCurrentScreen()
        {
            if (string.IsNullOrEmpty(CurrentCardNumber))
            {
                MainScreenVisible = ConstantValue.Hidden;
                StandbyScreenVisible = ConstantValue.Visible;
            }
            else
            {
                MainScreenVisible = ConstantValue.Visible;
                StandbyScreenVisible = ConstantValue.Hidden;
            }
        }

        #endregion
    }
}
