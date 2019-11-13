using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for saveActionDialog.xaml
    /// </summary>
    public partial class saveActionDialog : Window
    {
        public string myNameAction;
        public saveActionDialog(string nameAction)
        {
            InitializeComponent();
            myNameAction = nameAction;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if(nameAction.Text=="")
            {
                erorr.Text = "Please fill your name action";
                return;
            }
            myNameAction = nameAction.Text ;
            DialogResult = true;
            Close();
        }

    }
}
