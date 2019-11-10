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
    /// Interaction logic for ReplaceConfigDialog.xaml
    /// </summary>
    public partial class ReplaceConfigDialog : Window
    {
        ReplaceArgs myArgs;
        public ReplaceConfigDialog(StringArgs args)
        {
            InitializeComponent();
            myArgs = args as ReplaceArgs;
        }

        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            if (fromTextBox.Text == "")
            {
                errorFromTextBlock.Text = "Please fill the 'old word'";
                return;
            }
            else
            {
                errorFromTextBlock.Text = "";
            }
            if (toTextBox.Text == "")
            {
                errorToTextBlock.Text = "Please fill the 'new word'";
                return;
            }
            else
            {
                errorToTextBlock.Text = "";
            }
            myArgs.From = fromTextBox.Text;
            myArgs.To = toTextBox.Text;
            DialogResult = true;
            Close();
        }
    }
}
