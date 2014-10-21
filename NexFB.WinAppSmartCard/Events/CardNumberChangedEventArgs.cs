// --------------------------------------------------------------------------
// <copyright file="CardNumberChangedEventArgs.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.10.20">Created</change>
// </history>

namespace NexFB.WinAppSmartCard.Events
{
    using System;

    /// <summary>
    /// Card Number Changed EventArgs.
    /// </summary>
    public class CardNumberChangedEventArgs : EventArgs
    {
        public CardNumberChangedEventArgs(string newCardNumber)
        {
            CardNumber = newCardNumber;
        }

        public string CardNumber { get; set; }
    }
}
