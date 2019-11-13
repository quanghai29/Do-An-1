using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;

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
        int ErrorFile = 0;// Đếm số lượng lỗi
        int ErrorFolder = 0;// Đếm số lượng lỗi 
        List<StringOperation> _prototype = new List<StringOperation>();
        
        BindingList<StringOperation> _action = new BindingList<StringOperation>();
        BindingList<StringFileName> _fileName = new BindingList<StringFileName>();
        BindingList<StringFolderName> _folderName = new BindingList<StringFolderName>();
        BindingList<HistoryAction> _preset = new BindingList<HistoryAction>();
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

            presetComboBox.ItemsSource = _preset;
        }

        private void Add_Method_Click(object sender, RoutedEventArgs e)
        {
            var action = prototypeMethodCobobox.SelectedItem as StringOperation;
            if (action == null)
            {
                MessageBox.Show("Please Select Method first!");
                return;
            }
            _action.Add(action.Clone());
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

        // Xử lý nút start Button
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
                    //Lần lượt thực hiện các điều kiện
                    string begin;
                    // lấy phần tên của  file 
                    begin=System.IO.Path.GetFileNameWithoutExtension(_fileName[i].infoName.FullName);
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

                        //Kiểm tra có trùng tên hay không 
                        var checkSameFile = _fileName.Where(x => x.infoName.Name == final);
                       
                        if (checkSameFile.Count() != 0)
                        {
                            var NotifyError = $"Name is a already exists on this location";
                            _fileName[i].Error = NotifyError;
                            ErrorFile++;
                        }
                    }
                }
                fileView.ItemsSource = null;
                fileView.ItemsSource = _fileName;

                //Vùng cảnh báo
                String NotifyWarning = $"!Warning\nThere are {ErrorFile} erorr \n If you want to continute, please press 'ok' else press 'cancle'";
                noteFileTextBox.Text = NotifyWarning;
            }
            else if(index==1)
            {
                var numberFolder = _folderName.Count;
                if (!checkErorr(numberFolder))
                    return;
                for (int i = 0; i < numberFolder; i++)
                {
                    //Lần lượt thực hiện các điều kiện 
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

                        //Kiểm tra có trùng tên hay không 
                        var checkSameFile = _folderName.Where(x => x.dri.Name == final);
                        if (checkSameFile.Count() != 0)
                        {
                            var NotifyError = $"There is a folder already exists in the same location";
                            _folderName[i].Error = NotifyError;
                            ErrorFolder++;
                        }
                    }
                   
                }
                folderView.ItemsSource = null;
                folderView.ItemsSource = _folderName;

                //Vùng cảnh báo
                String NotifyErorr = $"!Warning\nThere are {ErrorFolder} erorr \n If you want to continute, please press 'ok' else press 'cancle'";
                noteFolderTextBox.Text = NotifyErorr;
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

        // Xử lý nút Ok 
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
                        
                    }
                    catch
                    {

                    }
                    _fileName[i].newName = "";
                    if (_fileName[i].Error != "")
                    {
                        _fileName[i].Error = "";
                    }
                }
            }
            fileView.ItemsSource = null;
            fileView.ItemsSource = _fileName;
            noteFileTextBox.Text = "!No Warning";
            ErrorFile = 0;
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
                       
                    }
                    catch
                    {

                    }
                    _folderName[i].newName = "";
                    if (_folderName[i].Error != "")
                    {
                        _folderName[i].Error = "";
                    }
                }
            }
            folderView.ItemsSource = null;
            folderView.ItemsSource = _folderName;
            noteFolderTextBox.Text = "!No Warning";
            ErrorFolder = 0;
        }

        //Xử lý nút refresh  
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            
            var index = typeRename.SelectedIndex;
            if (index == 0)//view file
            {
                _fileName.Clear();
                _action.Clear();
                noteFileTextBox.Text = "!No Warning";
                ErrorFile = 0;
                presetComboBox.SelectedIndex = -1;
            }
            else if(index==1)//view folder
            {
                _folderName.Clear();
                _action.Clear();
                noteFolderTextBox.Text = "!No Warning";
                ErrorFile = 0;
                presetComboBox.SelectedIndex = -1;
            }
            else
            {
                //do no thing
            }
        }

        private void SaveActionButton_Click(object sender, RoutedEventArgs e)
        {
            string myAction = ""; 
            var screen = new saveActionDialog(myAction);
            if (screen.ShowDialog() == true)
            {
                // Truyền theo kiểu tà đạo
                myAction = screen.myNameAction;

                var checkSameName = _preset.Where(x => x.actionName == myAction);

                if (checkSameName.Count() != 0)
                {
                    MessageBox.Show($"{myAction} is a already exists ");
                    return;
                }

                var numberAction = _action.Count();
                var act = new BindingList<StringOperation>();
                for(int i = 0; i < numberAction; i++)
                {
                    act.Add(_action[i]);
                }
                var saveAction = new HistoryAction
                {
                    action = act,
                    actionName = myAction
                };

                
                _preset.Add(saveAction);

            }
        }

      
        private void PresetComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Bắt lỗi khi người dùng refresh 
            if (presetComboBox.SelectedIndex == -1)
                return;
            //Tải lại action đã lưu
            var myAction = presetComboBox.SelectedItem as HistoryAction;
            var numberAction = myAction.action.Count();

            _action.Clear();
            
            for(int i = 0; i < numberAction; i++)
            {
                _action.Add(myAction.action[i]);
            }
            
            operationListBox.ItemsSource = _action;
        }

       

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            var screen = new HelpDialog();
            screen.ShowDialog();
        }
    }
}
