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
using System.Windows.Shapes;

namespace _1712349_1712407
{
    /// <summary>
    /// Interaction logic for ISBNConfigDialog.xaml
    /// </summary>
    public partial class ISBNConfigDialog : Window
    {
        ISBNArgs myArgs;
        public ISBNConfigDialog(StringArgs args)
        {
            InitializeComponent();
            myArgs = args as ISBNArgs;
        }

        private void SubmitDirecting(object sender, RoutedEventArgs e)
        {
            if (beforeRadio.IsChecked == true)
            {
                myArgs.Direction = "before";
                this.DialogResult = true;
            }
            else if (afterRadio.IsChecked == true)
            {
                myArgs.Direction = "after";
                this.DialogResult = true;
            }
            else
            {
                MessageBox.Show("No direction selected!");
            }

        }
        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
