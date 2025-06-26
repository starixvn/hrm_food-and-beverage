namespace NexFB.WinAppSmartCard.SmartCardHelper
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Reader State.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    internal struct ReaderState
    {
        #region Member Fields
        [MarshalAs(UnmanagedType.LPWStr)]
        private string _reader;
        private IntPtr _userData;
        private CardState _currentState;
        private CardState _eventState;
        [MarshalAs(UnmanagedType.U4)]
        private int _attribute;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 36)]
        private byte[] _rgbAtr;

        #endregion

        #region "Properties"
        public string Reader
        {
            ////get { return this._reader; }
            set { this._reader = value; }
        }

        ////public IntPtr UserData
        ////{
        ////    ////get { return this._userData; }
        ////    set { this._userData = value; }
        ////}

        public CardState CurrentState
        {
            get { return this._currentState; }
            set { this._currentState = value; }
        }

        public CardState EventState
        {
            get { return this._eventState; }
        }
        #endregion

        #region Methods

        public byte[] RGBAttribute()
        {
            return this._rgbAtr;
        }

        public int Attribute()
        {
            return this._attribute;
        }
        #endregion
    }
}
