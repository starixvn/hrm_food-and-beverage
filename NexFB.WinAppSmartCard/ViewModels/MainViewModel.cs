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
    using System.Windows.Input;
    using NexFB.WinAppSmartCard.Core.Commands;
    using NexFB.WinAppSmartCard.Core.Constants;
    using NexFB.WinAppSmartCard.Core.Models;
    using NexFB.WinAppSmartCard.GlobalResources.Services;

    /// <summary>
    /// Main View Model.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private ViewModelBase _currentViewModel;
        private string _currentTime;
        private int _currentCulture;
        private MenuColor _mainDishesMenu = new MenuColor();
        private MenuColor _sideDishesMenu = new MenuColor();

        public MainViewModel()
        {
            ExecuteMainDishesCommand();
            MainDishesCommand = new RelayCommand((x) => CanExecuteMainDishesCommand(), (y) => ExecuteMainDishesCommand());
            SideDishesCommand = new RelayCommand((x) => CanExecuteSideDishesCommand(), (y) => ExecuteSideDishesCommand());
            ChangeLanguageCommand = new RelayCommand((x) => CanExecuteChangeLanguageCommand(), (y) => ExecuteChangeLanguageCommand());
            ChangeCulture();
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

        private void ExecuteChangeLanguageCommand()
        {
            ChangeCulture();
        }

        #endregion

        #region Methods

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
                    ResourceManagerService.ChangeLocale("vi-VN");
                    CurrentCulture = ConstantValue.VietNamCulture;
                break;
                case ConstantValue.VietNamCulture:
                     ResourceManagerService.ChangeLocale("zh-CN");
                    CurrentCulture = ConstantValue.ChinaCulture;
                break;
                case ConstantValue.ChinaCulture:
                    ResourceManagerService.ChangeLocale("en-GB");
                    CurrentCulture = ConstantValue.EnglishCulture;
                break;
                default:
                    CurrentCulture = ConstantValue.EnglishCulture;
                    ResourceManagerService.ChangeLocale("en-GB");
                break;
            }
        }

        #endregion
    }
}
