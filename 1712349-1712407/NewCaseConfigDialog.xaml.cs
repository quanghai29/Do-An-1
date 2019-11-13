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
    /// Interaction logic for NewCaseConfigDialog.xaml
    /// </summary>
    /// 
    public partial class NewCaseConfigDialog : Window
    {
        NewCaseArgs myArgs;

        static int flag = 0;
        public NewCaseConfigDialog(StringArgs args)
        {
            InitializeComponent();
            myArgs = args as NewCaseArgs;
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            if (flag == 0)
            {
                MessageBox.Show("Please choose a option!");
            }
            else
            {
                myArgs.Option = NewCaseComboBox.Text as String;
                flag = 0;
            }

            DialogResult = true;
            Close();
        }

        private void NewCaseComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            flag = 1;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
