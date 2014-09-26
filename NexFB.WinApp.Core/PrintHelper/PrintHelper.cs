// --------------------------------------------------------------------------
// <copyright file="PrintHelper.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.8.29">Created</change>
// </history>

namespace NexFB.WinApp.Core.PrintHelper
{
    using System;
    using System.IO;
    using System.IO.Packaging;
    using System.Printing;
    using System.Printing.IndexedProperties;
    using System.Windows;
    using System.Windows.Documents;
    using System.Windows.Markup;
    using System.Windows.Threading;
    using System.Windows.Xps;
    using System.Windows.Xps.Packaging;
    using NexFB.Resources;

    /// <summary>
    /// Print Helper.
    /// </summary>
    public class PrintHelper : IPrintHelper
    {
        public void InjectData(FixedDocument document, object dataSource)
        {
            document.DataContext = dataSource;
            var dispatcher = Dispatcher.CurrentDispatcher;
            dispatcher.Invoke(
               DispatcherPriority.SystemIdle,
               new DispatcherOperationCallback(delegate { return null; }),
               null);
        }

        /// <summary>
        /// Method Convert Fixed Document to XPS file.
        /// </summary>
        /// <param name="fixedDoc">Fixed Document.</param>
        /// <param name="outputFile">Output file.</param>
        public void ConvertToXps(FixedDocument fixedDoc, string outputFile)
        {
            using (FileStream outputStream = File.Create(outputFile))
            {
                var package = Package.Open(outputStream, FileMode.Create);
                var xpsDoc = new XpsDocument(package, CompressionOption.Normal);
                XpsDocumentWriter xpsWriter = XpsDocument.CreateXpsDocumentWriter(xpsDoc);

                //// xps documents are built using fixed document sequences
                var fixedDocSeq = new FixedDocumentSequence();
                var docRef = new DocumentReference();
                docRef.BeginInit();
                docRef.SetDocument(fixedDoc);
                docRef.EndInit();
                ((IAddChild)fixedDocSeq).AddChild(docRef);

                //// write out our fixed document to xps
                xpsWriter.Write(fixedDocSeq.DocumentPaginator);

                xpsDoc.Close();
                package.Close();
            }
        }

        /// <summary>
        /// Print Invoice.
        /// </summary>
        /// <param name="documentPaginator">Document type.</param>
        /// <returns>Check is printed.</returns>
        public bool Print(DocumentPaginator documentPaginator)
        {
            LocalPrintServer localPrintServer = new LocalPrintServer(PrintSystemDesiredAccess.AdministrateServer);
            PrintQueue sourcePrintQueue = localPrintServer.DefaultPrintQueue;
            bool printed = false;
            if (sourcePrintQueue == null)
            {
                MessageBox.Show(WinFormMainResource.LocalPrinterError, "Errors", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                PrintPropertyDictionary localPrintProperties = sourcePrintQueue.PropertiesCollection;
                PrintBooleanProperty shared = new PrintBooleanProperty("IsShared", true);
                localPrintProperties.Remove("IsShared");
                localPrintProperties.Add("IsShared", shared);

                XpsDocumentWriter xpsDocumentWriter = PrintQueue.CreateXpsDocumentWriter(sourcePrintQueue);
                xpsDocumentWriter.Write(documentPaginator);
                printed = true;
            }

            sourcePrintQueue.Dispose();
            return printed;
        }

        /// <summary>
        /// Load Template from XAML file.
        /// </summary>
        /// <param name="templatePath">Path to XAML file.</param>
        /// <returns>An Object.</returns>
        public object LoadTemplate(string templatePath)
        {
            object template;

            // get the needed template paths
            string absolutePath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, templatePath);
            string directoryPath = Path.GetDirectoryName(absolutePath);
            using (FileStream inputStream = File.OpenRead(absolutePath))
            {
                var pc = new ParserContext
                {
                    BaseUri = new Uri(directoryPath + "\\")
                };
                template = XamlReader.Load(inputStream, pc);
            }

            return template;
        }
    }
}
