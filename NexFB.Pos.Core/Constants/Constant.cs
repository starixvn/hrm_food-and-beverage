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

        public const int ReceiveOrderFoodInvoice = 1;

        public const int DeliverOrderFoodInvoice = 2;

        public const int MainMenuId = 1;

        public const int AlternativeMenuId = 2;

        public const double BannerResolution1280 = 522;

        public const double BannerResolution1024 = 400;

        public const double RemainingWidth = 380;

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

        public const string DefaultButtonOrderBorderColor = "#fff";

        public const string DefaultButtonOrderColor = "#6cb23e";

        public const string SelectedButtonOrderColor = "#427720";

        public const string Hidden = "Hidden";

        public const string Visible = "Visible";

        public const string FontWeightBold = "Bold";

        public const string FontWeightNormal = "Normal";

        public const string WaitingMealViewModel = "WAITINGMEALVIEWMODEL";

        public const string AwaitingDeliverViewModel = "AWAITINGDELIVERVIEWMODEL";

        public const string DeliveredFoodViewModel = "DELIVEREDFOODVIEWMODEL";

        public const string CanceledFoodViewModel = "CANCELEDFOODVIEWMODEL";

        public const string OrderFoodViewModel = "ORDERFOODVIEWMODEL";

        public const int WaitingFoodScreen = 1;

        public const int AwaitingDeliverFoodScreen = 2;

        public const int DeliveredFoodScreen = 3;

        public const int GetNewOrder = 1;

        public const int GetAllData = 2;

        public const string DevMode = "DEV";

        public const string FinalMode = "RELEASE";

        public const string ConfigFilePath = "NexFB.WinApp.exe.config";

        public const string CancelImageActive = "pack://application:,,,/Images/button_cancel.png";

        public const string CancelImageDisabled = "pack://application:,,,/Images/button_cancel_disabled.png";

        public const string DefaultAvatar = "pack://application:,,,/Images/no-avatar.jpg";

        public const string NexcelAvatar = "pack://application:,,,/Images/Nexcel/{0}.jpg";

        public const string SmsAvatar = "pack://application:,,,/Images/SMS/{0}.jpg";

        public const string HongMAAvatar = "pack://application:,,,/Images/HongMa/{0}.jpg";

        public const string PosKitchenAssembly = "NexFB.PosKitchen";

        public const string DefaultImagePath = "images/no-avatar.jpg";

        public const string NexcelImagePath = "images/nexcel/{0}.jpg";

        public const string SmsImagePath = "images/sms/{0}.jpg";

        public const string HongMAImagePath = "images/hongma/{0}.jpg";

        public const string DefaultMealMenuSelected = "#46A3E1";

        public const string Expat = "EXPAT";

        public const string Local = "LOCAL";

        public const string UNRegisterMealColor = "#cdcdcd";

        public const string RegisterNonVegetarianMealColor = "#b1cde0";

        public const string RegisterVegetarianMealColor = "#c1e9a7";

        public const string HasOrderMealBeforeColor = "#f4c881";

        public const string FixedMeal = "Món Chính";

        public const string NA = "N/A";

        public const int ReleaseAllButton = 0;

        public const int PickRoomButton = 1;

        public const int PickGroupButton = 2;

        public const int ChooseMainFoodButton = 3;

        public const int ChooseAlternativeFoodButton = 4;

        public const int EnglishCulture = 1;

        public const int VietNamCulture = 2;

        private Constant()
        {
        }
    }
}
