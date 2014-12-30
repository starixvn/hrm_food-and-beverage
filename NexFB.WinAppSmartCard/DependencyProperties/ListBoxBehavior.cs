// --------------------------------------------------------------------------
// <copyright file="ListBoxBehavior.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.12.30">Created</change>
// </history>

namespace NexFB.WinAppSmartCard.DependencyProperties
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// ListBox Behavior.
    /// </summary>
    public class ListBoxBehavior
    {
        public static readonly DependencyProperty ScrollOnNewItemProperty =
            DependencyProperty.RegisterAttached(
                "ScrollOnNewItem",
                typeof(bool),
                typeof(ListBoxBehavior),
                new UIPropertyMetadata(false, OnScrollOnNewItemChanged));

        private static Dictionary<ListBox, Capture> associations = new Dictionary<ListBox, Capture>();

        public static bool GetScrollOnNewItem(DependencyObject obj)
        {
            return (bool)obj.GetValue(ScrollOnNewItemProperty);
        }

        public static void SetScrollOnNewItem(DependencyObject obj, bool value)
        {
            obj.SetValue(ScrollOnNewItemProperty, value);
        }

        /// <summary>
        /// On Scroll On New Item.
        /// </summary>
        /// <param name="d">Dependency Object.</param>
        /// <param name="e">Dependency Property Changed.</param>
        public static void OnScrollOnNewItemChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var listBox = d as ListBox;
            if (listBox == null)
            {
                return;
            }

            bool oldValue = (bool)e.OldValue, newValue = (bool)e.NewValue;
            if (newValue == oldValue)
            {
                return;
            }

            if (newValue)
            {
                listBox.Loaded += new RoutedEventHandler(ListBox_Loaded);
                listBox.Unloaded += new RoutedEventHandler(ListBox_Unloaded);
            }
            else
            {
                listBox.Loaded -= ListBox_Loaded;
                listBox.Unloaded -= ListBox_Unloaded;
                if (associations.ContainsKey(listBox))
                {
                    associations[listBox].Dispose();
                }
            }
        }

        public static void ListBox_Unloaded(object sender, RoutedEventArgs e)
        {
            var listBox = (ListBox)sender;
            if (associations.ContainsKey(listBox))
            {
                associations[listBox].Dispose();
            }

            listBox.Unloaded -= ListBox_Unloaded;
        }

        /// <summary>
        /// ListBox Loaded.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Routed event arguments.</param>
        public static void ListBox_Loaded(object sender, RoutedEventArgs e)
        {
            var listBox = (ListBox)sender;
            var incc = listBox.Items as INotifyCollectionChanged;
            if (incc == null)
            {
                return;
            }

            listBox.Loaded -= ListBox_Loaded;
            associations[listBox] = new Capture(listBox);
        }
    }
}
