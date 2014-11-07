// --------------------------------------------------------------------------
// <copyright file="IPrintHelper.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.8.29">Created</change>
// </history>

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
