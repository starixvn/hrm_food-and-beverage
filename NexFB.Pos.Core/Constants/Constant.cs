// --------------------------------------------------------------------------
// <copyright file="Constant.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.9.4">Created</change>
// </history>

namespace NexFB.Pos.Core.Constants
{
    /// <summary>
    /// Class Constant.
    /// </summary>
    public sealed class Constant
    {
        public const string OrderFoodReceiptTemplate = "PrintingTemplates\\OrderFoodReceipt.xaml";

        public const int DisabledEmployeeOrder = -1;

        public const int DefaultMainFoodValue = 1;

        public const double BannerResolution1280 = 522;

        public const double BannerResolution1024 = 400;

        public const double RemainingWidth = 373;

        public const string TimerAddQuantity = "TimerAddQuantity";

        public const string AllowMultipleInstance = "AllowMultipleInstance";

        public const string DefaultLanguage = "DefaultLanguage";

        public const string ReleaseMode = "ReleaseMode";

        public const string LocalDB = "LocalDB";

        public const string ServerDB = "ServerDB";

        public const string HubHost = "HubHost";

        public const string HubHostLocal = "HubHostLocal";

        public const string DatabaseDelayTime = "DBDelayTime";

        public const string AutoRefeshTime = "AutoRefeshTime";

        public const string DefaultCulture = "vi-VN";

        public const string EngCulture = "en-US";

        public const string Menu1Color = "#3A95FC";

        public const string Menu2Color = "#ef4339";

        public const string Menu3Color = "#cf4fde";

        public const string Menu4Color = "#39b54a";

        public const string Menu5Color = "#f39c11";

        public const string Red = "#FF0000";

        public const string Black = "#000";

        public const string DefaultMenuColor = "#fff";

        public const string DefaultButtonOrderColor = "#6cb23e";

        public const string SelectedButtonOrderColor = "#427720";

        public const string LocationContentMargin = "0,57,0,0";

        public const string OtherContentMargin = "0";

        public const string Hidden = "Hidden";

        public const string Visible = "Visible";

        public const string FontWeightBold = "Bold";

        public const string FontWeightNormal = "Normal";

        public const string WaitingMealViewModel = "WAITINGMEALVIEWMODEL";

        public const string AwaitingDeliverViewModel = "AWAITINGDELIVERVIEWMODEL";

        public const string DeliveredFoodViewModel = "DELIVEREDFOODVIEWMODEL";

        public const string CanceledFoodViewModel = "CANCELEDFOODVIEWMODEL";

        public const string OrderFoodViewModel = "ORDERFOODVIEWMODEL";

        public const string DevMode = "DEV";

        public const string FinalMode = "RELEASE";

        public const string ConfigFilePath = "NexFB.WinApp.exe.config";

        public const string CancelImageActive = "pack://application:,,,/Images/button_cancel.png";

        public const string CancelImageDisabled = "pack://application:,,,/Images/button_cancel_disabled.png";

        public const string DefaultAvatar = "Images\\no-avatar.jpg";

        public const string NexcelAvatar = "Images\\Nexcel\\{0}.jpg";

        public const string SmsAvatar = "Images\\SMS\\{0}.jpg";

        public const string HongMAAvatar = "Images\\HongMa\\{0}.jpg";

        public const string PosKitchenAssembly = "NexFB.PosKitchen";

        public const string DefaultImagePath = "images/no-avatar.jpg";

        public const string NexcelImagePath = "images/nexcel/{0}.jpg";

        public const string Expat = "EXPAT";

        public const string Local = "LOCAL";

        public const string UNRegisterMealColor = "#cdcdcd";

        public const string RegisterNonVegetarianMealColor = "#b1cde0";

        public const string RegisterVegetarianMealColor = "#c1e9a7";

        public const string HasOrderMealBeforeColor = "#f4c881";

        public const string FixedMeal = "Món Chính";

        public const string NA = "N/A";

        public const string AdditionalMenuTemplate = "<User SysId='{0}' FoodId='{1}' Qty='{2}'></User>";

        public const string MainMenuTemplate = "<User SysId='{0}' MenuId='{1}' Qty='{2}'></User>";

        public const string AlternativeMenuTemplate = "<User SysId='{0}' FoodId='{1}' Qty='{2}'></User>";

        public const string BeginRootTag = "<ROOT>";

        public const string EndRootTag = "</ROOT>";

        private Constant()
        {
        }
    }
}
