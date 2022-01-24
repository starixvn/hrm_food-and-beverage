namespace NexFB.Pos.Core.PrintHelper
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

                sourcePrintQueue.Dispose();
            }

            return printed;
        }

        public object LoadTemplate(string templatePath)
        {
            object template;

            // get the needed template paths
            string absolutePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, templatePath);
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