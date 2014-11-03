// --------------------------------------------------------------------------
// <copyright file="OrderFoodDynamicScreen.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.11.3">Created</change>
// </history>

namespace NexFB.WinApp.ViewModels.Partials.OrderFood
{
    using System.Windows;

    /// <summary>
    /// Order Food Dynamic Screen.
    /// </summary>
    public class OrderFoodDynamicScreen : ViewModelBase
    {
        private static double _screenWidth = SystemParameters.PrimaryScreenWidth;
        private static double _screeHeight = SystemParameters.PrimaryScreenHeight;
        private static int _paddingValue5 = 5;
        private static int _paddingValue6 = 6;
        private static int _paddingValue8 = 8;
        private static int _paddingValue10 = 10;
        private static int _paddingValue15 = 15;
        private double _leftSide;
        private double _middleSide;
        private double _rightSide;

        private double _leftSideGeneralInfo;
        private double _leftSideActionButton;
        private double _leftSideKeyPad;

        private double _leftSideImageWidth;
        private double _leftSideImageHeight;
        private double _leftSideInfoDetails;

        private double _rightSideImageWidth;
        private double _rightSideImageHeight;
        private double _rightSideInfoDetails;
        private double _rightSideWrapper;
        private double _middleSideWrapper;

        private int _generalInfoFontSize;
        private int _keypadResultFontSize;

        private double _keyPadHeader;
        private double _keyPadResult;
        private double _keyPadMainButton;

        public OrderFoodDynamicScreen()
        {
        }

        #region Properties

        public double KeyPadMainButton
        {
            get
            {
                return _keyPadMainButton;
            }

            set
            {
                _keyPadMainButton = value;
                OnPropertyChanged("KeyPadMainButton");
            }
        }

        public double KeyPadHeader
        {
            get
            {
                return _keyPadHeader;
            }

            set
            {
                _keyPadHeader = value;
                OnPropertyChanged("KeyPadHeader");
            }
        }

        public double KeyPadResult
        {
            get
            {
                return _keyPadResult;
            }

            set
            {
                _keyPadResult = value;
                OnPropertyChanged("KeyPadResult");
            }
        }

        public double LeftSide
        {
            get
            {
                return _leftSide;
            }

            set
            {
                _leftSide = value;
                OnPropertyChanged("LeftSide");
            }
        }

        public double MiddleSide
        {
            get
            {
                return _middleSide;
            }

            set
            {
                _middleSide = value;
                OnPropertyChanged("MiddleSide");
            }
        }

        public double RightSide
        {
            get
            {
                return _rightSide;
            }

            set
            {
                _rightSide = value;
                OnPropertyChanged("RightSide");
            }
        }

        public double LeftSideGeneralInfo
        {
            get
            {
                return _leftSideGeneralInfo;
            }

            set
            {
                _leftSideGeneralInfo = value;
                OnPropertyChanged("LeftSideGeneralInfo");
            }
        }

        public double LeftSideActionButton
        {
            get
            {
                return _leftSideActionButton;
            }

            set
            {
                _leftSideActionButton = value;
                OnPropertyChanged("LeftSideActionButton");
            }
        }

        public double LeftSideKeyPad
        {
            get
            {
                return _leftSideKeyPad;
            }

            set
            {
                _leftSideKeyPad = value;
                OnPropertyChanged("LeftSideKeyPad");
            }
        }

        public double LeftSideImageWidth
        {
            get
            {
                return _leftSideImageWidth;
            }

            set
            {
                _leftSideImageWidth = value;
                OnPropertyChanged("LeftSideImageWidth");
            }
        }

        public double LeftSideImageHeight
        {
            get
            {
                return _leftSideImageHeight;
            }

            set
            {
                _leftSideImageHeight = value;
                OnPropertyChanged("LeftSideImageHeight");
            }
        }

        public double LeftSideInfoDetails
        {
            get
            {
                return _leftSideInfoDetails;
            }

            set
            {
                _leftSideInfoDetails = value;
                OnPropertyChanged("LeftSideInfoDetails");
            }
        }

        public int GeneralInfoFontSize
        {
            get
            {
                return _generalInfoFontSize;
            }

            set
            {
                _generalInfoFontSize = value;
                OnPropertyChanged("GeneralInfoFontSize");
            }
        }

        public int KeyPadResultFontSize
        {
            get
            {
                return _keypadResultFontSize;
            }

            set
            {
                _keypadResultFontSize = value;
                OnPropertyChanged("KeyPadResultFontSize");
            }
        }

        public double RightSideImageWidth
        {
            get
            {
                return _rightSideImageWidth;
            }

            set
            {
                _rightSideImageWidth = value;
                OnPropertyChanged("RightSideImageWidth");
            }
        }

        public double RightSideImageHeight
        {
            get
            {
                return _rightSideImageHeight;
            }

            set
            {
                _rightSideImageHeight = value;
                OnPropertyChanged("RightSideImageHeight");
            }
        }

        public double RightSideInfoDetails
        {
            get
            {
                return _rightSideInfoDetails;
            }

            set
            {
                _rightSideInfoDetails = value;
                OnPropertyChanged("RightSideInfoDetails");
            }
        }

        public double RightSideWrapper
        {
            get
            {
                return _rightSideWrapper;
            }

            set
            {
                _rightSideWrapper = value;
                OnPropertyChanged("RightSideWrapper");
            }
        }

        public double MiddleSideWrapper
        {
            get
            {
                return _middleSideWrapper;
            }

            set
            {
                _middleSideWrapper = value;
                OnPropertyChanged("MiddleSideWrapper");
            }
        }
        
        #endregion

        /// <summary>
        /// Initialize data.
        /// </summary>
        public void InitializeData()
        {
            LeftSide = RightSide = (_screenWidth * 37 / 100) - _paddingValue10;
            MiddleSide = _screenWidth - (LeftSide + RightSide) - _paddingValue6;

            LeftSideGeneralInfo = (_screeHeight * 30 / 100) - _paddingValue5;
            LeftSideKeyPad = (_screeHeight * 55 / 100) - _paddingValue5;
            LeftSideActionButton = _screeHeight - (LeftSideGeneralInfo + LeftSideKeyPad) - _paddingValue5;

            LeftSideImageWidth = 178;
            LeftSideImageHeight = 180;
            LeftSideInfoDetails = LeftSide - LeftSideImageWidth - _paddingValue5;

            RightSideImageWidth = RightSideImageHeight = 70;
            RightSideInfoDetails = RightSide - RightSideImageWidth - _paddingValue5;
            RightSideWrapper = RightSide - _paddingValue8;
            MiddleSideWrapper = MiddleSide - _paddingValue15;

            KeyPadHeader = 60;
            KeyPadResult = 60;
            KeyPadMainButton = LeftSideKeyPad - (KeyPadHeader + KeyPadResult) - 80;

            if (_screeHeight <= 768)
            {
                GeneralInfoFontSize = 17;
                KeyPadResultFontSize = 30;
            }

            if (_screeHeight > 768 && _screeHeight <= 800)
            {
                GeneralInfoFontSize = 18;
                KeyPadResultFontSize = 32;
            }

            if (_screeHeight > 800 && _screeHeight <= 960)
            {
                GeneralInfoFontSize = 19;
                KeyPadResultFontSize = 34;
            }

            if (_screeHeight > 960)
            {
                GeneralInfoFontSize = 20;
                KeyPadResultFontSize = 35;
            }
        }
    }
}
