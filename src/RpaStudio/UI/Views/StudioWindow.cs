﻿using System.Windows;

namespace RpaStudio.UI.Views
{
    public class StudioWindow : Window
    {
        static StudioWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StudioWindow), new FrameworkPropertyMetadata(typeof(StudioWindow)));
        }
    }
}
