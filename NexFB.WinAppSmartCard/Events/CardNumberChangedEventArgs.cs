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
