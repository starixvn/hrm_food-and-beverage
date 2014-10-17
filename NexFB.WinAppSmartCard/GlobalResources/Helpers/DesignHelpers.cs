// --------------------------------------------------------------------------
// <copyright file="DesignHelpers.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.10.15">Created</change>
// </history>

namespace NexFB.WinAppSmartCard.GlobalResources.Helpers
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;

    /// <summary>
    /// Design Helpers.
    /// </summary>
    public static class DesignHelpers
    {
        private static bool? _isInDesignMode;

        [SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Justification = "Reviewed. Suppression is OK here.")]
        public static bool IsInDesignModeStatic
        {
            get
            {
                if (!_isInDesignMode.HasValue)
                {
#if SILVERLIGHT
                    _isInDesignMode = DesignerProperties.IsInDesignTool;
#else
                    var prop = DesignerProperties.IsInDesignModeProperty;
                    _isInDesignMode = (bool)DependencyPropertyDescriptor.FromProperty(prop, typeof(FrameworkElement)).Metadata.DefaultValue;

                    if (!_isInDesignMode.Value)
                    {
                    } 
                    
                    if (System.Diagnostics.Process.GetCurrentProcess().ProcessName.StartsWith(@"devenv", StringComparison.CurrentCulture))
                    {
                        _isInDesignMode = true;
                    }
#endif
                }

                return _isInDesignMode.Value;
            }
        }
    }
}
