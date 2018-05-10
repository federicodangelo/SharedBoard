using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SharedBoard.Controls
{
    public sealed partial class StickyNote : UserControl
    {
        public StickyNote()
        {
            this.InitializeComponent();

            textBox.LostFocus += TextBox_LostFocus;
            this.DoubleTapped += StickyNote_DoubleTapped;

            textBox.Visibility = Visibility.Collapsed;
        }

        private void StickyNote_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            textBlock.Visibility = Visibility.Collapsed;
            textBox.Visibility = Visibility.Visible;
            textBox.Focus(FocusState.Pointer);
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            textBox.Visibility = Visibility.Collapsed;
            textBlock.Visibility = Visibility.Visible;
            textBlock.Text = textBox.Text;
        }

    }
}
