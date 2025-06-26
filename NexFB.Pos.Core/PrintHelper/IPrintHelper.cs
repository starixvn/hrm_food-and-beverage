namespace NexFB.Pos.Core.PrintHelper
{
    using System.Windows.Documents;

    /// <summary>
    /// Interface Print Helper.
    /// </summary>
    public interface IPrintHelper
    {
        void InjectData(FixedDocument document, object dataSource);

        void ConvertToXps(FixedDocument fixedDoc, string outputFile);

        bool Print(DocumentPaginator documentPaginator);

        object LoadTemplate(string templatePath);
    }
}
