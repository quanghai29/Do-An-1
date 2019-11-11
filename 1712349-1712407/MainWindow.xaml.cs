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
        BindingList<StringFileName> _fileName = new BindingList<StringFileName>();
        BindingList<StringFolderName> _folderName = new BindingList<StringFolderName>();
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

            var _prototype2 = new ISBNOperation()
            {
                Args = new ISBNArgs()
                {
                    Direction = "before"
                }
            };
            _prototype.Add(_prototype2);
            prototypeMethodCobobox.ItemsSource = _prototype;
            operationListBox.ItemsSource = _action;

            var _prototype3 = new UniqueOperation()
            {

            };
            _prototype.Add(_prototype3);
            prototypeMethodCobobox.ItemsSource = _prototype;
            operationListBox.ItemsSource = _action;


            // ADD file 
            fileView.ItemsSource = _fileName;
            // ADD folder
            folderView.ItemsSource = _folderName;
        }

        private void Add_Method_Click(object sender, RoutedEventArgs e)
        {
            var action = prototypeMethodCobobox.SelectedItem as StringOperation;
            if (action == null)
            {
                MessageBox.Show("Please Select Method first!");
                return;
            }
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
                var file = new FileInfo(filePaths[i]);
                var filename = new StringFileName()
                {
                    infoName=file,
                    newName = "",
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
            foreach (DirectoryInfo d in diArr)
            {
                var foldername = new StringFolderName()
                {
                    dri = d,
                    newName = "",
                    Error = "",
                };
                _folderName.Add(foldername);
            }
        }

        private void EditOperationItem_Click(object sender, RoutedEventArgs e)
        {
            var item = operationListBox.SelectedItem as StringOperation;
            item.Config();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            var index= typeRename.SelectedIndex;
            if (index == 0)
            {
                var numberFile = _fileName.Count;
                if (!checkErorr(numberFile))
                    return;
                for (int i = 0; i < numberFile; i++)
                { 
                    string begin = _fileName[i].infoName.Name;
                    string final = begin;
                    var numberAction = _action.Count;
                    for (int j = 0; j < numberAction; j++)
                    {
                        final = _action[j].Operation(final);
                    }
                    //kiểm tra có thay đổi hay không
                    if (final != begin)
                    {
                        _fileName[i].newName = final;
                    }
                }
                fileView.ItemsSource = null;
                fileView.ItemsSource = _fileName;
            }
            else if(index==1)
            {
                var numberFolder = _folderName.Count;
                if (!checkErorr(numberFolder))
                    return;
                for (int i = 0; i < numberFolder; i++)
                {
                    string begin = _folderName[i].dri.Name;
                    string final = begin;
                    var numberAction = _action.Count;
                    for (int j = 0; j < numberAction; j++)
                    {
                        final = _action[j].Operation(final);
                    }
                    //Kiểm tra có thay đổi hay không
                    if (final != begin)
                    {
                        _folderName[i].newName = final;
                    }
                   
                }
                folderView.ItemsSource = null;
                folderView.ItemsSource = _folderName;
            }
            else
            {
                //do no thing
            }
            
        }

        private bool checkErorr(int number)
        {
            if (_action.Count == 0)
            {
                MessageBox.Show("Please add method first");
                return false;
            }
            if(number == 0)
            {
                MessageBox.Show("Please add file or folder");
                return false;
            }
            return true;
        }

        private void OkFileButton_Click(object sender, RoutedEventArgs e)
        {
            var numberFile = _fileName.Count;
            if (!checkErorr(numberFile))
                return;
            for (int i = 0; i < numberFile; i++)
            { 
                string newName = _fileName[i].newName;
                if (newName != "")
                {
                    newName = _fileName[i].infoName.Directory.FullName + "\\" + newName;
                    try
                    {
                        _fileName[i].infoName.MoveTo(newName);
                        _fileName[i].newName = "";
                    }
                    catch
                    {

                    }
                    
                }
            }
            fileView.ItemsSource = null;
            fileView.ItemsSource = _fileName;
        }

        private void OkFolderButton_Click(object sender, RoutedEventArgs e)
        {
            var numberFolder = _folderName.Count;
            if (!checkErorr(numberFolder))
                return;
            for (int i = 0; i < numberFolder; i++)
            {
                string newName = _folderName[i].newName;
                if (newName != "")
                {
                    newName = _folderName[i].dri.Parent.FullName + "\\" + newName;
                    try
                    {
                        _folderName[i].dri.MoveTo(newName);
                        _folderName[i].newName = "";
                    }
                    catch
                    {

                    }
                }
            }
            folderView.ItemsSource = null;
            folderView.ItemsSource = _folderName;
        }
    }
}
