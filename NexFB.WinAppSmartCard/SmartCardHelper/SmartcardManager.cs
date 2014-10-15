// --------------------------------------------------------------------------
// <copyright file="SmartcardManager.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.10.14">Created</change>
// </history>

namespace NexFB.WinAppSmartCard.SmartCardHelper
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;
    using System.Windows;
    using NexFB.WinAppSmartCard.Enums;

    /// <summary>
    /// Smart Card Manager.
    /// </summary>
    public class SmartcardManager : IDisposable
    {
        #region Member Fields

        private static readonly SmartcardManager _Instance = new SmartcardManager();
        private SmartcardContextSafeHandle _context;
        private SmartcardErrorCode _lastErrorCode;
        private bool _disposed = false;
        private ReaderState[] _states;
        private BackgroundWorker _worker;

        #endregion

        #region Methods

        public SmartcardManager()
        {
            this._context = new SmartcardContextSafeHandle();
            this.EstablishContext();
            ArrayList availableReaders = this.ListReaders();
            this._states = new ReaderState[availableReaders.Count];
            for (int i = 0; i <= availableReaders.Count - 1; i++)
            {
                this._states[i].Reader = availableReaders[i].ToString();
            }

            if (availableReaders.Count > 0)
            {
                this._worker = new BackgroundWorker();
                this._worker.WorkerSupportsCancellation = true;
                this._worker.DoWork += WaitChangeStatus;
                this._worker.RunWorkerAsync();
            }
        }

        #region Properties

        private bool HasContext
        {
            get { return !this._context.IsInvalid; }
        }

        #endregion

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Reviewed. Suppression is OK here.")]
        public static SmartcardManager GetManager(int number)
        {
            if (number == 1)
            {
                return _Instance;
            }

            return new SmartcardManager();
        }

        /// <summary>
        /// List Readers.
        /// </summary>
        /// <returns>Array List.</returns>
        public ArrayList ListReaders()
        {
            ArrayList result = new ArrayList();
            if (this.EstablishContext())
            {
                int size = this.GetReaderListBufferSize();
                string readerList = new string('\0', size);
                this._lastErrorCode =
                    (SmartcardErrorCode)UnsafeNativeMethods.ListReaders(this._context, null, readerList, ref size);
                if (this._lastErrorCode == SmartcardErrorCode.None)
                {
                    string readerName = string.Empty;
                    for (int i = 0; i <= readerList.Length - 1; i++)
                    {
                        if (readerList[i] == '\0')
                        {
                            if (readerName.Length > 0)
                            {
                                result.Add(readerName);
                                readerName = string.Empty;
                            }
                        }
                        else
                        {
                            readerName += new string(readerList[i], 1);
                        }
                    }
                }
            }

            return result;
        }

        // Implement IDisposable.
        // Do not make this method virtual.
        // A derived class should not be able to override this method.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// By Array to String Converter.
        /// </summary>
        /// <param name="data">Byte data convert.</param>
        /// <param name="attribute">Integer number.</param>
        /// <returns>String after convert.</returns>
        private static string ByteArrayToString(byte[] data, int attribute)
        {
            StringBuilder cardNumber;

            if (data != null)
            {
                cardNumber = new StringBuilder(data.Length * 2);
                for (int nI = 0; nI < attribute; nI++)
                {
                    cardNumber.AppendFormat("{0:X02}", data[nI]);
                }
            }
            else
            {
                cardNumber = new StringBuilder();
            }

            return cardNumber.ToString();
        }

        /// <summary>
        /// Establish Context.
        /// </summary>
        /// <returns>Boolean to check.</returns>
        private bool EstablishContext()
        {
            if (this.HasContext)
            {
                return true;
            }

            this._lastErrorCode =
                (SmartcardErrorCode)UnsafeNativeMethods.EstablishContext(ScopeOption.System, IntPtr.Zero, IntPtr.Zero, ref this._context);

            return this._lastErrorCode == SmartcardErrorCode.None;
        }

        /// <summary>
        /// Wait Change Status.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Do work event.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        private void WaitChangeStatus(object sender, DoWorkEventArgs e)
        {
            while (!e.Cancel)
            {
                SmartcardErrorCode result;
                lock (this)
                {
                    if (!this.HasContext)
                    {
                        return;
                    }

                    result = (SmartcardErrorCode)UnsafeNativeMethods.GetStatusChange(this._context, 1000, this._states, this._states.Length);
                }

                if (result == SmartcardErrorCode.Timeout)
                {
                    continue;
                }

                for (int i = 0; i <= this._states.Length - 1; i++)
                {
                    if ((this._states[i].EventState & CardState.Changed) == CardState.Changed)
                    {
                        SmartcardState state = SmartcardState.None;
                        if ((this._states[i].EventState & CardState.Present) == CardState.Present
                            && (this._states[i].CurrentState & CardState.Present) != CardState.Present)
                        {
                            state = SmartcardState.Inserted;
                        }
                        else if ((this._states[i].EventState & CardState.Empty) == CardState.Empty
                            && (this._states[i].CurrentState & CardState.Empty) != CardState.Empty)
                        {
                            state = SmartcardState.Ejected;
                        }

                        if (state != SmartcardState.None && this._states[i].CurrentState != CardState.None)
                        {
                            switch (state)
                            {
                                case SmartcardState.Inserted:
                                    {
                                        int attribute = _states[0].Attribute();
                                        MessageBox.Show(ByteArrayToString(_states[0].RGBAttribute(), attribute));
                                        break;
                                    }

                                case SmartcardState.Ejected:
                                    {
                                        break;
                                    }

                                default:
                                    {
                                        MessageBox.Show("Some other state...");
                                        break;
                                    }
                            }
                        }
                        ////Update the current state for the next time they are checked.
                        this._states[i].CurrentState = this._states[i].EventState;
                    }
                }
            }
        }

        #endregion

        #region IDisposable Support

        /// <summary>
        /// Dispose function.
        /// </summary>
        /// <param name="disposing">Boolean to check.</param>
        private void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                }

                this._states = null;
                this._worker.CancelAsync();
                this._worker.Dispose();
                this._context.Dispose();
            }

            this._disposed = true;
        }

        /// <summary>
        /// Get Reader List Buffer Size.
        /// </summary>
        /// <returns>An Integer number.</returns>
        private int GetReaderListBufferSize()
        {
            if (this._context.IsInvalid)
            {
                return 0;
            }

            int result = 0;
            this._lastErrorCode =
                (SmartcardErrorCode)UnsafeNativeMethods.ListReaders(this._context, null, null, ref result);
            return result;
        }

        #endregion
    }
}
