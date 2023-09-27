using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RetailAppUI.Views.Administration
{
    /// <summary>
    /// Interaction logic for VendorView.xaml
    /// </summary>
    public partial class VendorView : UserControl
    {
        public VendorView()
        {
            InitializeComponent();
        }

        private void TxtNumericOnly_KeyDown(object sender, KeyEventArgs e)
        {
            //Limits text to digits only and a max length of 3 digits
            if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9))
            {
                //Allowed key presses
                e.Handled = false;
            }
            else if (e.Key == Key.Back)
            {
                e.Handled = false;
            }
            else if (e.Key == Key.Tab)
            {
                e.Handled = false;
            }
            else if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
            else
                    {
                        e.Handled = true;
                    }
        }

        private void TxtPhone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox box = (TextBox)sender;
            e.Handled = box.Text.Length > 2;
        }

        private void TxtPhoneSuffix_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox box = (TextBox)sender;
            if (box.Text.Length > 3)
            {
                e.Handled = true;
            }
            
        }
    }
}
