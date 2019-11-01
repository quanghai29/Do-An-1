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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _1712349_1712407
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        List<StringOperation> _prototype = new List<StringOperation>();
        BindingList<StringOperation> _action = new BingdingList<StringOperation>();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var _prototype1 = new ReplaceOperation()
            {
                Args = new ReplaceArgs()
                {
                    From = "origin string",
                    To = "new string"
                }
            };
            _prototype.Add(_prototype1);
            prototypeMethodCobobox.ItemsSource = _prototype;
            operationListBox.ItemsSource = _action;
        }

        private void Add_Method_Click(object sender, RoutedEventArgs e)
        {
            var action = prototypeMethodCobobox.SelectedItem as StringOperation;

            _action.Add(action);
        }
    }
}
