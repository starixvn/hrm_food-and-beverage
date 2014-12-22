// --------------------------------------------------------------------------
// <copyright file="ConstantValue.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.10.16">Created</change>
// </history>

namespace NexFB.WinAppSmartCard.Core.Constants
{
    /// <summary>
    /// Constant Value.
    /// </summary>
    public sealed class ConstantValue
    {
        public const int CountDownTimer = 10;

        public const int NoMenuAvailableCode = -1;

        public const string MainDishesMenuColor = "#66B5D6";

        public const string SideDishesMenuColor = "#6CB23E";

        public const string DefaultMenuColor = "#fff";

        public const string FontWeightBold = "Bold";

        public const string FontWeightNormal = "Normal";

        public const string HubHost = "HubHost";

        public const string LocalDB = "LocalDB";

        public const string ServerDB = "ServerDB";

        public const string SessionTimeout = "SessionTimeout";

        public const string HubHostLocal = "HubHostLocal";

        public const string MainDishesViewModel = "MainDishesViewModel";

        public const string SideDishesViewModel = "SideDishesViewModel";

        public const string CombinedDishesViewModel = "CombinedDishesViewModel";

        public const string LogoMarginMoreThan960 = "0,-25,0,0";

        public const string LogoMargin960 = "0,-40,0,0";

        public const string LogoMarginLessThan900 = "0,-90,0,0";

        public const string SideDishOrderDetails = "<User FoodId='{0}' Qty='{1}'></User>";

        public const string MainDishOrderDetails = "<User SysId='{0}' MenuId='{1}' IsMainMenu='{2}'></User>";

        public const string CombinedSideDishOrderDetails = "<User SysId='{0}' FoodId='{1}' Qty='{2}'></User>";

        public const string BeginRootTag = "<ROOT>";

        public const string EndRootTag = "</ROOT>";

        public const string VietNameseCultureCode = "vi-VN";

        public const string ThemeXmas = "XMAS";

        public const string XmasBackground = "pack://application:,,,/Images/POS_IdleScreen_Xmas.jpg";

        public const string ThemeNornal = "NORMAL";

        public const string NormalBackground = "pack://application:,,,/Images/bg.jpg";

        public const string Themes = "Themes";

        private ConstantValue()
        {
        }
    }
}
