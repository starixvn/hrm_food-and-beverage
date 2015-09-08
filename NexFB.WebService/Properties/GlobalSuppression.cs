// --------------------------------------------------------------------------
// <copyright file="GlobalSuppression.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2015.9.8">Created</change>
// </history>

using System.Diagnostics.CodeAnalysis;

[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member",
    Target = "NexFB.WebService.Services.ISynchronizeDataService.#GetJobTitle()", Justification = "Reviewed. Suppression is OK here.")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member",
    Target = "NexFB.WebService.Services.ISynchronizeDataService.#GetShift()", Justification = "Reviewed. Suppression is OK here.")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member",
    Target = "NexFB.WebService.Services.ISynchronizeDataService.#GetLeaveType()", Justification = "Reviewed. Suppression is OK here.")]