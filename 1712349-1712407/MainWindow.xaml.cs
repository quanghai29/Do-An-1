using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
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
using Microsoft.WindowsAPICodePack.Dialogs;

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
        BindingList<StringOperation> _action = new BindingList<StringOperation>();
        BindingList<StringName> _fileName = new BindingList<StringName>();
        BindingList<StringName> _folderName = new BindingList<StringName>();
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

            var _prototype2 = new NewCaseOperation()
            {
                Args = new NewCaseArgs()
                {
                    Option = ""
                }
            };
            _prototype.Add(_prototype2);


            var _prototype3 = new FullnameNormalizeOperation();

            _prototype.Add(_prototype3);

            prototypeMethodCobobox.ItemsSource = _prototype;
            operationListBox.ItemsSource = _action;

           

            // ADD file 
            fileView.ItemsSource = _fileName;
            //ADD folder
            folderView.ItemsSource = _folderName;
        }

        private void Add_Method_Click(object sender, RoutedEventArgs e)
        {
            var action = prototypeMethodCobobox.SelectedItem as StringOperation;

            _action.Add(action);
        }

        
        private void AddFileNameButton_Click(object sender, RoutedEventArgs e)
        {
            
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = "C:\\Users\\DELL\\Desktop";
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() != CommonFileDialogResult.Ok)
            {
                MessageBox.Show("You must choose a folder");
                return;
            }
            string[] filePaths = Directory.GetFiles(dialog.FileName);
            //Reset lại

            if(_fileName.Count != 0)
            {
                _fileName.Clear();
            }
            _fileName.Clear();
            for (int i = 0; i < filePaths.Length; i++)
            {
                var infoName = new FileInfo(filePaths[i]);
                
                var filename = new StringName()
                {
                    Name = infoName.Name,
                    newName = "",
                    Path=infoName.FullName,
                    Error="",
                };
                _fileName.Add(filename);
            }

        }

        private void FolderNameButton_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = "C:\\Users\\DELL\\Desktop";
            dialog.IsFolderPicker = true;
            // Make a reference to a directory.
            if (dialog.ShowDialog() != CommonFileDialogResult.Ok)
            {
                MessageBox.Show("You must choose a folder");
                return;
            }
            DirectoryInfo di = new DirectoryInfo(dialog.FileName);
            // Get a reference to each directory in that directory.
            DirectoryInfo[] diArr = di.GetDirectories();

            //Reset lại
            if (_folderName.Count != 0)
            {
                _folderName.Clear();
            }
            // Display the names of the directories.
            foreach (DirectoryInfo dri in diArr)
            {
                var foldername = new StringName()
                {
                    Name = dri.Name,
                    newName = "",
                    Path = dri.FullName,
                    Error = "",
                };
                _folderName.Add(foldername);
            }

        }

        private void EditOperationItem_Click(object sender, RoutedEventArgs e)
        {
            var item = operationListBox.SelectedItem as StringOperation;
            string temp = item.Description;
            if(temp.Contains("Fullname Normalize"))
            {
                MessageBox.Show("This method can not edit!");
                return;
            }
            item.Config();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            var index= typeRename.SelectedIndex;
            if (index == 0)
            {

                rename(_fileName);
                fileView.ItemsSource = null;
                fileView.ItemsSource = _fileName;
            }
            else if(index==1)
            {
                rename(_folderName);
                folderView.ItemsSource = null;
                folderView.ItemsSource = _folderName;
            }
            else
            {
                //do no thing
            }
            
        }

        private void rename(BindingList<StringName> names)
        {
            var numberAction = _action.Count;
            if (numberAction == 0)
            {
                MessageBox.Show("Please add method first");
                return;
            }

            for(int i=0;i<numberAction;i++)
            {
                if(_action[i].Description.Contains("Hello"))
                {
                    MessageBox.Show($"Please choose a option for {_action[i].Name} method");
                    return;
                }
            }

            var numberName = names.Count;
            if(numberName == 0)
            {
                MessageBox.Show("Please add file or folder");
                return;
            }
            for (int i = 0; i < numberName; i++)
            {
                string final = names[i].Name;
                for (int j = 0; j < numberAction; j++)
                {
                    final = _action[j].Operation(final);
                }
                names[i].newName = final;
            }
        }
    }
}
