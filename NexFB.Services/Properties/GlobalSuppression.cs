// --------------------------------------------------------------------------
// <copyright file="GlobalSuppression.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.8.29">Created</change>
// </history>

using System.Diagnostics.CodeAnalysis;

[module: SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", Scope = "namespace",
    Target = "NexFB.Services.Interface", MessageId = "Interface", Justification = "Reviewed. Suppression is OK here.")]
[module: SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", Scope = "namespace",
    Target = "NexFB.Services.Interface.Report", MessageId = "Interface", Justification = "Reviewed. Suppression is OK here.")]
