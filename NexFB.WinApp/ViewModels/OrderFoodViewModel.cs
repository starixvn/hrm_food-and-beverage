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

    /// <summary>
    /// Order Food ViewModel.
    /// </summary>
    public class OrderFoodViewModel : ViewModelBase
    {
        private string _result;

        public OrderFoodViewModel()
        {
        }

        public string Result
        {
            get
            {
                return _result;
            }

            private set
            {
                _result = value;
                this.OnPropertyChanged("Result");
            }
        }

        public ICommand PressAndRelease { get; set; }

        public bool CanExecutePressAndReleaseKey()
        {
            return true;
        }

        /// <summary>
        /// Method Press virtual key pad.
        /// </summary>
        /// <param name="obj">Object parameter.</param>
        public void ExecutePressAndReleaseKey(object obj)
        {
            switch (obj.ToString())
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
                    Result += "button.Content.ToString();";
                    break;
            }   
        }
    }
}
